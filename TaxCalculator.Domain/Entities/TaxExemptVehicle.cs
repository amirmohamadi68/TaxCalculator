using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaxCalculator.Domain.Common;

namespace TaxCalculator.Domain.Entities
{
    public record TaxExemptVehicle : EntityBase
    {
        public long CityTaxRuleId { get; set; }

        public CityTaxRule CityTaxRule { get; set; } = null!;

        public string VehicleType { get; set; } = null!;
    }
}
