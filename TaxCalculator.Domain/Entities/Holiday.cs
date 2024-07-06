using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaxCalculator.Domain.Common;

namespace TaxCalculator.Domain.Entities
{
    public record Holiday : EntityBase
    {
        public long CityTaxRuleId { get; set; }

        public CityTaxRule CityTaxRule { get; set; } = null!;

        public DateOnly Date { get; set; }

        public string? Description { get; set; }
    }
}
