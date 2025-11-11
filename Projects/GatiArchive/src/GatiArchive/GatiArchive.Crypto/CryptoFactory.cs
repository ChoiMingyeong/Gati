namespace GatiArchive.Crypto
{
    public static class CryptoFactory
    {
        private static readonly Lazy<ICryptoCipher> Default = new(() => new AesGcmCipher());

        public static ICryptoCipher Create(CryptoCipherType type) => type switch
        {
            CryptoCipherType.None => new NullCipher(),
            CryptoCipherType.AesGcm => new AesGcmCipher(),
            CryptoCipherType.ChaCha20Poly1305 => new ChaCha20Poly1305Cipher(),
            CryptoCipherType.AesCbcHmac => new AesCbcHmacCipher(),
            _ => Default,
        };
    }
}
