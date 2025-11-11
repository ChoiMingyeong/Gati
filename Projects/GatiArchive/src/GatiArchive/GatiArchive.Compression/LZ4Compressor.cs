using K4os.Compression.LZ4;
using K4os.Compression.LZ4.Streams;

namespace GatiArchive.Compression
{
    /// <summary>
    /// 실시간/스트리밍용 (실시간 데이터/게임 네트워크용)
    /// <br/>압축률     : ⭐⭐
    /// <br/>압축 속도  : ⭐⭐⭐⭐⭐
    /// <br/>해제 속도  : ⭐⭐⭐⭐⭐
    /// </summary>
    public sealed class LZ4Compressor : ICompressor
    {
        public CompressorType CompressorType => CompressorType.LZ4;

        public LZ4Level Level { get; set; } = LZ4Level.L03_HC;

        public Stream CreateCompressionStream(Stream output, bool leaveOpen = false)
            => LZ4Stream.Encode(output, new LZ4EncoderSettings { CompressionLevel = Level }, leaveOpen);

        public Stream CreateDecompressionStream(Stream input, bool leaveOpen = false)
            => LZ4Stream.Decode(input, leaveOpen: leaveOpen);
    }
}
