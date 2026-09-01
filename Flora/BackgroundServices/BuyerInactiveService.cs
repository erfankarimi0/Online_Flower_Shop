using Flora.Data;
using Flora.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Flora.BackgroundServices
{
    public class BuyerInactiveService : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;

        public BuyerInactiveService(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                using var scope = _scopeFactory.CreateScope();

                var context = scope.ServiceProvider
                    .GetRequiredService<FloraContext>();

                var buyers = await context.Buyers
                    .Where(p =>
                        p.Status == BuyerStatus.Active.ToString() &&
                        p.LastLoginDate != null &&
                        p.LastLoginDate < DateTime.UtcNow.AddDays(-30))
                    .ToListAsync(stoppingToken);

                foreach (var buyer in buyers)
                {
                    buyer.Status = BuyerStatus.Inactive.ToString();
                }

                await context.SaveChangesAsync(stoppingToken);

                await Task.Delay(
                    TimeSpan.FromDays(1),
                    stoppingToken);
            }
        }
    }
}