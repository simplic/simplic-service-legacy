namespace Simplic.Authentication
{
    /// <summary>
    /// Type of login failed
    /// </summary>
    public enum LoginFailedType
    {
        /// <summary>
        /// User not found
        /// </summary>
        UserNotFound,

        /// <summary>
        /// User is inactive
        /// </summary>
        UserNotActive,

        /// <summary>
        /// No password
        /// </summary>
        PasswordNotEntered,

        /// <summary>
        /// No username
        /// </summary>
        UserNameNotEntered,

        /// <summary>
        /// Invalid credentials
        /// </summary>
        InvalidCredentials
    }
}
