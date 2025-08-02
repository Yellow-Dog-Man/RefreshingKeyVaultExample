using Microsoft.Extensions.Options;
using Patreon.NET;

namespace RefreshingKeyVaultExample
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> logger;
        private readonly IOptionsMonitor<DummyPatreonKeys> keysMonitor;
        private PatreonClient patreonClient;
        private DateTimeOffset lastKeyUpdate;

        public Worker(ILogger<Worker> logger, IOptionsMonitor<DummyPatreonKeys> keys)
        {
            this.logger = logger;
            keysMonitor = keys;

            patreonClient = MakeClient(keysMonitor.CurrentValue);
            keysMonitor.OnChange(PatreonKeysChanged);

            logger.LogInformation($"Starting Up RefreshingKeyVaultExample");
        }

        private void PatreonKeysChanged(DummyPatreonKeys keys, string? _)
        {
            logger.LogInformation("Refreshed Patreon Keys at: {time}", DateTimeOffset.Now);
            patreonClient = MakeClient(keys);
        }

        private PatreonClient MakeClient(DummyPatreonKeys keys)
        {
            lastKeyUpdate = DateTimeOffset.Now;
            keys.LogTokens(logger);
            return new PatreonClient(keys.CampaignId, keys.AccessToken);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                logger.LogInformation("Worker running at: {time}, last key update: {update}", DateTimeOffset.Now, lastKeyUpdate);
                await Task.Delay(10 * 1000, stoppingToken);
            }
        }
    }
}
