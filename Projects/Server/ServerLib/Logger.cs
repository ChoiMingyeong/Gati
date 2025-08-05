using Microsoft.Extensions.Logging;
using NLog;
using NLog.Config;
using NLog.Extensions.Logging;
using NLog.Web;
using System;
using System.IO;
using ILogger = Microsoft.Extensions.Logging.ILogger;
using LogLevel = Microsoft.Extensions.Logging.LogLevel;

namespace ServerLib
{
    public static class Logger
    {
        private static ILoggerFactory _loggerFactory;
        private static ISetupBuilder _nlogSetupBuilder;
        private static LogFactory _nLogFactory;
        private static ILogger _default;    // 기본 로그 저장. 이후 분리해서 추가 (Redis, Chat, Schedule 등)

        public static ILogger Default => _default; // { get; private set; }
        public static ILoggerFactory LogFactory => _loggerFactory;

        static Logger()
        {

        }

        public static void Init(string envName)
        {
            var logFilePath = Path.Join(AppContext.BaseDirectory, $"AppConfig/nlog.{envName}.xml");
            if (File.Exists(logFilePath) == false)
                logFilePath = Path.Join(AppContext.BaseDirectory, $"AppConfig/nlog.xml");

            _nLogFactory = NLog.Web.NLogBuilder.ConfigureNLog(logFilePath);
            //_nlogSetupBuilder = NLog.LogManager.Setup().LoadConfigurationFromAppSettings(logFilePath);
            _loggerFactory = LoggerFactory.Create(builder =>
            {
                //builder.AddNLogWeb(_nlogSetupBuilder.LogFactory.Configuration);
                builder.AddNLogWeb(_nLogFactory.Configuration);
            });

            _default = LogFactory.CreateLogger("ServerSystem");
            _default.LogTrace("init logger");
        }
    }
}
