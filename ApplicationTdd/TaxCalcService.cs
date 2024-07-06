﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationTdd
{
    public class TaxCalcService
    {
        public bool IsFreeDate(DateTime date)
        {
            int year = date.Year;
            int month = date.Month;
            int day = date.Day;

            // weekends
            if (date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday)
            {
                return true;
            }

            // month of July
            if (month == 7)
            {
                return true;
            }

            // Public holidays in 2013
            var publicHolidays2013 = new HashSet<DateTime>
             {
                 new DateTime(2013, 1, 1),   // New Year's Day
                 new DateTime(2013, 3, 29),  // Good Friday
                 new DateTime(2013, 4, 1),   // Easter Monday
                 new DateTime(2013, 5, 1),   // Labour Day
                 new DateTime(2013, 5, 9),   // Ascension Day
                 new DateTime(2013, 6, 6),   // National Day
                 new DateTime(2013, 12, 25), // Christmas Day
                 new DateTime(2013, 12, 26)  // Boxing Day
             };

            if (publicHolidays2013.Contains(date.Date))
            {
                return true;
            }

            // Days before public holidays in 2013
            var daysBeforePublicHolidays2013 = new HashSet<DateTime>
             {
                 new DateTime(2013, 3, 28),  // Before Good Friday (March 29)
                 new DateTime(2013, 4, 30),  // Before Labour Day (May 1)
                 new DateTime(2013, 5, 8),   // Before Ascension Day (May 9)
                 new DateTime(2013, 6, 5),   // Before National Day (June 6)
                 new DateTime(2013, 12, 24), // Before Christmas (Dec 25)
                 new DateTime(2013, 12, 31)  // New Year's Eve
             };

            if (daysBeforePublicHolidays2013.Contains(date.Date))
            {
                return true;
            }

            return false;
        }
        public bool IsFreeVehicle(string vehicleType)
        {
            return Enum.IsDefined(typeof(TollFreeVehicles), vehicleType);
        }
        public int GetTollFee(int hour, int minute)
        {
            if (hour == 6 && minute >= 0 && minute <= 29) return 8;
            if (hour == 6 && minute >= 30 && minute <= 59) return 13;
            if (hour == 7) return 18;
            if (hour == 8 && minute >= 0 && minute <= 29) return 13;
            if ((hour == 8 && minute >= 30) || (hour > 8 && hour < 15)) return 8;
            if (hour == 15 && minute >= 0 && minute <= 29) return 13;
            if (hour == 15 && minute >= 30 || (hour == 16)) return 18;
            if (hour == 17) return 13;
            if (hour == 18 && minute >= 0 && minute <= 29) return 8;

            return 0;
        }
        public int CalculateTaxInOneDay(VehicleDetailsDto request)
        {
        
            if (IsFreeVehicle(request.VehicleType))
            {
                return 0;
            }

            request.PassedDate = request.PassedDate.OrderBy(date => date).ToArray();
            // years must be in 2013
            if (request.PassedDate.Any(d => d.Year > 2013))
                throw new Exception("Aplication works only in 2013 dates");

            if (IsFreeDate(request.PassedDate[0]))
            {
                return 0;
            }

            int totalFee = 0;
            int currentMaxFee = 0;
            DateTime intervalStart = request.PassedDate[0];

            foreach (DateTime date in request.PassedDate)
            {
                int nextFee = GetTollFee(date.Hour, date.Minute);
                double minutesDiff = CalculateTimeDiff(intervalStart, date);

                CalcTotalFee(ref totalFee, ref currentMaxFee, ref intervalStart, date, nextFee, minutesDiff);
            }

            totalFee += currentMaxFee;

            //max fee in day is 60
            return totalFee > 60 ? 60 : totalFee;
        }
        public int CalCulateTaxInDates(VehicleDetailsDto request)
        {
            var dailyRequests = SplitByDays(request);
            int AlldatesFee = 0;
            foreach (var dailyRequest in dailyRequests)
            {
                AlldatesFee+= CalculateTaxInOneDay(dailyRequest);
            }
            return AlldatesFee;
        }

        private static void CalcTotalFee(ref int totalFee, ref int currentMaxFee, ref DateTime intervalStart, DateTime date, int nextFee, double minutesDiff)
        {
            if (minutesDiff <= 60)
            {
                currentMaxFee = nextFee > currentMaxFee ? nextFee : currentMaxFee;
            }          
            else
            {
                totalFee += currentMaxFee;
                intervalStart = date;
                currentMaxFee = nextFee;
            }
        }

        private static double CalculateTimeDiff(DateTime intervalStart, DateTime date)
        {
            TimeSpan span = date.Subtract(intervalStart);
            double minutesDiff = span.TotalMinutes;
            return minutesDiff;
        }
        private static IEnumerable<VehicleDetailsDto> SplitByDays(VehicleDetailsDto request)
        {
            // Group PassedDate by date part only (ignoring time)
            var groupedByDay = request.PassedDate
                .GroupBy(date => date.Date)
                .Select(group => new VehicleDetailsDto
                {
                    VehicleType = request.VehicleType,
                    PassedDate = group.ToArray()
                });

            return groupedByDay;
        }
    }
}
