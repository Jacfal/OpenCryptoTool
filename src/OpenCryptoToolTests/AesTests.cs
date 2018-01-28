using System.Security.Cryptography;
using OpenCryptoTool.Providers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AesTests
{
    [TestClass]
    public class AesTests
    {
        [DataTestMethod]
        [DataRow("Hello, world!", 256)]
        [DataRow("Another test string .!;", 192)]
        [DataRow("Test string![]{}%", 128)]
        public void Aes_Encryption_Decryption_Success(string testPhrase, int keySize)
        {
            // ARRANGE
            var aesCrypto = new AesProvider();

            var key = aesCrypto.GenerateKey(keySize);
            var IV = aesCrypto.GenerateInitializationVector();

            // ACT
            var encrypted = aesCrypto.Encrypt(testPhrase, key, IV, CipherMode.CBC);
            var decrypted = aesCrypto.Decrypt(encrypted, key, IV, CipherMode.CBC);

            // ASSERT
            Assert.AreEqual(testPhrase, decrypted);
        }

        [DataTestMethod]
        [DataRow(150)]
        [DataRow(289)]
        [DataRow(56)]
        public void Aes_InvalidKey_Error(int keySize)
        {
            // ARRANGE
            var aesCrypto = new AesProvider();

            CryptographicException cryptographyError = null;
            try
            {
                var key = aesCrypto.GenerateKey(keySize);
            }
            catch (CryptographicException e)
            {
                cryptographyError = e;
            }

            // ASSERT
            Assert.IsNotNull(cryptographyError);
        }
    }
}
