using System.IO.Compression;

namespace GatiArchive.Compression
{
    /// <summary>
    /// 일반 용도 (텍스트/로그/일반 리소스용)
    /// <br/>압축률     : ⭐⭐⭐
    /// <br/>압축 속도  : ⭐⭐⭐⭐
    /// <br/>해제 속도  : ⭐⭐⭐⭐⭐
    /// </summary>
    public sealed class DefalteCompressor : ICompressor
    {
        public CompressorType CompressorType => CompressorType.Deflate;

        public CompressionLevel Level { get; set; } = CompressionLevel.Optimal;

        public Stream CreateCompressionStream(Stream output, bool leaveOpen)
            => new DeflateStream(output, Level, leaveOpen);

        public Stream CreateDecompressionStream(Stream input, bool leaveOpen)
            => new DeflateStream(input, CompressionMode.Decompress, leaveOpen);
    }
}
