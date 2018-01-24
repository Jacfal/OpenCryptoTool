namespace OpenCryptoTool.Models
{
    /// <summary>
    ///     Symmetric cryptography input CLI options.
    /// </summary>
    public interface ISymmetricCryptographyCliInput
    {
        SymmetricCipherType CipherType { get; set; }
        bool Encryption { get; set; }
        bool Decryption { get; set; }
        string Content { get; set; }
        string Key { get; set; }
        string InitializationVector { get; set; }
    }
}