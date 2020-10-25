using System.Windows.Input;

namespace Simplic.User.UI
{
    /// <summary>
    /// Basic interface for entities supporting state saving
    /// </summary>
    interface ISaveableViewModel
    {
        /// <summary>
        /// Save state command
        /// </summary>
        ICommand SaveCommand { get; }
    }
}
