using Microsoft.Extensions.Logging;
using SGI.Application.Interfaces;

namespace SGI.Application.Services
{
    public class LoggerService : ILoggerService
    {
        private readonly ILogger<LoggerService> _logger; 

        public LoggerService(ILogger<LoggerService> logger)
        {
            _logger = logger;
        }

        public void LogInfo(string mensaje) => _logger.LogInformation(mensaje);
        public void LogWarning(string mensaje) => _logger.LogWarning(mensaje);
        public void LogError(Exception ex, string mensaje) => _logger.LogError(ex, mensaje);
    }
}