using System.Security.Cryptography;
using OpenCryptoTool;
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
        public void EncryptionDecryptionTestSuccess(string testPhrase, int keySize)
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
    }
}
