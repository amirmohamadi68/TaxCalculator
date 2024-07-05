using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaxCalculator.Application.Services;
using TaxCalculator.Application.Services.Interfaces;

namespace TaxCalculator.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplicationDependency(IServiceCollection services)
        {
            services.AddScoped<ITaxService , TaxService>();
            return services;
        }
    }
}
