using System.Security.Cryptography;

namespace GatiArchive.Crypto
{
    /// <summary>
    /// ChaCha20-Poly1305
    /// <br/> 모바일/브라우저용
    /// </summary>
    public sealed class ChaCha20Poly1305Cipher : ICryptoCipher
    {
        public CryptoCipherType CryptoCipherType => CryptoCipherType.ChaCha20Poly1305;

        public int KeySize => 32;
        
        public int NonceSize => 12;
        
        public int TagSize => 16;
        
        public void Encrypt(
            ReadOnlySpan<byte> plaintext,
            ReadOnlySpan<byte> key,
            ReadOnlySpan<byte> nonce,
            ReadOnlySpan<byte> aad,
            Span<byte> ciphertext,
            Span<byte> tag)
        {
            ChaCha20Poly1305.Encrypt(key, nonce, plaintext, aad, ciphertext, tag);
        }
        public bool Decrypt(
            ReadOnlySpan<byte> ciphertext,
            ReadOnlySpan<byte> key,
            ReadOnlySpan<byte> nonce,
            ReadOnlySpan<byte> aad,
            ReadOnlySpan<byte> tag,
            Span<byte> plaintext)
        {
            return ChaCha20Poly1305.Decrypt(key, nonce, ciphertext, aad, tag, plaintext);
        }
    }
}
