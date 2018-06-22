namespace Simplic.Authentication
{
    /// <summary>
    /// Authentication service handles logging a user in and holding the current session data
    /// </summary>
    public interface IAuthenticationService
    {
        /// <summary>
        /// Authenticate a user and create a use session
        /// </summary>
        /// <param name="domain">Domain</param>
        /// <param name="userName">User name</param>
        /// <param name="password">Password</param>
        /// <returns>A user session if the user could be logged in, else an exception will be thrown</returns>
        Simplic.Session.Session Login(string domain, string userName, string password);

        /// <summary>
        /// Activate autologin
        /// </summary>
        /// <param name="domain">Current domain</param>
        /// <param name="userName">Current user</param>
        /// <param name="password">Current password</param>
        void SetAutologin(string domain, string userName, string password);

        /// <summary>
        /// Remove autologin for the current windows user
        /// </summary>
        void RemoveAutologin();

        /// <summary>
        /// Check whether autologin is existing and valid for the current user
        /// </summary>
        /// <returns>Simplic session if login was successfull, or null</returns>
        Simplic.Session.Session TryAutologin();
    }
}
