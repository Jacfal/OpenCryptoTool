using CommandLine;
using System;
using System.Text;
using OpenCryptoTool.Models;
using System.Security.Cryptography;

namespace OpenCryptoTool.Models
{
    /// <summary>
    ///     Base cryptography CLI input options.
    /// </summary>
    public abstract class BaseCryptographyCliInput
    {
        /// <summary>
        ///     Output file.
        /// </summary>
        [Option('o', "output")]
        public string OutputFile { get; set; }

        /// <summary>
        ///     Output format.
        /// </summary>
        [Option('f', "format")]
        public OutputFormat OutputFormat { get; set; }
    }

    /// <summary>
    ///     CLI options for symmetric encryption.
    /// </summary>
    public abstract class SymmetricCryptographyCliInput : BaseCryptographyCliInput, ISymmetricCryptographyCliInput
    {
        [Option('p', "phrase")]
        public string Phrase { get; set; }
        [Option('e', "encrypt")]
        public bool Encryption { get; set; }
        [Option('d', "decrypt")]
        public bool Decryption { get; set; }
        public SymmetricCipherType CipherType { get; set; }
        [Option('k', "key")]
        public string Key { get; set; }
        [Option('i', "iv")]
        public string InitializationVector { get; set; }
    }

    /// <summary>
    ///     Aes 256 CBC CLI command.
    /// </summary>
    [Verb("aes-256-cbc", HelpText = "N/A")]
    public class Aes256CBC : SymmetricCryptographyCliInput
    {
        public Aes256CBC()
        {
            CipherType = new SymmetricCipherType(256, CryptographyStandard.Aes, CipherMode.CBC);
        }
    }

    /// <summary>
    ///     Aes 192 CBC CLI command.
    /// </summary>
    [Verb("aes-192-cbc", HelpText = "N/A")]
    public class Aes192CBC : SymmetricCryptographyCliInput
    {
        public Aes192CBC()
        {
            CipherType = new SymmetricCipherType(192, CryptographyStandard.Aes, CipherMode.CBC);
        }
    }

    /// <summary>
    ///     Cipher type definition.
    /// </summary>
    public enum CryptographyStandard
    {
        Aes
    }

    public abstract class BaseCipherType
    {
        public BaseCipherType(int keySize, CryptographyStandard cryptographyStandard, CipherMode cipherMode)
        {
            KeySize = keySize;
            CryptographyStandard = cryptographyStandard;
            CipherMode = cipherMode;
        }

        public int KeySize { get; set; }
        public CryptographyStandard CryptographyStandard { get; set; }
        public CipherMode CipherMode { get; set; }
    }

    public class SymmetricCipherType : BaseCipherType
    {
        public SymmetricCipherType(int keySize, CryptographyStandard cryptographyStandard, CipherMode cipherMode)
            : base (keySize, cryptographyStandard, cipherMode)
        {

        }

        public override string ToString()
        {
            return $"{CryptographyStandard.ToString()}-{KeySize}-{CipherMode.ToString()}".ToLower();
        }
    }
}