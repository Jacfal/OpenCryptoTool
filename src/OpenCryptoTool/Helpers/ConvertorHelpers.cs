using OpenCryptoTool.Models;
using System;

namespace OpenCryptoTool.Helpers
{
    /// <summary>
    ///     Data convertor helpers.
    /// </summary>
    public static class ConvertorHelpers
    {
        /// <summary>
        ///     Converts byte array to base64 format.
        /// </summary>
        /// <param name="initializationVector">Initialization vector.</param>
        /// <param name="key">Encryption key.</param>
        /// <param name="encryptedPhrase">Encrypted phrase.</param>
        /// <returns>Symmetric encryption printing class where all values are encoded to base64.</returns>
        public static SymmetricEncryption ConvertToBase64(byte[] initializationVector, byte[] key, byte[] encryptedPhrase)
        {
            var IVBase64 = Convert.ToBase64String(initializationVector);
            var keyBase64 = Convert.ToBase64String(key);
            var encryptedBase64 = Convert.ToBase64String(encryptedPhrase);

            return new SymmetricEncryption(
                IVBase64,
                keyBase64,
                encryptedBase64,
                EncryptedTextReturnOptions.Base64String
            );
        }
    }
}