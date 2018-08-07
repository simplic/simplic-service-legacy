using System;

namespace Simplic.Authentication
{
    /// <summary>
    /// Exception that will be thrown, if the login process fails
    /// </summary>
    public class LoginFailedException : Exception
    {
        /// <summary>
        /// Create login failed exception
        /// </summary>
        /// <param name="type">Login failed type</param>
        public LoginFailedException(LoginFailedType type) : base("Login failed")
        {
            Type = type;
        }

        /// <summary>
        /// Gets or sets the login failed type
        /// </summary>
        public LoginFailedType Type
        {
            get;
            set;
        }
    }
}
