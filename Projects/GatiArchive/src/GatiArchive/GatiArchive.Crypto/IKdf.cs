namespace GatiArchive.Crypto
{
    public interface IKdf
    {
        /// <summary>
        /// KDF 이름 (예: "pbkdf2-sha256")
        /// </summary>
        string Name { get; }

        /// <summary>
        /// 키 파생 함수
        /// </summary>
        /// <param name="secret">패스워드 또는 마스터 키</param>
        /// <param name="salt">랜덤 솔트</param>
        /// <param name="keyBytes">출력 키 길이 (바이트)</param>
        /// <param name="iterations">반복 횟수</param>
        /// <returns>파생된 키</returns>
        byte[] DeriveKey(
            ReadOnlySpan<byte> secret,
            ReadOnlySpan<byte> salt,
            int keyBytes,
            int iterations);
    }
}
