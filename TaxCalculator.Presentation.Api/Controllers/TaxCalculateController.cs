using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TaxCalculator.Application.Dtos;
using TaxCalculator.Application.Services.Interfaces;

namespace TaxCalculator.Presentation.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TaxCalculateController : ControllerBase
    {
        private readonly ITaxService _taxService;

        public TaxCalculateController(ITaxService taxService)
        {
            _taxService = taxService;
        }

        [HttpPost]
        public async Task<IActionResult> CalculateTax([FromBody] TaxCalculationDto taxCalculationDto)
        {
            var result = await _taxService.CalculateTax(taxCalculationDto);
            return Ok(result);
        }
    }
}
