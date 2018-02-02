using OpenCryptoTool.Models;
using Serilog;
using System;
using System.IO;
using System.Security.Cryptography;

namespace OpenCryptoTool.Providers
{
    /// <summary>
    ///     AES cryptographic class.
    /// </summary>
    public class AesProvider : CryptoProvider
    {
        /// <summary>
        ///     Creates new instance of AES crypto provider.
        /// </summary>
        public AesProvider()
            : base (Aes.Create("AesCryptoServiceProvider"))
        {
        }
        
        /// <summary>
        ///     Creates new instance of AES crypto provider.
        /// </summary>
        /// <param name="key">Symmetric key.</param>
        /// <param name="IV">Initialization vector.</param>
        /// <param name="cypherMode">Cipher mode.</param>
        public AesProvider(string key, string IV, CipherMode cipherMode)
            : base(key, IV, cipherMode, Aes.Create("AesCryptoServiceProvider"))
        {
        }

        /// <summary>
        ///     Creates new instance of AES crypto provider.
        /// </summary>
        /// <param name="key">Symmetric key.</param>
        /// <param name="cypherMode">Cipher mode.</param>
        public AesProvider(string key, CipherMode cipherMode)
            : base(key, cipherMode, Aes.Create("AesCryptoServiceProvider"))
        {
        }

        /// <summary>
        ///     AES Encryption method.
        /// </summary>
        /// <param name="toEncrypt">String which should be encrypted.</param>
        /// <param name="key">Encryption key.</param>
        /// <param name="initializationVector">Initialization vector.</param>
        /// <param name="cipherMode">Block cipher mode of operation.</param>
        /// <returns>Encrypted bytes array.</returns>
        public override byte[] Encrypt(string toEncrypt, byte[] key,byte[] initializationVector, CipherMode cipherMode)
        {
            using(var aes = Aes.Create("AesCryptoServiceProvider"))
            {
                aes.Mode = cipherMode;
                aes.IV = initializationVector;
                var encryptor = aes.CreateEncryptor(key, aes.IV);

                Log.Information("AES encryptor created.");
                return base.Encrypt(toEncrypt, encryptor);
            }
        }

        /// <summary>
        ///     AES decryption method.
        /// </summary>
        /// <param name="toDecrypt">Byte array which should be decrypted.</param>
        /// <param name="key">Decryption key.</param>
        /// <param name="initializationVector">Initialization vector.</param>
        /// <param name="cipherMode">Block cipher mode of operation.</param>
        /// <returns>Decrypted string.</returns>
        public override string Decrypt(byte[] toDecrypt, byte[] key, byte[] initializationVector, CipherMode cipherMode)
        {
            if(toDecrypt == null || toDecrypt.Length < 0) throw new ArgumentNullException("Encrypted text cann't be null or empty.");

            using(var aes = Aes.Create("AesCryptoServiceProvider"))
            {
                aes.Mode = cipherMode;
                aes.IV = initializationVector;
                aes.Key = key;

                var decryptor = aes.CreateDecryptor();

                Log.Information("AES decryptor created.");
                return base.Decrypt(toDecrypt, decryptor);
            }
        }

        /// <summary>
        ///     Generate AES key for encryption and decryption.
        /// </summary>
        /// <param name="keySize">Key size. In default set to 256.</param>
        /// <returns>AES key byte array.</returns>
        public override byte[] GenerateKey(int keySize = 256)
        {
            if (!_symmetricAlgorithm.ValidKeySize(keySize)) throw new CryptographicException("Invalid AES keysize. Valid AES keysize value is 128, 192 or 256 bytes.");
            return base.GenerateKey(keySize);
        }
    }
}