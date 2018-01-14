using CommandLine;
using OpenCryptoTool.Models;
using System;
using System.Security.Cryptography;

namespace OpenCryptoTool
{
    class Program
    {
        // TODO logs
        // TODO json
        static void Main(string[] args)
        {
            var result = Parser.Default.ParseArguments<Aes256CBC, Aes192CBC>(args);

            result
                .WithParsed(obj => 
                {
                    Console.WriteLine(SymmetricCryptographyServices.ProcessOperation(obj));
                }); // TODO with no parsed
        }
    }
}