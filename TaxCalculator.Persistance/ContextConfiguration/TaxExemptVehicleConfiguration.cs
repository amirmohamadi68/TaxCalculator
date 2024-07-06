﻿using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaxCalculator.Domain.Entities;

namespace TaxCalculator.Persistance.ContextConfiguration
{

    public class TaxExemptVehicleConfiguration : IEntityTypeConfiguration<TaxExemptVehicle>
    {
        public void Configure(EntityTypeBuilder<TaxExemptVehicle> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.CityTaxRuleId)
                .IsRequired();

            builder.Property(x => x.VehicleType)
                .IsRequired();
        }
    }

}
