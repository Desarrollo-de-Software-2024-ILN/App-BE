using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace MySeries.Updates
{
    public class SerieUpdateChecker : IHostedService, IDisposable
    {
        private readonly ILogger<SerieUpdateChecker> _logger;
        private readonly ISerieUpdateChecker _serieUpdateChecker;
        private Timer _timer;
        private bool _checkTimer;

        public SerieUpdateChecker(ILogger<SerieUpdateChecker> logger, ISerieUpdateChecker serieUpdateChecker)
        {
            _logger = logger;
            _serieUpdateChecker = serieUpdateChecker;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            if (_checkTimer) 
            {
                return Task.CompletedTask;
            }

            _logger.LogInformation("SerieUpdateChecker starting.");
            _timer = new Timer(DoWork, null, TimeSpan.Zero, TimeSpan.FromMilliseconds(86400000));
            _checkTimer = true; 
            return Task.CompletedTask;
        }

        public void DoWork(object state)
        {
            _ = DoWorkAsync(state);
        }

        public async Task DoWorkAsync(object state)
        {
            if (_serieUpdateChecker == null)
            {
                _logger.LogWarning("SerieUpdateService no esta inicializado");
                return;
            }

            _logger.LogInformation("SerieUpdateService verificando");
            await _serieUpdateChecker.UpdateSeriesAsync();
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("SerieUpdateChecker stopping.");
            _timer?.Change(Timeout.Infinite, 0);
            _checkTimer = false; // Marcar como no en ejecución
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }
    }
}
