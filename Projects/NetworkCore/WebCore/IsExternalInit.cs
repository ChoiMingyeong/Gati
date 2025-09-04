// IsExternalInit.cs
#if !NET5_0_OR_GREATER
namespace System.Runtime.CompilerServices
{
    // C# 9 init / record 지원용 폴리필 (참조 전용 타입)
    internal static class IsExternalInit { }
}
#endif
