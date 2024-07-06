using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaxCalculator.Domain.Entities;

namespace TaxCalculator.Persistance.ContextConfiguration
{
    public class CityTaxRuleConfiguration : IEntityTypeConfiguration<CityTaxRule>
    {
        public void Configure(EntityTypeBuilder<CityTaxRule> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.CityName)
                .IsRequired();

            builder.HasIndex(x => x.CityName)
                .IsUnique();
        }
    }
}
