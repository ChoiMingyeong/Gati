namespace GatiArchive.Crypto
{
    public sealed class NoneCipher : ICryptoCipher
    {
        public CryptoCipherType CryptoCipherType => CryptoCipherType.None;
        public int KeySize => 0;
        public int NonceSize => 0;
        public int TagSize => 0;
        public void Encrypt(ReadOnlySpan<byte> plaintext, ReadOnlySpan<byte> key, ReadOnlySpan<byte> nonce, ReadOnlySpan<byte> aad, Span<byte> ciphertext, Span<byte> tag)
        {
            plaintext.CopyTo(ciphertext);
        }
        public bool Decrypt(ReadOnlySpan<byte> ciphertext, ReadOnlySpan<byte> key, ReadOnlySpan<byte> nonce, ReadOnlySpan<byte> aad, ReadOnlySpan<byte> tag, Span<byte> plaintext)
        {
            ciphertext.CopyTo(plaintext);
            return true;
        }
    }
}
