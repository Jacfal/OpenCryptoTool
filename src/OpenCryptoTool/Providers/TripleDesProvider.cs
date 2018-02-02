using System;
using System.IO;
using System.Security.Cryptography;
using Serilog;

namespace OpenCryptoTool.Providers
{
    /// <summary>
    ///     Triple DES provider.
    /// </summary>
    public class TripleDesProvider : CryptoProvider
    {
        /// <summary>
        ///     Create new instance of a TDes provider.
        /// </summary>
        public TripleDesProvider()
            : base(new TripleDESCryptoServiceProvider())
        {
        }

        /// <summary>
        ///     Create new instance of a TDes provider.
        /// </summary>
        /// <param name="key">Symmetric key.</param>
        /// <param name="IV">Initialization vector.</param>
        /// <param name="cypherMode">Cipher mode.</param>
        public TripleDesProvider(string key, string IV, CipherMode cipherMode)
            : base(key, IV, cipherMode, new TripleDESCryptoServiceProvider())
        {
        }

        /// <summary>
        ///     TDES encryption method.
        /// </summary>
        /// <param name="toEncrypt">String which should be encrypted.</param>
        /// <param name="key">Encryption key.</param>
        /// <param name="initializationVector">Initialization vector.</param>
        /// <param name="cipherMode">Block cipher mode of operation.</param>
        /// <returns>Encrypted bytes array.</returns>
        public override byte[] Encrypt(string toEncrypt, byte[] key, byte[] initializationVector, CipherMode cipherMode)
        {
            if(string.IsNullOrEmpty(toEncrypt)) throw new ArgumentNullException("Encrypted text cann't be null or empty.");

            using(var tdes = new TripleDESCryptoServiceProvider())
            {
                tdes.Mode = cipherMode;
                tdes.IV = initializationVector;
                var encryptor = _symmetricAlgorithm.CreateEncryptor(key, _symmetricAlgorithm.IV);

                Log.Information("TDES encryptor created.");
                return base.Encrypt(toEncrypt, encryptor);
            }
        }

        public override string Decrypt(byte[] toDecrypt, byte[] key, byte[] initializationVector, CipherMode cipherMode)
        {
            if(toDecrypt == null || toDecrypt.Length < 0) throw new ArgumentNullException("Encrypted text cann't be null or empty.");
            
            using(var tdes = new TripleDESCryptoServiceProvider())
            {
                tdes.Mode = cipherMode;
                tdes.IV = initializationVector;
                tdes.Key = key;

                var decryptor = tdes.CreateDecryptor();

                Log.Information("TDES decryptor created.");
                return Decrypt(toDecrypt, decryptor);
            }
        }

        /// <summary>
        ///     Generate TDES key for encryption and decryption.
        /// </summary>
        /// <param name="keySize"></param>
        /// <returns></returns>
        public override byte[] GenerateKey(int keySize = 192)
        {
            if (!_symmetricAlgorithm.ValidKeySize(keySize)) throw new CryptographicException("Invalid TDES keysize. Valid TDES keysize value is 128, 192 bytes.");
            return base.GenerateKey(keySize);
        }
    }
}