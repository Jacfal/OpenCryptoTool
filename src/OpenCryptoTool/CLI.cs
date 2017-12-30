using System;
using System.Text;
using CommandLine;

namespace OpenCryptoTool
{
    /// <summary>
    ///     CLI options for symmetric encryption.
    /// </summary>
    public abstract class SymmetricEncryptionOptions : IEncryptionObject
    {
        [Option('p', "phrase")]
        public string Phrase { get; set; }
        [Option('e')]
        public bool Encryption { get; set; }
        [Option('d')]
        public bool Decryption { get; set; }
        public CipherType CipherType { get; set; }
        public int KeySize { get; set; }
    }

    /// <summary>
    ///     Aes 256 CBC CLI command.
    /// </summary>
    [Verb("aes-256-cbc", HelpText = "N/A")]
    public class Aes256CBC : SymmetricEncryptionOptions
    {
        public Aes256CBC()
        {
            CipherType = CipherType.Aes256CBC;
            KeySize = 256;
        }
    }

    /// <summary>
    ///     Aes 192 CBC CLI command.
    /// </summary>
    [Verb("aes-192-cbc", HelpText = "N/A")]
    public class Aes192CBC : SymmetricEncryptionOptions
    {
        public Aes192CBC()
        {
            CipherType = CipherType.Aes192CBC;
            KeySize = 192;
        }
    }

    /// <summary>
    ///     Cipher type definition.
    /// </summary>
    public enum CipherType
    {
        Aes256CBC,
        Aes192CBC
    }
}