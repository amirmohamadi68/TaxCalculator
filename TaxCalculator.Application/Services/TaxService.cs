using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaxCalculator.Application.DbContextInterfaces;
using TaxCalculator.Application.Dtos;
using TaxCalculator.Application.Services.Interfaces;
using TaxCalculator.Domain.Entities;

namespace TaxCalculator.Application.Services
{
    public class TaxService : ITaxService
    {
        private readonly ITaxDbContext _dbContext;

        public TaxService(ITaxDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<CityTaxRule?> GetCityById(long cityId)
        {
            return await _dbContext.CityTaxRules.FindAsync(cityId);
        }

        public async Task<CityTaxRule?> GetCityByName(string cityName)
        {
            return await _dbContext.CityTaxRules.Where(x => x.CityName.ToLower() == cityName.ToLower())
                .FirstOrDefaultAsync();
        }


        public async Task<long> GetTaxAmount(long cityId, TimeOnly time)
        {
            var taxAmount = await _dbContext.TaxAmounts.Where
                (x => x.CityTaxRuleId == cityId & x.StartTime <= time && x.EndTime >= time)
                .FirstOrDefaultAsync();

            return taxAmount.Amount;
        }

        public async Task<bool> IsHoliday(long cityId, DateOnly passDate)
        {
            return await _dbContext.Holidays.AnyAsync
                (x => x.CityTaxRuleId == cityId & x.Date == passDate);
        }

        public async Task<bool> IsTaxExemptVehicle(long cityId, string vehicleType)
        {
            return await _dbContext.TaxExemptVehicles.AnyAsync
                (x => x.CityTaxRuleId == cityId & x.VehicleType.ToLower() == vehicleType.ToLower());
        }

        public async Task<bool> IsTaxExemptDate(long cityId, DateOnly date)
        {
            var city = await GetCityById(cityId);

            // Public holidays
            if (city.IsHolidayTaxExempt)
            {
                if (await IsHoliday(cityId, date))
                    return true;
            }

            // A day before a public holiday
            if (city.IsDayBeforeHolidayTaxExempt)
            {
                DateOnly tomorrow = date.AddDays(1);
                if (await IsHoliday(cityId, tomorrow))
                    return true;
            }

            // During the month of July
            if (city.IsJulyTaxExempt)
            {
                if (date.Month == 7)
                    return true;
            }

            // Weekends
            if (city.IsWeekendTaxExempt)
            {
                if (date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday)
                {
                    return true;
                }
            }

            return false;
        }
        public async Task<long> CalculateTaxInOneDay(TaxCalculationDto request)
        {

            var city = await GetCityByName(request.CityName) ?? throw new ArgumentNullException(nameof(request.CityName)); // better way is create custome exception and handling that in exception handler midlware
            if (await IsTaxExemptVehicle(city.Id, request.VehicleType))
                return 0;

            request.PassesDates = request.PassesDates.OrderBy(date => date).ToArray();
            // years must be in 2013
            if (request.PassesDates.Any(d => d.Year > 2013))
                throw new Exception("Aplication works only in 2013 dates");

            DateOnly passDate = DateOnly.FromDateTime(request.PassesDates[0]);
            if (await IsTaxExemptDate(city.Id, passDate))
                return 0;

            long totalFee = 0;
            long currentMaxFee = 0;
            DateTime intervalStart = request.PassesDates[0];
            // Single charge rule does not apply
            if (city.SingleChargeDurationMinutes is 0)
            {
                foreach (DateTime date in request.PassesDates)
                {
                    TimeOnly time = TimeOnly.FromDateTime(date);
                    totalFee += await GetTaxAmount(city.Id, time);
                }
            }
            foreach (DateTime date in request.PassesDates)
            {
                TimeOnly time = TimeOnly.FromDateTime(date);
                long nextFee = await GetTaxAmount(city.Id, time);
                double minutesDiff = CalculateTimeDiff(intervalStart, date);

                CalcTotalFee(city.SingleChargeDurationMinutes, ref totalFee, ref currentMaxFee, ref intervalStart, date, nextFee, minutesDiff);
            }

            totalFee += currentMaxFee;

            //max fee in day rule
            return city.MaximumTaxPerDay != 0 && totalFee > city.MaximumTaxPerDay ? city.MaximumTaxPerDay : totalFee;

        }
        public async Task<long> CalculateTax(TaxCalculationDto request)
        {
            var dailyRequests = SplitByDays(request);
            long AlldatesFee = 0;
            foreach (var dailyRequest in dailyRequests)  // we could use parallel task here and then using whenAll to calcukate all of them
            {
                AlldatesFee += await CalculateTaxInOneDay(dailyRequest);
            }
            return AlldatesFee;
        }

        private static void CalcTotalFee(uint citySingleChargeDurationMinutes, ref long totalFee, ref long currentMaxFee, ref DateTime intervalStart, DateTime date, long nextFee, double minutesDiff)
        {
            if (minutesDiff <= citySingleChargeDurationMinutes)
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
        private static IEnumerable<TaxCalculationDto> SplitByDays(TaxCalculationDto request)
        {
            // Group PassedDate by date part only (ignoring time)
            var groupedByDay = request.PassesDates
                .GroupBy(date => date.Date)
                .Select(group => new TaxCalculationDto
                {
                    CityName = request.CityName,
                    VehicleType = request.VehicleType,
                    PassesDates = group.ToArray()
                });

            return groupedByDay;
        }
    }
}
}
