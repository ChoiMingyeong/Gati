namespace GatiArchive.Crypto
{
    public sealed class AesCbcHmacCipher : ICryptoCipher
    {
        public CryptoCipherType CryptoCipherType => CryptoCipherType.AesCbcHmac;

        public int KeySize => throw new NotImplementedException();

        public int NonceSize => throw new NotImplementedException();

        public int TagSize => throw new NotImplementedException();

        public bool Decrypt(ReadOnlySpan<byte> ciphertext, ReadOnlySpan<byte> key, ReadOnlySpan<byte> nonce, ReadOnlySpan<byte> aad, ReadOnlySpan<byte> tag, Span<byte> plaintext)
        {
            throw new NotImplementedException();
        }

        public void Encrypt(ReadOnlySpan<byte> plaintext, ReadOnlySpan<byte> key, ReadOnlySpan<byte> nonce, ReadOnlySpan<byte> aad, Span<byte> ciphertext, Span<byte> tag)
        {
            throw new NotImplementedException();
        }
    }
}
