using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaxCalculator.Domain.Entities;

namespace TaxCalculator.Application.DbContextInterfaces
{
    public interface ITaxDbContext
    {
        public DbSet<CityTaxRule> CityTaxRules { get; set; }
        public DbSet<TaxAmount> TaxAmounts { get; set; }
        public DbSet<TaxExemptVehicle> TaxExemptVehicles{ get; set; }
        public DbSet<Holiday> Holidays{ get; set; }


    }
}
