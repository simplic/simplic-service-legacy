using Simplic.UserSession;

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
        Session Login(string domain, string userName, string password);
    }
}
