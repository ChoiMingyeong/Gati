namespace GatiArchive.Crypto
{
    public interface ICryptoCipher
    {
        CryptoCipherType CryptoCipherType { get; }

        int KeySize { get; }    // 16 / 32 등

        int NonceSize { get; }  // AES-GCM 12, ChaCha20 12

        int TagSize { get; }    // GCM 16, Poly1305 16

        void Encrypt(
            ReadOnlySpan<byte> plaintext,
            ReadOnlySpan<byte> key,
            ReadOnlySpan<byte> nonce,
            ReadOnlySpan<byte> aad,
            Span<byte> ciphertext,
            Span<byte> tag);

        bool Decrypt(
            ReadOnlySpan<byte> ciphertext,
            ReadOnlySpan<byte> key,
            ReadOnlySpan<byte> nonce,
            ReadOnlySpan<byte> aad,
            ReadOnlySpan<byte> tag,
            Span<byte> plaintext);
    }
}
