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
        [DataRow(128, CipherMode.CBC, Constants.TestPhrase)]
        [DataRow(192, CipherMode.CBC, Constants.TestPhrase)]
        [DataRow(256, CipherMode.CBC, Constants.TestPhrase)]
        [DataRow(128, CipherMode.ECB, Constants.TestPhrase)]
        [DataRow(192, CipherMode.ECB, Constants.TestPhrase)]
        [DataRow(256, CipherMode.ECB, Constants.TestPhrase)]
        public void Aes_CLI_Success(int keySize, CipherMode cipherMode, string phrase)
        {
            // ARRANGE
            SymmetricCryptographyCliInput cliInput;
            if (keySize == 128)
            {
                if (cipherMode == CipherMode.ECB)
                    cliInput = new Aes128ECB();
                else
                    cliInput = new Aes128CBC();
            }
            else if (keySize == 192)
            {
                if (cipherMode == CipherMode.ECB)
                    cliInput = new Aes192ECB();
                else
                    cliInput = new Aes192CBC();
            }
            else
            {
                if (cipherMode == CipherMode.ECB)
                    cliInput = new Aes256ECB();
                else
                    cliInput = new Aes256CBC();
            }

            cliInput.Content = phrase;
            cliInput.Encryption = true;

            // ACT

            #region Encryption
            var cliOutputEncryption = SymmetricCryptographyServices.ProcessOperation(cliInput);
            OutputModeler.CreateOutput(cliInput, cliOutputEncryption);
            #endregion

            cliInput.Content = cliOutputEncryption.Phrase;
            cliInput.Key = cliOutputEncryption.Key;
            cliInput.InitializationVector = cliOutputEncryption.IV;
            cliInput.Encryption = false;
            cliInput.Decryption = true;

            #region Decryption
            var cliOutputDecryption = SymmetricCryptographyServices.ProcessOperation(cliInput);
            OutputModeler.CreateOutput(cliInput, cliOutputDecryption);
            #endregion

            // ASSERT
            Assert.AreEqual(cliOutputDecryption.Phrase, phrase);
        }


        // TODO failed test
        public void Aes256CBC_CLI_Decryption_Failed(string encryptedPhrase, string key, string IV)
        {
        }

        [DataTestMethod]
        [DataRow(128, CipherMode.CBC, Constants.TestPhrase)]
        [DataRow(192, CipherMode.CBC, Constants.TestPhrase)]
        [DataRow(128, CipherMode.ECB, Constants.TestPhrase)]
        [DataRow(192, CipherMode.ECB, Constants.TestPhrase)]
        public void Tdes_CLI_Success(int keySize, CipherMode cipherMode, string phrase)
        {
            // ARRANGE
            SymmetricCryptographyCliInput cliInput;
            if (keySize == 128)
            {
                if (cipherMode == CipherMode.ECB)
                    cliInput = new TDes128ECB();
                else
                    cliInput = new TDes128CBC();
            }
            else
            {
                if (cipherMode == CipherMode.ECB)
                    cliInput = new TDes192ECB();
                else
                    cliInput = new TDes192CBC();
            }

            cliInput.Content = phrase;
            cliInput.Encryption = true;

            // ACT

            #region Encryption
            var cliOutputEncryption = SymmetricCryptographyServices.ProcessOperation(cliInput);
            OutputModeler.CreateOutput(cliInput, cliOutputEncryption);
            #endregion

            cliInput.Content = cliOutputEncryption.Phrase;
            cliInput.Key = cliOutputEncryption.Key;
            cliInput.InitializationVector = cliOutputEncryption.IV;
            cliInput.Encryption = false;
            cliInput.Decryption = true;

            #region Decryption
            var cliOutputDecryption = SymmetricCryptographyServices.ProcessOperation(cliInput);
            OutputModeler.CreateOutput(cliInput, cliOutputDecryption);
            #endregion

            // ASSERT
            Assert.AreEqual(cliOutputDecryption.Phrase, phrase);
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