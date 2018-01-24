using Newtonsoft.Json;
using OpenCryptoTool.Helpers;
using OpenCryptoTool.Models;
using Serilog;
using System;
using System.Collections.Generic;
using System.Text;

namespace OpenCryptoTool
{
    public static class OutputModeler
    {
        public static void CreateOutput(BaseCryptographyCliInput input, object toOutput)
        {
            // TODO to output check

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
            if (string.IsNullOrEmpty(input.OutputFile))
            {
                PrintOutputToConsole(formattedOutput);
            }
            else
            {
                // to file
            }
        }

        private static void PrintOutputToConsole(string output)
        {
            Log.Information("Printing output to console.");
            Console.WriteLine(output);
        }
    }
}
