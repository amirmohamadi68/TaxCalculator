using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaxCalculator.Application.Dtos
{
    public record TaxCalculationDto
    {
        public string CityName { get; set; } = null!;

        public string VehicleType { get; set; } = null!;

        public IList<DateTime> PassesDates { get; set; } = null!;
    }
}
