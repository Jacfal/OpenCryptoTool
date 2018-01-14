namespace OpenCryptoTool.Models
{
    /// <summary>
    ///     Encryption options.
    /// </summary>
    public interface ISymmetricCryptographyCliInput
    {
        SymmetricCipherType CipherType { get; set; }
        bool Encryption { get; set; }
        bool Decryption { get; set; }
        string Phrase { get; set; }
        string Key { get; set; }
        string InitializationVector { get; set; }
    }
}