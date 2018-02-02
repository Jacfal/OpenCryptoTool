using CommandLine;
using OpenCryptoTool.Models;
using Serilog;
using System;
using System.Security.Cryptography;

namespace OpenCryptoTool
{
    class Program
    {
        static void Main(string[] args)
        {
            var loggerConfiguration = new LoggerConfiguration()
                .WriteTo.File("./Log/OpenCryptoTool.log", rollingInterval: RollingInterval.Month,
                    outputTemplate: "[{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz}] {Message:lj}{NewLine}{Exception}");

            #if DEBUG

            loggerConfiguration.WriteTo.Trace();

            #endif

            Log.Logger = loggerConfiguration.CreateLogger();
            Log.Information("");
            Log.Information("Operation started.");
            
            try
            {
                var result = Parser.Default.ParseArguments<
                    Aes256CBC,
                    Aes192CBC,
                    Aes128CBC,
                    Aes256ECB,
                    Aes192ECB,
                    Aes128ECB,
                    TDes192CBC,
                    TDes128CBC,
                    TDes192ECB,
                    TDes128ECB
                    >(args);

                result
                    .WithParsed(obj =>
                    {
                        var symmetricCliInput = obj as ISymmetricCryptographyCliInput;

                        var cryptoResult = SymmetricCryptographyServices.ProcessOperation(symmetricCliInput);

                        OutputModeler.CreateOutput(obj as BaseCryptographyCliInput, cryptoResult);
                    }); // TODO with no parsed
            }
            catch (Exception e)
            {
                // TODO if invalid key or IV is used, then invalid padding mode exception is raised... (more human readable info required)
                Console.WriteLine(e.Message);
                Log.Information(e.ToString());
            }
            finally
            {
                Log.Information("Operation complete.");
            }
        }
    }
}