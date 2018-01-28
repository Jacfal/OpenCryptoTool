using OpenCryptoTool.Models;
using System;
using System.Reflection;
using System.Security.Cryptography;

namespace OpenCryptoTool.Helpers
{
    /// <summary>
    ///     Command line interface helpers.
    /// </summary>
    public static class CLIHelpers
    {
        /// <summary>
        ///     This helper converts all object properties into the single string.
        /// </summary>
        /// <param name="input">Object which will be printed into the console.</param>
        public static string PropertiesToString(object input)
        {
            string stringToReturn = "";

            var properties = input.GetType().GetProperties();

            foreach (var property in properties)
            {
                var propertyName = property.Name;
                var propertyValue = property.GetValue(input);

                if (propertyValue is string && propertyValue != null)
                {
                    stringToReturn += ($"{propertyName}:{propertyValue}\n");
                }
            }
            
            return stringToReturn;
        }

        /// <summary>
        ///     Ask the user for needed information via CLI.
        /// </summary>
        /// <param name="hint">Phrase wich will be shown to user.</param>
        /// <returns>User input.</returns>
        public static string InformationProvider(string hint)
        {
            Console.Write($"{hint}:");
            return Console.ReadLine();
        }
    }
}