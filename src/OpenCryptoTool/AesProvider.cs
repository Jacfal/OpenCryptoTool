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
        ///     AES Encryption method.
        /// </summary>
        /// <param name="toEncrypt">String which should be encrypted.</param>
        /// <param name="key">Encryption key.</param>
        /// <param name="initializationVector">Initialization vector.</param>
        /// <param name="cipherMode">Block cipher mode of operation.</param>
        /// <returns></returns>
        public byte[] Encrypt(string toEncrypt, byte[] key,byte[] initializationVector, CipherMode cipherMode)
        {
            if(string.IsNullOrEmpty(toEncrypt)) throw new ArgumentNullException("Encrypted text cann't be null or empty.");
            if(key == null || key.Length < 0) throw new ArgumentNullException("Invalid key.");

            using(var aes = Aes.Create("AesCryptoServiceProvider"))  
            {
                aes.Mode = cipherMode;
                aes.IV = initializationVector;
                var encryptor = aes.CreateEncryptor(key, aes.IV);

                using (var memoryStream = new MemoryStream())
                {
                    using (var cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
                    {
                        using (var streamWritter = new StreamWriter(cryptoStream))
                        {
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
        /// <param name="toDecrypt">Byte array which should be decrypted.</param>
        /// <param name="key">Decryption key.</param>
        /// <param name="initializationVector">Initialization vector.</param>
        /// <param name="cipherMode">Block cipher mode of operation.</param>
        /// <returns></returns>
        public string Decrypt(byte[] toDecrypt, byte[] key, byte[] initializationVector, CipherMode cipherMode)
        {
            if(toDecrypt == null || toDecrypt.Length < 0) throw new ArgumentNullException("Encrypted text cann't be null or empty.");
            if(key == null || key.Length < 0) throw new ArgumentNullException("Invalid key.");

            using(var aes = Aes.Create("AesCryptoServiceProvider"))
            {
                aes.Mode = cipherMode;
                aes.IV = initializationVector;
                var decryptor = aes.CreateDecryptor(key, aes.IV);

                using (var memoryStream = new MemoryStream(toDecrypt))
                {
                    using (var cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read))
                    {
                        using (var streamReader = new StreamReader(cryptoStream))
                        {
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