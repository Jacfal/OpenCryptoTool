using OpenCryptoTool.Models;
using Serilog;
using System;
using System.IO;
using System.Security.Cryptography;

namespace OpenCryptoTool
{
    /// <summary>
    ///     AES cryptographic class.
    /// </summary>
    public class AesProvider
    {
        /// <summary>
        ///     Symmetric cryptography properties.
        /// </summary>
        public ISymmetricCryptographyProperties Properties { get; set; }

        /// <summary>
        ///     Creates new instance of AES crypto provider.
        /// </summary>
        public AesProvider()
        {
        }

        /// <summary>
        ///     Creates new instance of AES crypto provider.
        /// </summary>
        /// <param name="key">Symmetric key.</param>
        /// <param name="IV">Initialization vector.</param>
        /// <param name="cypherMode">Cipher mode.</param>
        public AesProvider(string key, string IV, CipherMode cipherMode)
        {
            Properties = new SymmetricCryptographyProperties(key, IV, cipherMode);
        }

        /// <summary>
        /// <summary>
        ///     AES Encryption method.
        /// </summary>
        /// <param name="toEncrypt">String which should be encrypted.</param>
        /// <param name="key">Encryption key.</param>
        /// <param name="initializationVector">Initialization vector.</param>
        /// <param name="cipherMode">Block cipher mode of operation.</param>
        /// <returns>Encrypted bytes array.</returns>
        public byte[] Encrypt(string toEncrypt, byte[] key,byte[] initializationVector, CipherMode cipherMode)
        {
            if(string.IsNullOrEmpty(toEncrypt)) throw new ArgumentNullException("Encrypted text cann't be null or empty.");
            if(key == null || key.Length < 0) throw new ArgumentNullException("Invalid key.");

            using(var aes = Aes.Create("AesCryptoServiceProvider"))  
            {
                aes.Mode = cipherMode;
                aes.IV = initializationVector;
                var encryptor = aes.CreateEncryptor(key, aes.IV);

                Log.Information("AesCryptoServiceProvider for encryption created.");

                using (var memoryStream = new MemoryStream())
                {
                    Log.Information("Memory stream - ready.");

                    using (var cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
                    {
                        Log.Information("Crypto stream - ready.");

                        using (var streamWritter = new StreamWriter(cryptoStream))
                        {
                            Log.Information("Stream writter - ready.");

                            streamWritter.Write(toEncrypt);
                        }
                    }
                    return memoryStream.ToArray();
                }
            }   
        }

        /// <summary>
        ///     AES decryption method.
        /// </summary>
        /// <param name="toDecrypt">Phrase to decrypt.</param>
        /// <returns>Decrypted phrase.</returns>
        public string Decrypt(string toDecrypt)
        {
            if (Properties == null) throw new NullReferenceException("Aes provider properties mus be set.");
            if (Properties.Key == null) throw new NullReferenceException("The decryption key must be set.");
            if (Properties.InitializationVector == null) throw new NullReferenceException("The initialization vector must be set.");

            var toDecryptBytes = Convert.FromBase64String(toDecrypt);

            return Decrypt(toDecryptBytes, Properties.Key, Properties.InitializationVector, Properties.CipherMode);
        }

        /// <summary>
        ///     AES decryption method.
        /// </summary>
        /// <param name="toDecrypt">Byte array which should be decrypted.</param>
        /// <param name="key">Decryption key.</param>
        /// <param name="initializationVector">Initialization vector.</param>
        /// <param name="cipherMode">Block cipher mode of operation.</param>
        /// <returns>Decrypted string.</returns>
        public string Decrypt(byte[] toDecrypt, byte[] key, byte[] initializationVector, CipherMode cipherMode)
        {
            if(toDecrypt == null || toDecrypt.Length < 0) throw new ArgumentNullException("Encrypted text cann't be null or empty.");
            if(key == null || key.Length < 0) throw new ArgumentNullException("Invalid key.");

            using(var aes = Aes.Create("AesCryptoServiceProvider"))
            {
                aes.Mode = cipherMode;
                aes.IV = initializationVector;
                aes.Key = key;

                var decryptor = aes.CreateDecryptor();

                Log.Information("AesCryptoServiceProvider for decryption created.");

                using (var memoryStream = new MemoryStream(toDecrypt))
                {
                    Log.Information("Memory stream - ready.");

                    using (var cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read))
                    {
                        Log.Information("Crypto stream - ready.");

                        using (var streamReader = new StreamReader(cryptoStream))
                        {
                            Log.Information("Stream reader - ready.");

                            var decrytped = streamReader.ReadToEnd();
                            return decrytped;
                        }
                    }
                }
            }
        }

        /// <summary>
        ///     Generate AES key for encryption and decryption.
        /// </summary>
        /// <param name="keySize">Key size. In default set to 256.</param>
        /// <returns>AES key byte array.</returns>
        public byte[] GenerateKey(int keySize = 256)
        {
            using (var aes = Aes.Create("AesCryptoServiceProvider"))
            {
                if (!aes.ValidKeySize(keySize)) throw new CryptographicException("Invalid AES keysize. Valid AES keysize value is 128, 192 or 256 bytes.");

                aes.KeySize = keySize;
                aes.GenerateKey();

                return aes.Key;
            }
        }

        /// <summary>
        ///     Generate initialization vector.
        /// </summary>
        /// <returns>New initialization vector.</returns>
        public byte[] GenerateInitializationVector()
        {
            using (var aes = Aes.Create("AesCryptoServiceProvider"))
            {
                aes.GenerateIV();
                return aes.IV;
            }
        }
    }
}