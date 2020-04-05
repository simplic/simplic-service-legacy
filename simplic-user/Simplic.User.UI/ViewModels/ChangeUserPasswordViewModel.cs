using Simplic.UI.MVC;
using System;
using System.Windows.Input;
using Telerik.Windows.Controls;

namespace Simplic.User.UI
{
    class ChangeUserPasswordViewModel : Simplic.UI.MVC.ViewModelBase, IUserDialogViewModel
    {
        #region fields
        private readonly UserViewModel _user;
        private ICommand _changePasswordCommand;
        #endregion

        #region events
        public event EventHandler DialogClosing;
        #endregion

        #region ctr
        public ChangeUserPasswordViewModel(UserViewModel user)
        {
            _user = user;
            ChangePasswordCommand = new RelayCommand(OnChangePassword);
        }
        #endregion

        #region methods
        private void Close()
        {
            DialogClosing(this, new EventArgs());
        }

        public void RequestClose()
        {
            Close();
        }

        private void OnChangePassword(object arg)
        {
            var passwordBox = arg as RadPasswordBox;
            if (passwordBox != null)
                _user.Password = passwordBox.Password;
            Close();
        }

        private void OnCancel(object arg)
        {
            Close();
        }
        #endregion

        #region properties
        public bool IsModal => true;

        public ICommand CancelCommand { get { return new RelayCommand(OnCancel); } }

        public ICommand ChangePasswordCommand
        {
            get { return _changePasswordCommand; }
            set { PropertySetter(value, newValue => _changePasswordCommand = newValue); }
        }
        #endregion
    }
}
