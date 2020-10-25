using System.Windows.Input;

namespace Simplic.User.UI.Utils
{
    interface IOpenSelectedUserDetails
    {
        ICommand OpenSelectedUserDetailsCommand
        {
            get;
            set;
        }
    }
}
