using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaxCalculator.Domain.Entities;

namespace TaxCalculator.Persistance.ContextConfiguration
{

    public class TaxAmountConfiguration : IEntityTypeConfiguration<TaxAmount>
    {
        public void Configure(EntityTypeBuilder<TaxAmount> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.CityTaxRuleId)
                .IsRequired();

            builder.Property(x => x.StartTime)
                .IsRequired();

            builder.Property(x => x.EndTime)
                .IsRequired();

            builder.Property(x => x.Amount)
                .IsRequired();
        }
    }
}
