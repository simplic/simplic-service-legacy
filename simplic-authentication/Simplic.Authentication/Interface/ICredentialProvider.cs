namespace Simplic.Authentication
{
    /// <summary>
    /// Provider for checking entered credentials
    /// </summary>
    public interface ICredentialProvider
    {
        /// <summary>
        /// Cehcl credentials and returns true, if the credentials are valid
        /// </summary>
        /// <param name="domain">User domain</param>
        /// <param name="userName">User name</param>
        /// <param name="rawPassword">Raw password</param>
        /// <param name="encryptedPassword">Encrypted password</param>
        /// <returns>True if the credentials are valid</returns>
        bool CheckCredentials(string domain, string userName, string rawPassword, string encryptedPassword);
    }
}
