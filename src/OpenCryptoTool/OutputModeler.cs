using Newtonsoft.Json;
using OpenCryptoTool.Helpers;
using OpenCryptoTool.Models;
using Serilog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace OpenCryptoTool
{
    public static class OutputModeler
    {
        public static void CreateOutput(BaseCryptographyCliInput input, object toOutput)
        {
            // Get required format
            string formattedOutput = default(string);
            switch (input.OutputFormat)
            {
                case OutputFormat.json:
                    formattedOutput = JsonConvert.SerializeObject(toOutput);
                    Log.Information("Output serialized to JSON object.");
                    break;

                default:
                    formattedOutput = CLIHelpers.PropertiesToString(toOutput);
                    Log.Information("Output serialized to console human readable object.");
                    break;
            }

            // Process to output
            if (string.IsNullOrEmpty(input.OutputFilePath))
            {
                Log.Information("Printing output to the console.");
                Console.WriteLine(formattedOutput);
            }
            else
            {
                Log.Information($"Data will be saved into the file {input.OutputFilePath}.");
                File.WriteAllText(input.OutputFilePath, formattedOutput);
            }
        }
    }
}
