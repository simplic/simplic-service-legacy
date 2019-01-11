namespace Simplic.Session
{
    /// <summary>
    /// Tenant changed delegate
    /// </summary>
    /// <param name="sender">Sender</param>
    /// <param name="args">Args</param>
    public delegate void OrganizationSelectionChangedEventHandler(object sender, SelectedOrganizationsChangedArgs args);

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
