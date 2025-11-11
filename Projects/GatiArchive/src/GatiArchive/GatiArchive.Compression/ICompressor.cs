namespace GatiArchive.Compression
{
    public interface ICompressor
    {
        CompressorType CompressorType { get; }

        Stream CreateCompressionStream(Stream output, bool leaveOpen);

        Stream CreateDecompressionStream(Stream input, bool leaveOpen);
    }
}
