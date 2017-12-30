namespace OpenCryptoTool
{
    /// <summary>
    ///     Encryption options.
    /// </summary>
    public interface IEncryptionObject
    {
        CipherType CipherType { get; set; }
        bool Encryption { get; set; }
        bool Decryption { get; set; }
        string Phrase { get; set; }
        int KeySize { get; set; }
    }
}