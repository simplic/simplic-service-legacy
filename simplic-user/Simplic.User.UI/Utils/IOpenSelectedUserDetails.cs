using System.Windows.Input;

namespace Simplic.User.UI.Utils
{
    /// <summary>
    /// Basic interface for opening details of a selected user
    /// </summary>
    interface IOpenSelectedUserDetails
    {
        /// <summary>
        /// Open the details dialog for the selected user command
        /// </summary>
        ICommand OpenSelectedUserDetailsCommand
        {
            get;
            set;
        }
    }
}
