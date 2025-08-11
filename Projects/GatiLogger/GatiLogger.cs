using NLog;

namespace GatiLogger
{
    public sealed class GatiLogger
    {
        private static Lazy<GatiLogger>? _instance = null;
        public static GatiLogger Instance
        {
            get
            {
                _instance ??= new Lazy<GatiLogger>(() => new GatiLogger());
                return _instance.Value;
            }
        }

        private readonly Logger _logger = LogManager.GetCurrentClassLogger();
        private bool _isInitialized = false;

        public void Initialize()
        {
            if (_isInitialized)
            {
                return;
            }

            _isInitialized = true;
        }

        public void Info(string message) => _logger.Info(message);

        public void Warn(string message) => _logger.Warn(message);

        public void Error(string message) => _logger.Error(message);

        public void Debug(string message) => _logger.Debug(message);

        public void Fatal(string message) => _logger.Fatal(message);

        public void Error(Exception ex, string message = "")
        {
            if (string.IsNullOrEmpty(message))
            {
                _logger.Error(ex);
            }
            else
            {
                _logger.Error(ex, message);
            }
        }
    }
}
