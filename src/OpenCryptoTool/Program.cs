using CommandLine;
using OpenCryptoTool.Models;
using Serilog;
using System;
using System.Security.Cryptography;

namespace OpenCryptoTool
{
    class Program
    {
        // TODO json
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
                var result = Parser.Default.ParseArguments<Aes256CBC, Aes192CBC>(args);

                result
                    .WithParsed(obj =>
                    {
                        var cryptoResult = SymmetricCryptographyServices.ProcessOperation(obj);

                        OutputModeler.CreateOutput(obj as BaseCryptographyCliInput, cryptoResult);
                    }); // TODO with no parsed
            }
            catch (Exception e)
            {
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