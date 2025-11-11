using System.IO.Compression;

namespace GatiArchive.Compression
{
    public static class CompressorFactory
    {
        // 기본값 압축기
        private static readonly Lazy<ICompressor> Default = new(() => new DefalteCompressor());

        public static ICompressor Create(CompressorType type) => type switch
        {
            CompressorType.None => new NoCompression(),
            CompressorType.Deflate => new DefalteCompressor(),
            CompressorType.Brotli => new BrotliCompressor(),
            CompressorType.LZ4 => new LZ4Compressor(),
            CompressorType.Auto => AutoSelect(),
            _ => Default.Value,
        };

        public static ICompressor AutoSelect(
            int? fileSize = null,
            bool preferCompressionRatio = false,
            bool preferSpeed = false)
        {
            if (preferSpeed)
            {
                return new LZ4Compressor();
            }

            if (preferCompressionRatio)
            {
                return new BrotliCompressor();
            }

            if (fileSize.HasValue)
            {
                if (fileSize.Value < 1_000_000)   // 1MB 미만
                {
                    return new DefalteCompressor()
                    {
                        Level = CompressionLevel.Fastest,
                    };
                }

                if (fileSize.Value < 50_000_000)  // 50MB 미만
                {
                    return new BrotliCompressor();
                }
            }

            // 기본
            return Default.Value;
        }
    }
}
