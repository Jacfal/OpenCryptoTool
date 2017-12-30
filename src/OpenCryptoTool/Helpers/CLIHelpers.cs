using System;
using System.Reflection;

namespace OpenCryptoTool.Helpers
{
    /// <summary>
    ///     Command line interface helpers.
    /// </summary>
    public static class CLIHelpers
    {
        /// <summary>
        ///     Print object to console.
        /// </summary>
        /// <param name="toPrint">Object which will be printed into the console.</param>
        public static void PrintToConsole(object toPrint)
        {
            var properties = toPrint.GetType().GetProperties();

            foreach (var property in properties)
            {
                var propertyName = property.Name;
                var propertyValue = property.GetValue(toPrint);

                if (propertyValue is string)
                {
                    Console.WriteLine($"{propertyName}: {propertyValue}");
                }
            }
        }
    }
}