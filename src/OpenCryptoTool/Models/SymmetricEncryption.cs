using OpenCryptoTool;

namespace OpenCryptoTool.Models
{
    /// <summary>
    ///     Symmetric encryption printing model.
    /// </summary>
    public class SymmetricEncryption
    {
        /// <summary>
        ///     Create symmetric encryption printing model.
        /// </summary>
        /// <param name="initializationVector">Initialization vector.</param>
        /// <param name="key">Encryption key.</param>
        /// <param name="encrypted">Encrypted phrase,</param>
        /// <param name="returnedDataFormat">Format on which all values are returned.</param>
        public SymmetricEncryption(
            string initializationVector, 
            string key, 
            string encrypted,
            EncryptedTextReturnOptions returnedDataFormat)
        {
            InitializationVector = initializationVector;
            Key = key;
            Encrypted = encrypted;
            Encoded = returnedDataFormat.ToString();
        }

        /// <summary>
        ///     Initialization vector.
        /// </summary>
        public string InitializationVector { get; private set; }
        /// <summary>
        ///     Encryption key.
        /// </summary>
        public string Key { get; private set; }
        /// <summary>
        ///     Encrypted phrase.
        /// </summary>
        public string Encrypted { get; private set; }
        /// <summary>
        ///     Format on which are all values returned.
        /// </summary>
        public string Encoded { get; private set; }
    }
}