namespace GatiArchive.Crypto
{
    public enum CryptoCipherType
    {
        None = 0,
        AesGcm,
        ChaCha20Poly1305,
        AesCbcHmac,
        Auto
    }
}
