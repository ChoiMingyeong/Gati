using NLog;
using NLog.Targets;
using NLog.Targets.Wrappers;

namespace GatiLogger
{
    public sealed class LoggerOptions
    {
        // 공통
        public LogLevel MinLevel { get; set; } = LogLevel.Info;
        public bool EnableConsole { get; set; } = true;
        public bool EnableFile { get; set; } = true;

        // 비동기 출력
        public bool EnableAsync { get; set; } = true;
        public int QueueLimit { get; set; } = 10000;
        public AsyncTargetWrapperOverflowAction OverflowAction { get; set; } =
            AsyncTargetWrapperOverflowAction.Discard;

        // 라인 포맷 (텍스트)
        public string LineLayout { get; set; } =
            "${longdate} | ${level:uppercase=true} | ${logger} | ${message} ${exception:format=ToString}";

        // JSON 로깅
        public bool UseJson { get; set; } = false;
        public bool IncludeContext { get; set; } = true;

        // 파일 경로/이름
        public string FileNamePattern { get; set; } = "logs/app.log"; // ⬅️ 시간기반 롤링 쓰려면 ${shortdate} 쓰지 말 것

        // 아카이브(롤링)
        public FileArchivePeriod ArchiveEvery { get; set; } = FileArchivePeriod.None; // Day/Hour/…로 설정
        public long ArchiveAboveSize { get; set; } = 0;         // 사이즈 기준(바이트). 0이면 미사용
        public int MaxArchiveFiles { get; set; } = -1;          // 음수면 제한 없음, 0이면 보관 안 함
        public int MaxArchiveDays { get; set; } = 0;            // 0이면 미사용

        // ✅ NLog 6 방식: 시퀀스/날짜 조합을 문자열 포맷으로 지정
        // {0} = 시퀀스 번호, {1}=파일의 기준 날짜/시간(ArchiveEvery 사용 시)
        // 기본값은 "_{0:00}" (예: app_{01}.log)
        public string ArchiveSuffixFormat { get; set; } = "_{0:00}";

        // (선택) 레거시 방식: ArchiveFileName 사용(가능하면 비권장)
        // null이면 비활성. 예: "logs/archive/app.log" 또는 "logs/archive/app_${shortdate}.log"
        public string? LegacyArchiveFileName { get; set; } = null;

        // 기타
        public bool WriteFooterOnArchivingOnly { get; set; } = false;
        public bool DeleteOldFileOnStartup { get; set; } = false;
        public bool ArchiveOldFileOnStartup { get; set; } = false;
        public bool KeepFileOpen { get; set; } = true;
        public bool AutoFlush { get; set; } = true;
        public int BufferSize { get; set; } = 32768;
    }
}
