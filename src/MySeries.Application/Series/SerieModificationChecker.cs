using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MySeries.Service;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace MySeries.Series
{
    public class SerieModificationChecker : IHostedService, IDisposable
    {
        private readonly ILogger<SerieModificationChecker> _logger;
        private readonly ISerieUpdateService _serieUpdateService;
        private Timer _timer;
        private bool _isRunning;

        public SerieModificationChecker(
            ILogger<SerieModificationChecker> logger,
            ISerieUpdateService serieUpdateService)
        {
            _logger = logger;
            _serieUpdateService = serieUpdateService;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            if (_isRunning)
            {
                return Task.CompletedTask;
            }

            _logger.LogInformation("SerieUpdateChecker starting...");
            _timer = new Timer(DoWork, null, TimeSpan.Zero, TimeSpan.FromMilliseconds(86400000));
            _isRunning = true;
            return Task.CompletedTask;
        }

        public void DoWork(object state)
        {
            _ = DoWorkAsync(state);
        }

        private async Task DoWorkAsync(object state)
        {
            if (_serieUpdateService == null)
            {
                _logger.LogWarning("SerieUpdateService is not initialized.");
                return;
            }

            _logger.LogInformation("SerieUpdateChecker running verification.");
            await _serieUpdateService.VerificarYActualizarSeriesAsync();
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("SerieUpdateChecker stopping.");
            _timer?.Change(Timeout.Infinite, 0);
            _isRunning = false;
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }
    }
}
