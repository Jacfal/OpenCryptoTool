using System;
using System.Security.Cryptography;
using System.Text;

namespace OpenCryptoTool.Models
{
    /// <summary>
    ///     Symmetric cryptography properties.
    /// </summary>
    public class SymmetricCryptographyProperties : ISymmetricCryptographyProperties
    {
        /// <summary>
        ///     Create new cryptography properties model.
        /// </summary>
        public SymmetricCryptographyProperties()
        {
        }

        /// <summary>
        ///     Create new cryptography properties model.
        /// </summary>
        /// <param name="key">Symmetric key.</param>
        /// <param name="cipherMode">Cipher mode.</param>
        public SymmetricCryptographyProperties(string key, CipherMode cipherMode)
        {
            Key = Convert.FromBase64String(key.Trim());
            CipherMode = cipherMode;
        }

        /// <summary>
        ///     Create new cryptography properties model.
        /// </summary>
        /// <param name="key">Symmetric key.</param>
        /// <param name="IV">Initialization vector.</param>
        /// <param name="cipherMode">Cipher mode.</param>

        public SymmetricCryptographyProperties(string key, string IV, CipherMode cipherMode)
            : this (key, cipherMode)
        {
            InitializationVector = Convert.FromBase64String(IV.Trim());
        }

        /// <summary>
        ///     Symmetric key.
        /// </summary>
        public byte[] Key { get; set; }
        /// <summary>
        ///     Initialization vector.
        /// </summary>
        public byte[] InitializationVector { get; set; }
        /// <summary>
        ///     Cipher mode.
        /// </summary>
        public CipherMode CipherMode { get; set; }
    }
}