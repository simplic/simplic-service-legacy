using Simplic.Authentication;
using Simplic.Security.Cryptography;

namespace Simplic.CredentialProvider
{
    /// <summary>
    /// Default simplic studio credential provider
    /// </summary>
    public class DefaultCredentialProvider : ICredentialProvider
    {
        /// <summary>
        /// Check credentials using the simplic login system
        /// </summary>
        /// <param name="domain">Not required</param>
        /// <param name="userName">User name (required)</param>
        /// <param name="rawPassword">Entered password (required)</param>
        /// <param name="encryptedPassword">Encrypted password (required)</param>
        /// <returns>True if the login was successfull</returns>
        public bool CheckCredentials(string domain, string userName, string rawPassword, string encryptedPassword)
        {
            return CryptographyHelper.GetMD5Hash(rawPassword) == encryptedPassword;
        }
    }
}
