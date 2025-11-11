using System.Security.Cryptography;

namespace GatiArchive.Crypto
{
    /// <summary>
    /// AES-GCM
    /// <br/> 표준, 하드웨어 가속 지원
    /// </summary>
    public sealed class AesGcmCipher : ICryptoCipher
    {
        public CryptoCipherType CryptoCipherType => CryptoCipherType.AesGcm;

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
            using var aesGcm = new AesGcm(key, TagSize);
            aesGcm.Encrypt(nonce, plaintext, ciphertext, tag, aad);
        }

        public bool Decrypt(
            ReadOnlySpan<byte> ciphertext,
            ReadOnlySpan<byte> key,
            ReadOnlySpan<byte> nonce,
            ReadOnlySpan<byte> aad,
            ReadOnlySpan<byte> tag,
            Span<byte> plaintext)
        {
            try
            {
                using var aesGcm = new AesGcm(key, TagSize);
                aesGcm.Decrypt(nonce, ciphertext, tag, plaintext, aad);
                return true;
            }
            catch (CryptographicException)
            {
                return false;
            }
        }
    }
}
