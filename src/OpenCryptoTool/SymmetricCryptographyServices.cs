using OpenCryptoTool.Helpers;
using OpenCryptoTool.Models;
using System;
using System.Security.Cryptography;

namespace OpenCryptoTool
{
    /// <summary>
    ///     Symmetric cryptography services.
    /// </summary>
    public static class SymmetricCryptographyServices
    {
        /// <summary>
        ///     Encryption services processor.
        /// </summary>
        /// <param name="cliInput">CLI options.</param>
        public static SymmetricCryptographyCliOutput ProcessOperation(object cliInput)
        {
            var symmetricCryptographyCli = cliInput as ISymmetricCryptographyCliInput;

            SymmetricCryptographyCliOutput cliOutput = null;
            switch (symmetricCryptographyCli.CipherType.CryptographyStandard)
            {
                case (CryptographyStandard.Aes):
                    cliOutput = AesCryptography(symmetricCryptographyCli);
                    break;

                default:
                    break;
            }

            return cliOutput;
        }

        /// <summary>
        ///     AES cryptography processor.code
        /// </summary>
        /// <param name="cliObject"></param>
        public static SymmetricCryptographyCliOutput AesCryptography(ISymmetricCryptographyCliInput cliObject)
        {
            if (cliObject.Encryption)
            {
                return AesEncryption(cliObject);
            }
            else if (cliObject.Decryption)
            {
                return AesDecryption(cliObject);
            }
            else
            {
                // TODO error, only enc, dec ops are allowed at this time
                throw new NotImplementedException();
            }
        }

        /// <summary>
        ///     AES Encryption service.
        /// </summary>
        /// <param name="toEncryption">CLI input model.</param>
        /// <returns>CLI output model with information about decryption.</returns>
        public static SymmetricCryptographyCliOutput AesEncryption(ISymmetricCryptographyCliInput toEncryption)
        {
            var aesProvider = new AesProvider();
            byte[] key;
            byte[] IV;

            // check if user provide encryption key
            if (!string.IsNullOrEmpty(toEncryption.Key))
            {
                key = Convert.FromBase64String(toEncryption.Key);
            }
            else
            {
                key = aesProvider.GenerateKey(toEncryption.CipherType.KeySize);
            }

            // check if user provide IV
            if (!string.IsNullOrEmpty(toEncryption.InitializationVector))
            {
                Console.WriteLine("Using same initialization vector for more then one encryption is not recommended!");
                IV = Convert.FromBase64String(toEncryption.InitializationVector);
            }
            else
            {
                IV = aesProvider.GenerateInitializationVector();
            }

            var encrypted = aesProvider.Encrypt(toEncryption.Phrase, key, IV, toEncryption.CipherType.CipherMode);

            return new SymmetricCryptographyCliOutput(IV, key, encrypted);
        }

        /// <summary>
        ///     AES Decryption service.
        /// </summary>
        /// <param name="toDecryption">CLI input model.</param>
        /// <returns>CLI output model with information about decryption.</returns>
        public static SymmetricCryptographyCliOutput AesDecryption(ISymmetricCryptographyCliInput toDecryption)
        {
            if (string.IsNullOrEmpty(toDecryption.Phrase))
            {
                toDecryption.Phrase = CLIHelpers.InformationProvider("Enter entrycpted phrase");
            }

            if (string.IsNullOrEmpty(toDecryption.Key))
            {
                toDecryption.Key = CLIHelpers.InformationProvider("Enter encryption key");
            }

            if (string.IsNullOrEmpty(toDecryption.InitializationVector))
            {
                toDecryption.InitializationVector = CLIHelpers.InformationProvider("Enter initialization vector");
            }

            var aesProvider = new AesProvider(toDecryption.Key, toDecryption.InitializationVector, toDecryption.CipherType.CipherMode);
            var decrypted =  aesProvider.Decrypt(toDecryption.Phrase);

            return new SymmetricCryptographyCliOutput(decrypted);
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