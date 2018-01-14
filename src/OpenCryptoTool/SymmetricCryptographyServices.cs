using OpenCryptoTool.Helpers;
using OpenCryptoTool.Models;
using Serilog;
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

            Log.Information($"Command for {symmetricCryptographyCli.CipherType.CryptographyStandard.ToString()} de/encryption successfully parsed.");

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
        ///     AES cryptography processor.
        /// </summary>
        /// <param name="cliObject"></param>
        public static SymmetricCryptographyCliOutput AesCryptography(ISymmetricCryptographyCliInput cliObject)
        {
            if (cliObject.Encryption)
            {
                // encryption
                return AesEncryption(cliObject); // TODO opravit... en/decryption should be enum
            }
            else
            {
                // decryption
                return AesDecryption(cliObject);
            }
        }

        /// <summary>
        ///     AES Encryption service.
        /// </summary>
        /// <param name="toEncryption">CLI input model.</param>
        /// <returns>CLI output model with information about decryption.</returns>
        public static SymmetricCryptographyCliOutput AesEncryption(ISymmetricCryptographyCliInput toEncryption)
        {
            Log.Information($"New aes encryption request => {toEncryption.CipherType}");

            var aesProvider = new AesProvider();
            byte[] key;
            byte[] IV;

            // check if user provide encryption key
            if (!string.IsNullOrEmpty(toEncryption.Key))
            {
                Log.Information("Working with the provided encryption key.");

                key = Convert.FromBase64String(toEncryption.Key);
            }
            else
            {
                Log.Information("Generating new encryption key.");

                key = aesProvider.GenerateKey(toEncryption.CipherType.KeySize);
            }

            // check if user provide IV
            if (!string.IsNullOrEmpty(toEncryption.InitializationVector))
            {
                Log.Information("Working with the provided initialization vector.");

                Console.WriteLine("Using same initialization vector for more then one encryption is not recommended!");
                IV = Convert.FromBase64String(toEncryption.InitializationVector);
            }
            else
            {
                Log.Information("Generating new initialization vector.");

                IV = aesProvider.GenerateInitializationVector();
            }

            var encrypted = aesProvider.Encrypt(toEncryption.Phrase, key, IV, toEncryption.CipherType.CipherMode);

            Log.Information("Successfully encrypted.");

            return new SymmetricCryptographyCliOutput(IV, key, encrypted);
        }

        /// <summary>
        ///     AES Decryption service.
        /// </summary>
        /// <param name="toDecryption">CLI input model.</param>
        /// <returns>CLI output model with information about decryption.</returns>
        public static SymmetricCryptographyCliOutput AesDecryption(ISymmetricCryptographyCliInput toDecryption)
        {
            Log.Information($"New aes decryption request => {toDecryption.CipherType}");

            if (string.IsNullOrEmpty(toDecryption.Phrase))
            {
                Log.Information("Data which should be decrypted missing - asking user for input.");

                toDecryption.Phrase = CLIHelpers.InformationProvider("Enter entrycpted phrase");
            }

            if (string.IsNullOrEmpty(toDecryption.Key))
            {
                Log.Information("The encryption key is missing - asking user for input.");

                toDecryption.Key = CLIHelpers.InformationProvider("Enter encryption key");
            }

            if (string.IsNullOrEmpty(toDecryption.InitializationVector))
            {
                Log.Information("The initialization vector is missing - asking user for input");

                toDecryption.InitializationVector = CLIHelpers.InformationProvider("Enter initialization vector");
            }

            var aesProvider = new AesProvider(toDecryption.Key, toDecryption.InitializationVector, toDecryption.CipherType.CipherMode);
            var decrypted =  aesProvider.Decrypt(toDecryption.Phrase);

            Log.Information("Successfully decrypted.");

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