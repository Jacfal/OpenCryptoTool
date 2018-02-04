using OpenCryptoTool.Helpers;
using OpenCryptoTool.Models;
using OpenCryptoTool.Providers;
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
        public static SymmetricCryptographyCliOutput ProcessOperation(ISymmetricCryptographyCliInput cliInput)
        {
            Log.Information($"Command for {cliInput.CipherType.CryptographyStandard.ToString()} de/encryption successfully parsed.");

            // warning when IV is entered by user and ECB cipher mode is used
            if (!string.IsNullOrEmpty(cliInput.InitializationVector) && cliInput.CipherType.CipherMode == CipherMode.ECB)
            {
                Log.Information("Initialization vector is not valid for ECB cipher mode and will be ignored.");
            }

            // Choosing cipher type
            CryptoProvider cryptoProvider;
            switch (cliInput.CipherType.CryptographyStandard)
            {
                case CryptographyStandard.Aes:
                    cryptoProvider = new AesProvider();
                    break;

                case CryptographyStandard.TripleDes:
                    cryptoProvider = new TripleDesProvider();
                    break;

                default:
                    throw new CryptographicException($"Unknown cryptography standard {cliInput.CipherType.CryptographyStandard}.");
            }


            if (cliInput.Encryption)
            {
                return EncryptionRequest(cliInput, cryptoProvider); // TODO opravit... en/decryption should be enum
            }
            else
            {
                return DecryptionRequest(cliInput, cryptoProvider);
            }
        }

        /// <summary>
        ///     AES Encryption service.
        /// </summary>
        /// <param name="toEncryption">CLI input model.</param>
        /// <returns>CLI output model with information about decryption.</returns>
        public static SymmetricCryptographyCliOutput EncryptionRequest(ISymmetricCryptographyCliInput toEncryption, CryptoProvider cryptoProvider)
        {
            Log.Information($"New aes encryption request => {toEncryption.CipherType}");

            byte[] key = null;
            byte[] IV = null;

            // check if user provide encryption key
            if (!string.IsNullOrEmpty(toEncryption.Key))
            {
                Log.Information("Working with the provided encryption key.");

                key = Convert.FromBase64String(toEncryption.Key);
            }
            else
            {
                Log.Information("Generating new encryption key.");

                key = cryptoProvider.GenerateKey(toEncryption.CipherType.KeySize);
            }

            // check if user provide IV
            if (!string.IsNullOrEmpty(toEncryption.InitializationVector) && toEncryption.CipherType.CipherMode != CipherMode.ECB)
            {
                Log.Information("Working with the provided initialization vector.");

                Console.WriteLine("Using same initialization vector for more then one encryption is not recommended!");
                IV = Convert.FromBase64String(toEncryption.InitializationVector);
            }
            else
            {
                Log.Information("Generating new initialization vector.");

                IV = cryptoProvider.GenerateInitializationVector();
            }

            var encrypted = cryptoProvider.Encrypt(toEncryption.Content, key, IV, toEncryption.CipherType.CipherMode);

            Log.Information("Successfully encrypted.");

            if (toEncryption.CipherType.CipherMode == CipherMode.ECB)
            {
                return new SymmetricCryptographyCliOutput(key, encrypted, toEncryption.CipherType);
            }
            else
            {
                return new SymmetricCryptographyCliOutput(IV, key, encrypted, toEncryption.CipherType);
            }
        }

        /// <summary>
        ///     AES Decryption service.
        /// </summary>
        /// <param name="toDecryption">CLI input model.</param>
        /// <returns>CLI output model with information about decryption.</returns>
        public static SymmetricCryptographyCliOutput DecryptionRequest(ISymmetricCryptographyCliInput toDecryption, CryptoProvider cryptoProvider)
        {
            Log.Information($"New aes decryption request => {toDecryption.CipherType}");

            if (string.IsNullOrEmpty(toDecryption.Content))
            {
                Log.Information("Data which should be decrypted missing - asking user for input.");

                toDecryption.Content = CLIHelpers.InformationProvider("Enter entrycpted phrase");
            }

            if (string.IsNullOrEmpty(toDecryption.Key))
            {
                Log.Information("The encryption key is missing - asking user for input.");

                toDecryption.Key = CLIHelpers.InformationProvider("Enter encryption key");
            }

            if (string.IsNullOrEmpty(toDecryption.InitializationVector) && toDecryption.CipherType.CipherMode != CipherMode.ECB)
            {
                Log.Information("The initialization vector is missing - asking user for input");

                toDecryption.InitializationVector = CLIHelpers.InformationProvider("Enter initialization vector");
            }

            SymmetricCryptographyProperties cryptoProperties;
            if (toDecryption.CipherType.CipherMode == CipherMode.ECB)
            {
                // ignore IV when ECB cipher mode is used
                cryptoProperties = new SymmetricCryptographyProperties(toDecryption.Key, toDecryption.CipherType.CipherMode);
            }
            else
            {
                cryptoProperties = new SymmetricCryptographyProperties(toDecryption.Key, toDecryption.InitializationVector, toDecryption.CipherType.CipherMode);
            }

            cryptoProvider.CryptographyProperties = cryptoProperties;
            var decrypted = cryptoProvider.Decrypt(toDecryption.Content);

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