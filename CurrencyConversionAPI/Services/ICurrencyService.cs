using CurrencyConversionAPI.DTO;
using CurrencyConversionAPI.Models;

namespace CurrencyConversionAPI.Services
{
    public interface ICurrencyService
    {
        Task FetchAndUpdateRatesAsync();
        Task<List<CurrencyRateDto>> GetRatesAsync();
        Task<decimal> ConvertToDKKAsync(string fromCurrency, decimal amount);
        Task StoreConversionAsync(string fromCurrency, decimal originalAmount, decimal convertedAmount);
        Task<List<ConversionRecord>> GetConversionsAsync(string? currency, DateTime? from, DateTime? to);
    }

}
