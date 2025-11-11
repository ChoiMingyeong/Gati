namespace GatiArchive.Compression
{
    /// <summary>
    /// 무압축
    /// <br/>압축률     : -
    /// <br/>압축 속도  : -
    /// <br/>해제 속도  : -
    /// </summary>
    public sealed class NoCompression : ICompressor
    {
        public CompressorType CompressorType => CompressorType.None;

        public Stream CreateCompressionStream(Stream output, bool leaveOpen) => output;

        public Stream CreateDecompressionStream(Stream input, bool leaveOpen) => input;
    }
}
