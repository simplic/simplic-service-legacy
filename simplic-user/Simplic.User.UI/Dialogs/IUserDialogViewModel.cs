using System;

namespace Simplic.User.UI
{
    /// <summary>
    /// View model interface for dialog window support
    /// </summary>
    public interface IUserDialogViewModel : IDialogViewModel
    {
        /// <summary>
        /// Dialog window modality flag
        /// </summary>
        bool IsModal { get; }

        /// <summary>
        /// Dialog window close request method
        /// </summary>
        void RequestClose();

        /// <summary>
        /// Notifies when a window is closed
        /// </summary>
        event EventHandler DialogClosing;
    }
}
