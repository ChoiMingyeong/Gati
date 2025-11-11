using System.Security.Cryptography;

namespace GatiArchive.Crypto
{
    public sealed class Pbkdf2Kdf : IKdf
    {
        public string Name => "pbkdf2-sha256";

        public byte[] DeriveKey(
            ReadOnlySpan<byte> secret,
            ReadOnlySpan<byte> salt,
            int keyBytes,
            int iterations)
        {
            return Rfc2898DeriveBytes.Pbkdf2(
                password: secret,
                salt: salt.ToArray(),
                iterations: iterations,
                hashAlgorithm: HashAlgorithmName.SHA256,
                outputLength: keyBytes);
        }
    }
}
