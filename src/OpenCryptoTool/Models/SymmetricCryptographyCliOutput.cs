using OpenCryptoTool;
using OpenCryptoTool.Helpers;
using System;

namespace OpenCryptoTool.Models
{
    /// <summary>
    ///     Symmetric encryption printing model.
    /// </summary>
    public class SymmetricCryptographyCliOutput
    {
        /// <summary>
        ///     Create symmetric encryption printing model.
        /// </summary>
        /// <param name="phrase">The phrase to encrypt/decrypt.</param>
        public SymmetricCryptographyCliOutput(string phrase)
        {
            Phrase = phrase;
        }

        /// <summary>
        ///     Create printing model for symmetric encryption.
        /// </summary>
        /// <param name="initializationVector">Initialization vector.</param>
        /// <param name="key">Encryption key.</param>
        /// <param name="phrase">The phrase to encrypt/decrypt.</param>
        /// <param name="returnedDataFormat">Data format of returned values.</param>
        public SymmetricCryptographyCliOutput(
            string initializationVector, 
            string key, 
            string phrase,
            SymmetricCipherType cipherType,
            EncryptedTextReturnOptions returnedDataFormat)
        {
            IV = initializationVector;
            Key = key;
            Phrase = phrase;
            Encoded = returnedDataFormat.ToString();
            Method = cipherType.ToString();
        }

        /// <summary>
        ///     Create printing model for symmetric encryption.
        /// </summary>
        /// <param name="initializationVector">Initialization vector.</param>
        /// <param name="key">Encryption key.</param>
        /// <param name="phrase">The phrase to encrypt/decrypt.</param>
        public SymmetricCryptographyCliOutput(
            byte[] initializationVector, 
            byte[] key, 
            byte[] encryptedPhrase,
            SymmetricCipherType cipherType) : this(
                Convert.ToBase64String(initializationVector),
                Convert.ToBase64String(key),
                Convert.ToBase64String(encryptedPhrase),
                cipherType,
                EncryptedTextReturnOptions.Base64String)
        {
        }

        /// <summary>
        ///     Encryption, decryption method.
        /// </summary>
        public string Method { get; set; }
        /// <summary>
        ///     Initialization vector.
        /// </summary>
        public string IV { get; private set; }
        /// <summary>
        ///     Encryption key.
        /// </summary>
        public string Key { get; private set; }
        /// <summary>
        ///     Encrypted phrase.
        /// </summary>
        public string Phrase { get; private set; }
        /// <summary>
        ///     Format on which are all values returned.
        /// </summary>
        public string Encoded { get; private set; }

        /// <summary>
        ///     Convert method properties to a human readable string.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return CLIHelpers.PropertiesToString(this);
        }
    }
}