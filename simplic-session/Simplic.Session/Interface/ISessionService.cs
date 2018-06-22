namespace Simplic.Session
{
    /// <summary>
    /// Session service holds a variable to the current session
    /// </summary>
    public interface ISessionService
    {
        /// <summary>
        /// Gets or sets the current session
        /// </summary>
        Session CurrentSession { get; set; }
    }
}
