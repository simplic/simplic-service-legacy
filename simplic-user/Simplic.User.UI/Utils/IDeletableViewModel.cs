using System.Windows.Input;

namespace Simplic.User.UI
{
    interface IDeletableViewModel
    {
        ICommand DeleteCommand { get; }
    }
}
