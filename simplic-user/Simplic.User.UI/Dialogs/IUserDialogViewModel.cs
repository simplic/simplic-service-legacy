using System;

namespace Simplic.User.UI
{
    public interface IUserDialogViewModel : IDialogViewModel
    {
        bool IsModal { get; }
        void RequestClose();
        event EventHandler DialogClosing;
    }
}
