using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaxCalculator.Application.Dtos;
using TaxCalculator.Domain.Entities;

namespace TaxCalculator.Application.Services.Interfaces
{
    public interface ITaxService
    {
        
        Task<long> CalculateTax(TaxCalculationDto dto);

        Task<CityTaxRule?> GetCityByName(string cityName);

        Task<CityTaxRule?> GetCityById(long cityId);

        Task<long> GetTaxAmount(long cityId, TimeOnly time);

        Task<bool> IsHoliday(long cityId, DateOnly passDate);

        Task<bool> IsTaxExemptVehicle(long cityId, string vehicleType);

        Task<bool> IsTaxExemptDate(long cityId, DateOnly date);
    }

}
