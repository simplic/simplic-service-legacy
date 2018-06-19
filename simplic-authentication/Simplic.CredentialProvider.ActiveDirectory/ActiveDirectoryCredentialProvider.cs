using Simplic.Authentication;
using System.DirectoryServices.AccountManagement;

namespace Simplic.CredentialProvider.ActiveDirectory
{
    /// <summary>
    /// Active directory provider
    /// </summary>
    public class ActiveDirectoryCredentialProvider : ICredentialProvider
    {
        /// <summary>
        /// Check credentials using ad
        /// </summary>
        /// <param name="domain">Domain name (required)</param>
        /// <param name="userName">User name (required)</param>
        /// <param name="rawPassword">Entered password (required)</param>
        /// <param name="encryptedPassword">Not requred</param>
        /// <returns>True if the login was successfull</returns>
        public bool CheckCredentials(string domain, string userName, string rawPassword, string encryptedPassword)
        {
            using (var pc = new PrincipalContext(ContextType.Domain, domain))
            {
                // validate the credentials
                return pc.ValidateCredentials(userName.Trim(), rawPassword);
            }
        }
    }
}
