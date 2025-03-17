using Microsoft.Extensions.Logging;
using System.Collections.Concurrent;

namespace Extensions.Logging.Prime
{
    internal sealed class PrimeLoggerProvider : ILoggerProvider
    {
        private readonly ConcurrentDictionary<string, PrimeLogger> _loggers = new(StringComparer.OrdinalIgnoreCase);
        private readonly IServiceProvider _serviceProvider;

        public PrimeLoggerProvider(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public ILogger CreateLogger(string categoryName) => _loggers.GetOrAdd(categoryName, name => new PrimeLogger(name, _serviceProvider));

        public void Dispose()
        {
            _loggers.Clear();
        }
    }
}
