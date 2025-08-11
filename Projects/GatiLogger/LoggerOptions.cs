using NLog;

namespace GatiLogger
{
    public sealed class LoggerOptions
    {
        public LogLevel MinLevel { get; set; } = LogLevel.Info;

        public bool EnableConsole { get; set; } = true;

        public bool EnableFile { get; set; } = true;

        public bool EnableAsync { get; set; } = true;

        public string LineLayout { get; set; } = "${longdate} | ${level:uppercase=true} | ${logger} | ${message} ${exception:format=ToString}";
    }
}
