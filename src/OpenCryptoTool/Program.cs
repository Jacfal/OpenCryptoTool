using CommandLine;
using System;
using System.Security.Cryptography;

namespace OpenCryptoTool
{
    class Program
    {
        static void Main(string[] args)
        {
            var result = Parser.Default.ParseArguments<Aes256CBC, Aes192CBC>(args);

            result
                .WithParsed(obj => EncryptionServices.ProcessOperation(obj)); // TODO with no parsed
        }
    }
}
