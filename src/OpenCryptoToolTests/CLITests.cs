using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenCryptoTool;
using OpenCryptoTool.Models;
using System;
using System.IO;
using System.Reflection;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace OpenCryptoToolTests.CLITests
{
    [TestClass]
    public class CLITests
    {
        [DataTestMethod]
        [DataRow("Hello from unit test!", null, null, null)]
        [DataRow("Hello from unit test!", "KKmBeaqfp9SQCpiHl3wd4zHcysTHo+8NqivZJZqG600=", null, null)]
        [DataRow("Hello from unit test!", "KKmBeaqfp9SQCpiHl3wd4zHcysTHo+8NqivZJZqG600=", "dQrXyGIYwEEP2hCjbDzrww==", "fEYga6aCnnMoiAhqzc490RPGpJQTStOqAH4tRe4kIgI=")]
        public void Aes256CBC_CLI_Encryption_Success(string phrase, string key, string IV, string expectedOutput)
        {
            // ARRANGE
            var cliInput = new Aes256CBC()
            {
                Encryption = true,
                Content = phrase,
                Key = key,
                InitializationVector = IV
            };

            // ACT
            var cliOutput = SymmetricCryptographyServices.ProcessOperation(cliInput);
            OutputModeler.CreateOutput(cliInput, cliOutput);

            // ASSERT
            var keyLength = Convert.FromBase64String(cliOutput.Key).Length;
            var IVLength = Convert.FromBase64String(cliOutput.IV).Length;

            // try convert phrase from base64
            Convert.FromBase64String(cliOutput.Phrase);

            Assert.AreEqual(IVLength, 16);
            Assert.AreEqual(keyLength, 32);

            if (!string.IsNullOrEmpty(key) && !string.IsNullOrEmpty(IV))
            {
                Assert.AreEqual(cliOutput.Phrase, expectedOutput);
            }
        }

        [DataTestMethod]
        [DataRow("uI+LEV/tOOSGPHLYist7GNpQ1RSXLrUtXYo3N9PnEdE=", "dHXCGIJnhpAoUlXlUFvUSw1x2/xFe+055H2wG153qPE=", "sE2s2dAUJLuvHE2dzlONjw==", "Hello word! My name is Jacob.")]
        [DataRow("Vq1W5PkJzLq36QxictNU/chbTDs3mTK1A5Z6n1FUpYEZjXmEAQUnPL/+52VML+sf", "KKmBeaqfp9SQCpiHl3wd4zHcysTHo+8NqivZJZqG600=", "dQrXyGIYwEEP2hCjbDzrww==", "82ead9a4-0358-4246-a524-79743f337691")]
        [DataRow(
            "0YJO1xH8tf2jh6QI/AqYw+OJwyfRVoBg5WKPhxr3fans+fTZb8qhS7NTNxNHv9pu31caJOguYvNThrzpIdv9D5aKc0RrT4bnhGRK8BER5F9i31lolBRPJkFEOKSr/s2RSVwjM2sR7vEk+ZPuxCx7FQI+lBJww6AgBUJ9DWa+ubOONy5GsxbNKou8rd3uORXVgU79ZuoLjTIMuX/CtuDd+eXS01l1Qz1RrEYF3oFCW2zenC8f2ArVEskzf9qRbQZVcX5j4CPpybOsKlXoHJ1m33nkyr3XLv/xpjKZev7PAqS+mZYBSAclEN/q+1lHk3+ZeJzz3lnGzs6nQX/Ss8d6ADz3CCtbt3YRC6UkxMdf+Kfp6ABiJ+4QFJ4isxDoK5Xwe7qV38fQk7Ou6JdCeH/jJIDx+mG89lU43bthVZUtjeW3KU3Mx8v+/9YmVPOS1QQUUJ7CaX09hMuMHca/xa4JhX/XdXP23UTyqF5sI60BZPFRvpsY0KGSZjhEZIyAIuLgbQHZvG9w4brqdqf2xMB69KcTw4DpFiq/fvIy+FkiO78N66KbE+crEMy0Dcr47vSRbjqnzMbeQZUeBksIaCN5Lg==",
            "Avd3wj/TALWi12A0Dt7XBVPMciBcHT4yTjaM8tBE0tE=",
            "0T7+GeIyH7EdvQyBnQ9upA==",
            "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.")]
        public void Aes256CBC_CLI_Decryption_Success(string encryptedPhrase, string key, string IV, string expectedOutput)
        {
            // ARRANGE
            var cliInput = new Aes256CBC()
            {
                Decryption = true,
                Content = encryptedPhrase,
                Key = key,
                InitializationVector = IV
            };

            // ACT
            var cliOutput = SymmetricCryptographyServices.ProcessOperation(cliInput);
            OutputModeler.CreateOutput(cliInput, cliOutput);

            // ASSERT
            Assert.AreEqual(expectedOutput, cliOutput.Phrase);
        }

        // TODO TDes CLI ECB test

        // TODO failed test
        public void Aes256CBC_CLI_Decryption_Failed(string encryptedPhrase, string key, string IV)
        {
        }

        [TestMethod]
        public async Task OpenInputFile_Success()
        {
            // ASSERT
            var testTextFile = "TestTextFile.txt";
            CreateTestTextFile(testTextFile);

            var testLocation = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            var fullPath = Path.Combine(testLocation, testTextFile);

            var testFileContent = await File.ReadAllLinesAsync(fullPath);

            // ACT
            var testInput = new Aes256CBC()
            {
                InputFilePath = fullPath
            };

            // ARRANGE
            Assert.AreEqual(testInput.Content, string.Join('\n', testFileContent));
        }

        [TestMethod]
        public void OpenInputFile_Failed()
        {
            // ASSERT
            var testTextFile = "NonExistTestTextFile.txt";

            var testLocation = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            var fullPath = Path.Combine(testLocation, testTextFile);

            FileNotFoundException exception = null;

            // ACT
            try
            {
                var testInput = new Aes256CBC()
                {
                    InputFilePath = fullPath
                };
            }
            catch (FileNotFoundException e)
            {
                exception = e;
            }

            // ARRANGE
            Assert.IsNotNull(exception);
        }

        private void CreateTestTextFile(string fileName)
        {
            var testFileText = new string[] { "Hello, world!", "How are you?", "Greetings from Jacob!" };
            File.WriteAllLines(fileName, testFileText);
        }
    }

}