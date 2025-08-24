using CurrencyConversionAPI.Data;
using CurrencyConversionAPI.DTO;
using CurrencyConversionAPI.Models;
using Microsoft.EntityFrameworkCore;
namespace CurrencyConversionAPI.Services
{


    public class CurrencyService : ICurrencyService
    {
        private readonly AppDbContext _context;
        private readonly HttpClient _httpClient;
        private readonly ILogger<CurrencyService> _logger;

        private const string NationalbankenUrl = "https://www.nationalbanken.dk/_vti_bin/DN/DataService.svc/CurrencyRates";

        public CurrencyService(AppDbContext context, HttpClient httpClient, ILogger<CurrencyService> logger)
        {
            _context = context;
            _httpClient = httpClient;
            _logger = logger;
        }

        public async Task FetchAndUpdateRatesAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync(NationalbankenUrl);
                response.EnsureSuccessStatusCode();

                var content = await response.Content.ReadAsStringAsync();
                var rates = ParseRates(content);

                foreach (var rate in rates)
                {
                    var existing = await _context.CurrencyRates
                        .FirstOrDefaultAsync(r => r.CurrencyCode == rate.CurrencyCode);

                    if (existing != null)
                    {
                        existing.RateToDKK = rate.RateToDKK;
                        existing.LastUpdated = DateTime.UtcNow;
                    }
                    else
                    {
                        _context.CurrencyRates.Add(new CurrencyRate
                        {
                            CurrencyCode = rate.CurrencyCode,
                            RateToDKK = rate.RateToDKK,
                            LastUpdated = DateTime.UtcNow
                        });
                    }
                }

                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to fetch or update currency rates.");
            }
        }

        public async Task<List<CurrencyRateDto>> GetRatesAsync()
        {
            var rates = await _context.CurrencyRates.ToListAsync();
            return rates.Select(r => new CurrencyRateDto
            {
                CurrencyCode = r.CurrencyCode,
                RateToDKK = r.RateToDKK
            }).ToList();
        }

        public async Task<decimal> ConvertToDKKAsync(string fromCurrency, decimal amount)
        {
            var rate = await _context.CurrencyRates
                .FirstOrDefaultAsync(r => r.CurrencyCode == fromCurrency.ToUpper());

            if (rate == null)
                throw new ArgumentException($"Currency '{fromCurrency}' not found.");

            return Math.Round(amount * rate.RateToDKK, 2);
        }

        public async Task StoreConversionAsync(string fromCurrency, decimal originalAmount, decimal convertedAmount)
        {
            var record = new ConversionRecord
            {
                FromCurrency = fromCurrency.ToUpper(),
                OriginalAmount = originalAmount,
                ConvertedAmount = convertedAmount,
                Timestamp = DateTime.UtcNow
            };

            _context.ConversionRecords.Add(record);
            await _context.SaveChangesAsync();
        }

        public async Task<List<ConversionRecord>> GetConversionsAsync(string? currency, DateTime? from, DateTime? to)
        {
            var query = _context.ConversionRecords.AsQueryable();

            if (!string.IsNullOrEmpty(currency))
                query = query.Where(c => c.FromCurrency == currency.ToUpper());

            if (from.HasValue)
                query = query.Where(c => c.Timestamp >= from.Value);

            if (to.HasValue)
                query = query.Where(c => c.Timestamp <= to.Value);

            return await query.OrderByDescending(c => c.Timestamp).ToListAsync();
        }

        private List<CurrencyRateDto> ParseRates(string xmlContent)
        {
            // Placeholder: Implement XML parsing logic based on Nationalbanken's format
            // Return a list of CurrencyRateDto with CurrencyCode and RateToDKK
            return new List<CurrencyRateDto>
        {
            new CurrencyRateDto { CurrencyCode = "USD", RateToDKK = 6.85M },
            new CurrencyRateDto { CurrencyCode = "EUR", RateToDKK = 7.45M }
        };
        }
    }

}
