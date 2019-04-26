namespace Simplic.Session.Service
{
    /// <summary>
    /// Session service holds a variable to the current session
    /// </summary>
    public class SessionService : ISessionService
    {
        /// <summary>
        /// Gets or sets the current session
        /// </summary>
        public Session CurrentSession
        {
            get;
            set;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public SessionService()
        {
            // empty constructor
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="session">Session object</param>
        public SessionService(Session session)
        {
            CurrentSession = session;
        }
    }
}
