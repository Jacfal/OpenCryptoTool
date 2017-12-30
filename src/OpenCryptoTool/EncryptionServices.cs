using OpenCryptoTool.Models;
using OpenCryptoTool.Helpers;
using System.Security.Cryptography;
using System.Collections.Generic;

namespace OpenCryptoTool
{
    /// <summary>
    ///     Encryption services.
    /// </summary>
    public static class EncryptionServices
    {
        /// <summary>
        ///     Encryption services processor.
        /// </summary>
        /// <param name="cliObject">CLI options.</param>
        public static void ProcessOperation(object cliObject)
        {
            var encryptionType = cliObject as IEncryptionObject;

            object toPrint = null;
            switch (encryptionType.CipherType)
            {
                case (CipherType.Aes256CBC):
                case (CipherType.Aes192CBC):
                    toPrint = AesEncryption(encryptionType);
                    break;

                default:
                    break;
            }

            CLIHelpers.PrintToConsole(toPrint);
        }

        /// <summary>
        ///     AES Encryption service.
        /// </summary>
        /// <param name="toEncryption"></param>
        /// <returns>Encryptioned info.</returns>
        public static SymmetricEncryption AesEncryption(IEncryptionObject toEncryption)
        {
            return AesEncryption(toEncryption.Phrase, toEncryption.KeySize);
        }

        /// <summary>
        ///     AES Encryption service.
        /// </summary>
        /// <param name="phraseToEncrypt">Phrase which will be encrypted.</param>
        /// <param name="keySize">AES key size.</param>
        /// <param name="returnAs">Data format on which will be encrypted data returned.</param>
        /// <returns></returns>
        public static SymmetricEncryption AesEncryption(string phraseToEncrypt, int keySize, EncryptedTextReturnOptions returnAs = EncryptedTextReturnOptions.Base64String)
        {
            var aesProvider = new AesProvider();
            var key = aesProvider.GenerateKey(keySize);
            var IV = aesProvider.GenerateInitializationVector();

            var encrypted = aesProvider.Encrypt(phraseToEncrypt, key, IV, CipherMode.CBC);

            return ConvertorHelpers.ConvertToBase64(IV, key, encrypted);
        }
    }

    /// <summary>
    ///     Returned data format options.
    /// </summary>
    public enum EncryptedTextReturnOptions
    {
        Base64String
    }
}