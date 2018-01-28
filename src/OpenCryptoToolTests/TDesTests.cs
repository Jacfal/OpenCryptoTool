using System.Security.Cryptography;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenCryptoTool.Providers;

namespace OpenCryptoToolTests
{
    [TestClass]
    public class TDesTests
    {
        [DataTestMethod]
        [DataRow("Hello, world!", 192)]
        [DataRow("Another test string .!;", 128)]
        [DataRow("Test string![]{}%", 192)]
        public void TDes_Encryption_Decryption_Success(string testPhrase, int keySize)
        {
            // ARRANGE
            var tdesCrypto = new TripleDesProvider();

            var key = tdesCrypto.GenerateKey(keySize);
            var IV = tdesCrypto.GenerateInitializationVector();

            // ACT
            var encrypted = tdesCrypto.Encrypt(testPhrase, key, IV, CipherMode.CBC);
            var decrypted = tdesCrypto.Decrypt(encrypted, key, IV, CipherMode.CBC);

            // ASSERT
            Assert.AreEqual(testPhrase, decrypted);
        } 

        [DataTestMethod]
        [DataRow(120)]
        [DataRow(256)]
        [DataRow(56)]
        public void TDes_InvalidKey_Error(int keySize)
        {
            // ARRANGE
            var tdesCrypto = new TripleDesProvider();

            CryptographicException cryptographyError = null;
            try
            {
                var key = tdesCrypto.GenerateKey(keySize);
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