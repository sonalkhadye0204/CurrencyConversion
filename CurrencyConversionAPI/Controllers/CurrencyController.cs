using Microsoft.AspNetCore.Mvc;
using CurrencyConversionAPI.Services;
using CurrencyConversionAPI.DTO;

namespace CurrencyApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CurrencyController : ControllerBase
    {
        private readonly ICurrencyService _service;

        public CurrencyController(ICurrencyService service) => _service = service;

        [HttpGet("rates")]
        public async Task<IActionResult> GetRates() => Ok(await _service.GetRatesAsync());

        [HttpPost("convert")]
        public async Task<IActionResult> Convert([FromBody] ConversionRequestDto request)
        {
            var result = await _service.ConvertToDKKAsync(request.FromCurrency, request.Amount);
            await _service.StoreConversionAsync(request.FromCurrency, request.Amount, result);
            return Ok(new { ConvertedAmount = result });
        }

        [HttpGet("conversions")]
        public async Task<IActionResult> GetConversions([FromQuery] string? currency, [FromQuery] DateTime? from, [FromQuery] DateTime? to)
        {
            var records = await _service.GetConversionsAsync(currency, from, to);
            return Ok(records);
        }
    }

}
