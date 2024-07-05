using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaxCalculator.Application.DbContextInterfaces;
using TaxCalculator.Application.Services.Interfaces;
using TaxCalculator.Persistance.DbContexts;

namespace TaxCalculator.Persistance
{

    public static class DependencyInjection
    {
        public static IServiceCollection AddPersistance( IServiceCollection services, IConfiguration configuration)
        {
            // better way is catching Iconfiguration in IOption to avoid reading in each scope
            var connectionString = configuration.GetConnectionString("DefaultConnection");
            services.AddDbContext<TaxDbContext>(options => options.UseNpgsql(connectionString, o => o.UseNodaTime()));
            services.AddHealthChecks().AddNpgSql(connectionString, "TaxDBContext");
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
            services.AddScoped<ITaxDbContext, TaxDbContext>();
            return services;
        }

    }
}
