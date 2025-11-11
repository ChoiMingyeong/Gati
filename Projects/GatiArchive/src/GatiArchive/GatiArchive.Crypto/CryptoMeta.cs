namespace GatiArchive.Crypto
{
    public sealed class CryptoMeta
    {
        public CryptoCipherType Algorithm { get; set; }

        public string Kdf { get; set; } = "";
        
        public string Salt { get; set; } = "";
        
        public int Iterations { get; set; }
    }
}
