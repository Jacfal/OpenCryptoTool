using System;
using System.IO;
using System.Security.Cryptography;
using OpenCryptoTool.Models;
using Serilog;

namespace OpenCryptoTool.Providers
{
    /// <summary>
    ///     Cryptographic provider.
    /// </summary>
    public abstract class CryptoProvider : IDisposable
    {
        /// <summary>
        ///     Symmetric cryptography properties.
        /// </summary>
        public ISymmetricCryptographyProperties Properties { get; set; }

        protected SymmetricAlgorithm _symmetricAlgorithm;

        /// <summary>
        ///     Create a new crypto provider.
        /// </summary>
        /// <param name="symmetricAlgorithm">Symmetric algorithm implementation.</param>
        public CryptoProvider(SymmetricAlgorithm symmetricAlgorithm)
        {
            _symmetricAlgorithm = symmetricAlgorithm;
        }

        /// <summary>
        ///     Create a new crypto provider.
        /// </summary>
        /// <param name="key">Symmetric key.</param>
        /// <param name="IV">Initialization vector.</param>
        /// <param name="cypherMode">Cipher mode.</param>
        /// <param name="symmetricAlgorithm">Symmetric algorithm implementation.</param>
        public CryptoProvider(
            string key, 
            string IV, 
            CipherMode cipherMode, 
            SymmetricAlgorithm symmetricAlgorithm)
            : this (symmetricAlgorithm)
        {
            Properties = new SymmetricCryptographyProperties(key, IV, cipherMode);
        }

        /// <summary>
        /// <summary>
        ///     General encryption method.
        /// </summary>
        /// <param name="toEncrypt">String which should be encrypted.</param>
        /// <param name="key">Encryption key.</param>
        /// <param name="initializationVector">Initialization vector.</param>
        /// <param name="cipherMode">Block cipher mode of operation.</param>
        /// <returns>Encrypted bytes array.</returns>
        public abstract byte[] Encrypt(string toEncrypt, byte[] key,byte[] initializationVector, CipherMode cipherMode);

        protected byte[] Encrypt(string toEncrypt, byte[] key,byte[] initializationVector, CipherMode cipherMode, ICryptoTransform encryptor)
        {
            if(string.IsNullOrEmpty(toEncrypt)) throw new ArgumentNullException("Encrypted text cann't be null or empty.");
            
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

        /// <summary>
        ///     General decryption method.
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
        ///     General decryption method.
        /// </summary>
        /// <param name="toDecrypt">Byte array which should be decrypted.</param>
        /// <param name="key">Decryption key.</param>
        /// <param name="initializationVector">Initialization vector.</param>
        /// <param name="cipherMode">Block cipher mode of operation.</param>
        /// <returns>Decrypted string.</returns>
        public abstract string Decrypt(byte[] toDecrypt, byte[] key, byte[] initializationVector, CipherMode cipherMode);

        protected string Decrypt(byte[] toDecrypt, byte[] key, byte[] initializationVector, CipherMode cipherMode, ICryptoTransform decryptor)
        {
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

        /// <summary>
        ///     Generate symmetric key for encryption and decryption.
        /// </summary>
        /// <param name="keySize">Key size.</param>
        /// <returns>Key byte array.</returns>
        public virtual byte[] GenerateKey(int keySize)
        {
            _symmetricAlgorithm.Clear();

            _symmetricAlgorithm.KeySize = keySize;
            _symmetricAlgorithm.GenerateKey();

            return _symmetricAlgorithm.Key;
        }

        /// <summary>
        ///     Generate initialization vector.
        /// </summary>
        /// <returns>New initialization vector.</returns>
        public byte[] GenerateInitializationVector()
        {
            _symmetricAlgorithm.Clear();

            Log.Information("Generating new IV.");
            _symmetricAlgorithm.GenerateIV();
            return _symmetricAlgorithm.IV;
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    _symmetricAlgorithm.Dispose();
                }
                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
        }
        #endregion
    }
}