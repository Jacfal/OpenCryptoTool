using CommandLine;
using System;
using System.Text;
using OpenCryptoTool.Models;
using System.Security.Cryptography;
using System.IO;
using Serilog;

namespace OpenCryptoTool.Models
{
    /// <summary>
    ///     Base cryptography CLI input options.
    /// </summary>
    public abstract class BaseCryptographyCliInput
    {
        /// <summary>
        ///     Path to input file.
        /// </summary>
        [Option('i', "input")]
        public string InputFilePath
        {
            get
            {
                return _inputFilePath;
            }

            set
            {
                var exist = File.Exists(value);

                if (!exist) throw new FileNotFoundException("Invalid path to input file.");

                _inputFilePath = value;
                LoadInputFileContent();
            }
        }
        private string _inputFilePath;

        /// <summary>
        ///     Path to output file.
        /// </summary>
        [Option('o', "output")]
        public string OutputFilePath
        {
            get
            {
                return _outputFilePath;
            }
            set
            {
                // TODO output kontrola
                //if (!Uri.IsWellFormedUriString(value, UriKind.RelativeOrAbsolute)) throw new UriFormatException("Invalid output path.");

                _outputFilePath = value;
            }
        }
        private string _outputFilePath;

        /// <summary>
        ///     Output format.
        /// </summary>
        [Option('f', "format")]
        public OutputFormat OutputFormat { get; set; }

        /// <summary>
        ///     Content which should be en/decrypted.
        /// </summary>
        [Option('c', "content")]
        public string Content { get; set; }

        private void LoadInputFileContent()
        {
            if (!string.IsNullOrEmpty(Content))
            {
                Log.Information("Content (-c, --content) is not empty and path to the valid input file was entered. Content field will be replaced by input file content.");
            }

            Content = string.Join("\n", File.ReadAllLines(InputFilePath));

            // output format should be json by default
            if (OutputFormat == OutputFormat.unknown)
            {
                OutputFormat = OutputFormat.json;
            }
        }
    }

    /// <summary>
    ///     CLI options for symmetric encryption.
    /// </summary>
    public abstract class SymmetricCryptographyCliInput : BaseCryptographyCliInput, ISymmetricCryptographyCliInput
    {
        [Option('e', "encrypt")]
        public bool Encryption { get; set; }
        [Option('d', "decrypt")]
        public bool Decryption { get; set; }
        public SymmetricCipherType CipherType { get; set; }
        [Option('k', "key")]
        public string Key { get; set; }
        [Option('v', "iv")]
        public string InitializationVector { get; set; }
    }
    
    // TODO rest of AES methods

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
    ///     Aes 128 CBC CLI command.
    /// </summary>
    [Verb("aes-128-cbc", HelpText = "N/A")]
    public class Aes128CBC : SymmetricCryptographyCliInput
    {
        public Aes128CBC()
        {
            CipherType = new SymmetricCipherType(128, CryptographyStandard.Aes, CipherMode.CBC);
        }
    }

    /// <summary>
    ///     Aes 256 ECB CLI command.
    /// </summary>
    [Verb("aes-256-ecb", HelpText = "N/A")]
    public class Aes256ECB : SymmetricCryptographyCliInput
    {
        public Aes256ECB()
        {
            CipherType = new SymmetricCipherType(256, CryptographyStandard.Aes, CipherMode.ECB);
        }
    }

    /// <summary>
    ///     Aes 192 ECB CLI command.
    /// </summary>
    [Verb("aes-192-ecb", HelpText = "N/A")]
    public class Aes192ECB : SymmetricCryptographyCliInput
    {
        public Aes192ECB()
        {
            CipherType = new SymmetricCipherType(192, CryptographyStandard.Aes, CipherMode.ECB);
        }
    }

    /// <summary>
    ///     Aes 128 ECB CLI command.
    /// </summary>
    [Verb("aes-128-ecb", HelpText = "N/A")]
    public class Aes128ECB : SymmetricCryptographyCliInput
    {
        public Aes128ECB()
        {
            CipherType = new SymmetricCipherType(128, CryptographyStandard.Aes, CipherMode.ECB);
        }
    }

    /// <summary>
    ///     TDes 192 CBC CLI command.
    /// </summary>
    [Verb("tdes-192-cbc", HelpText = "N/A")]
    public class TDes192CBC : SymmetricCryptographyCliInput
    {
        public TDes192CBC()
        {
            CipherType = new SymmetricCipherType(192, CryptographyStandard.TripleDes, CipherMode.CBC);
        }
    }

    /// <summary>
    ///     TDes 128 CBC CLI command.
    /// </summary>
    [Verb("tdes-128-cbc", HelpText = "N/A")]
    public class TDes128CBC : SymmetricCryptographyCliInput
    {
        public TDes128CBC()
        {
            CipherType = new SymmetricCipherType(128, CryptographyStandard.TripleDes, CipherMode.CBC);
        }
    }

    /// <summary>
    ///     TDes 192 CBC CLI command.
    /// </summary>
    [Verb("tdes-192-ecb", HelpText = "N/A")]
    public class TDes192ECB : SymmetricCryptographyCliInput
    {
        public TDes192ECB()
        {
            CipherType = new SymmetricCipherType(192, CryptographyStandard.TripleDes, CipherMode.ECB);
        }
    }

    /// <summary>
    ///     TDes 128 ECB CLI command.
    /// </summary>
    [Verb("tdes-128-ecb", HelpText = "N/A")]
    public class TDes128ECB : SymmetricCryptographyCliInput
    {
        public TDes128ECB()
        {
            CipherType = new SymmetricCipherType(128, CryptographyStandard.TripleDes, CipherMode.ECB);
        }
    }

    /// <summary>
    ///     Cipher type definition.
    /// </summary>
    public enum CryptographyStandard
    {
        Aes,
        TripleDes
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