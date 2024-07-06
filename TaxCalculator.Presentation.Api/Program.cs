using Microsoft.Extensions.DependencyInjection.Extensions;
using TaxCalculator.Application;
using TaxCalculator.Persistance;
namespace TaxCalculator.Presentation.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddPersistance(builder.Configuration);
            builder.Services.AddApplicationDependency();
            // Add services to the container.

            builder.Services.AddControllers();

            var app = builder.Build();
            app.UseExceptionHandleMiddleware();
            // Configure the HTTP request pipeline.

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
