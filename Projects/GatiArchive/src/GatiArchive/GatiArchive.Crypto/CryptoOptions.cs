namespace GatiArchive.Crypto
{
    public sealed class CryptoOptions
    {
        // 사용할 암호화 알고리즘 (기본값: AES-GCM)
        public ICryptoCipher Cipher { get; init; } = new AesGcmCipher();

        // 키 파생 방식 (기본값: PBKDF2-SHA256)
        public IKdf Kdf { get; init; } = new Pbkdf2Kdf();

        // 키 파생용 솔트 (랜덤 16~32바이트 권장)
        public byte[] Salt { get; init; } = System.Security.Cryptography.RandomNumberGenerator.GetBytes(16);

        // KDF 반복 횟수 (기본: 200,000)
        public int Iterations { get; init; } = 200_000;

        // 사용할 키 길이 (AES-256 → 32바이트)
        public int KeyBytes { get; init; } = 32;

        // 세션 키를 생성 (패스워드 기반)
        public byte[] DeriveKey(ReadOnlySpan<byte> passphrase)
            => Kdf.DeriveKey(passphrase, Salt, KeyBytes, Iterations);

        // 매니페스트에 기록할 메타데이터 생성
        public CryptoMeta ToMeta() => new()
        {
            Algorithm = Cipher.CryptoCipherType,
            Kdf = Kdf.Name,
            Salt = Convert.ToBase64String(Salt),
            Iterations = Iterations
        };

        public override string ToString() =>
            $"{Cipher.CryptoCipherType} / {Kdf.Name} (Iter={Iterations}, Salt={Salt.Length}B)";
    }
}
