using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using TaxCalculator.Application.Dtos;
using TaxCalculator.Persistance.DbContexts;
using TaxCalculator.Presentation.Api;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace IntegrationTest
{
    public class TaxCalculateControllerTests : IClassFixture<WebApplicationFactory<TaxCalculator.Presentation.Api.Controllers.TaxCalculateController>>
    {
        private readonly HttpClient _client;
        private WebApplicationFactory<Program> _factory;

        public TaxCalculateControllerTests(WebApplicationFactory<TaxCalculator.Presentation.Api.Controllers.TaxCalculateController> factory)
        {
            _factory = new WebApplicationFactory<Program>()
         .WithWebHostBuilder(builder =>
         {
             builder.ConfigureServices(services =>
             {
                 // Remove the app's MyDbContext registration.
                 var descriptor = services.SingleOrDefault(
                 d => d.ServiceType ==
                         typeof(DbContextOptions<TaxDbContext>));

                 if (descriptor != null)
                 {
                     services.Remove(descriptor);
                 }

                 // Add MyDbContext using an in-memory database for testing.
                 services.AddDbContext<TaxDbContext>(options =>
                 {
                     options.UseInMemoryDatabase("TestDatabase");
                 });

                 // Build the service provider.
                 var sp = services.BuildServiceProvider();

                 // Create a scope to obtain a reference to the database
                 // context (MyDbContext).
                 using var scope = sp.CreateScope();
                 var scopedServices = scope.ServiceProvider;
                 var db = scopedServices.GetRequiredService<TaxDbContext>();

                 // Ensure the database is created.
                 db.Database.EnsureCreated();

                 // Seed the database with test data.
             
             });
         });

            _client = _factory.CreateClient();
        }

        [Fact]
        public async Task CalculateTax_ReturnsCorrectTaxAmount()
        {
            // Arrange
            var taxCalculationDto = new TaxCalculationDto
            {
                CityName = "Gothenburg",
                VehicleType = "Car",
                PassesDates = new List<DateTime>
                {
                    new DateTime(2013, 5, 13, 6, 15, 0),
                    new DateTime(2013, 5, 13, 8, 45, 0),
                    new DateTime(2013, 5, 14, 9, 0, 0),
                    new DateTime(2013, 5, 14, 12, 30, 0),
                    new DateTime(2013, 5, 15, 14, 0, 0)
                }
            };

            // Act
            var response = await _client.PostAsJsonAsync("/api/taxcalculate", taxCalculationDto);

            // Assert
            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadAsStringAsync();
            Assert.Equal("40", result); // Replace with the actual expected tax amount
        }
    }

}
