using CurrencyConversionAPI.Services;

namespace CurrencyConversionAPI.Jobs
{
    public class CurrencyRateUpdaterJob : BackgroundService
    {
        private readonly IServiceProvider _services;

        public CurrencyRateUpdaterJob(IServiceProvider services) => _services = services;

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                using var scope = _services.CreateScope();
                var service = scope.ServiceProvider.GetRequiredService<ICurrencyService>();
                await service.FetchAndUpdateRatesAsync();
                await Task.Delay(TimeSpan.FromHours(1), stoppingToken);
            }
        }
    }

}
