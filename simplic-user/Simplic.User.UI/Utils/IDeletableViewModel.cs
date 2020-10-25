using System.Windows.Input;

namespace Simplic.User.UI
{
    /// <summary>
    /// Implemented by an entity that can be removed from the UI
    /// </summary>
    interface IDeletableViewModel
    {
        /// <summary>
        /// Delete entity command
        /// </summary>
        ICommand DeleteCommand { get; }
    }
}
