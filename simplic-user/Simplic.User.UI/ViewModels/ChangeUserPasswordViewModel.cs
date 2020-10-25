using Simplic.UI.MVC;
using System;
using System.Windows.Input;
using Telerik.Windows.Controls;

namespace Simplic.User.UI
{
    /// <summary>
    /// View model for password change dialog window
    /// </summary>
    class ChangeUserPasswordViewModel : Simplic.UI.MVC.ViewModelBase, IUserDialogViewModel, ISaveableViewModel
    {
        #region fields
        private readonly UserViewModel _user;
        private ICommand _changePasswordCommand;
        private string _password;
        #endregion

        #region events
        /// <summary>
        /// Notifies when a window is closed. Inherited from <see cref="IUserDialogViewModel"/>
        /// </summary>
        public event EventHandler DialogClosing;
        #endregion

        #region ctr
        /// <summary>
        /// Constructor for the view model
        /// </summary>
        /// <param name="user">The current user for whom the password is changed</param>
        public ChangeUserPasswordViewModel(UserViewModel user)
        {
            _user = user;
            SaveCommand = new RelayCommand(OnChangePassword);
        }
        #endregion

        #region methods
        private void Close()
        {
            DialogClosing(this, new EventArgs());
        }

        /// <summary>
        /// Dialog window close request method. Inherited from <see cref="IUserDialogViewModel"/>
        /// </summary>
        public void RequestClose()
        {
            Close();
        }

        /// <summary>
        /// Returns current password
        /// </summary>
        /// <returns></returns>
        public string GetPassword()
        {
            return _password;
        }

        private void OnChangePassword(object arg)
        {
            var passwordBox = arg as RadPasswordBox;
            if (passwordBox != null)
            {
                if(_user != null)
                {
                    _user.Password = passwordBox.Password;
                    _user.SavePassword();
                }
                else
                    _password = passwordBox.Password;
            }
        }
        #endregion

        #region properties
        /// <summary>
        /// Dialog window modality flag. Inherited from <see cref="IUserDialogViewModel"/>
        /// </summary>
        public bool IsModal => true;

        /// <summary>
        /// Save state command. Inherited from <see cref="ISaveableViewModel"/>
        /// </summary>
        public ICommand SaveCommand
        {
            get { return _changePasswordCommand; }
            set { PropertySetter(value, newValue => _changePasswordCommand = newValue); }
        }
        #endregion
    }
}
