using System.Windows.Input;

namespace Simplic.User.UI
{
    interface ISaveableViewModel
    {
        ICommand SaveCommand { get; }
    }
}
