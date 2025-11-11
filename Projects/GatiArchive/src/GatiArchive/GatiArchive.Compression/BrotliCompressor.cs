using System.IO.Compression;

namespace GatiArchive.Compression
{
    /// <summary>
    /// 최대 압축률 필요시 사용(웹 리소스/게임 패키지용)
    /// <br/>압축률     : ⭐⭐⭐⭐⭐
    /// <br/>압축 속도  : ⭐⭐
    /// <br/>해제 속도  : ⭐⭐⭐⭐
    /// </summary>
    public sealed class BrotliCompressor : ICompressor
    {
        public CompressorType CompressorType => CompressorType.Brotli;

        public Stream CreateCompressionStream(Stream output, bool leaveOpen)
            => new BrotliStream(output, CompressionMode.Compress, leaveOpen);

        public Stream CreateDecompressionStream(Stream input, bool leaveOpen)
            => new BrotliStream(input, CompressionMode.Decompress, leaveOpen);
    }
}
