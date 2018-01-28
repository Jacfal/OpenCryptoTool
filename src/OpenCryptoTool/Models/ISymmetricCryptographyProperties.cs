using System.Security.Cryptography;
using System.Text;

namespace OpenCryptoTool.Models
{
    /// <summary>
    ///     Symmetric cryptography properties.
    /// </summary>
    public interface ISymmetricCryptographyProperties
    {
        /// <summary>
        ///     Symmetric key.
        /// </summary>
        byte[] Key { get; set; }
        /// <summary>
        ///     Initialization vector.
        /// </summary>
        byte[] InitializationVector { get; set; }
        /// <summary>
        ///     Cipher mode.
        /// </summary>
        CipherMode CipherMode { get; set; }
    }
}