using Simplic.UI.MVC;
using System;
using System.Windows.Input;
using Telerik.Windows.Controls;

namespace Simplic.User.UI
{
    class ChangeUserPasswordViewModel : Simplic.UI.MVC.ViewModelBase, IUserDialogViewModel, ISaveableViewModel
    {
        #region fields
        private readonly UserViewModel _user;
        private ICommand _changePasswordCommand;
        private string _password;
        #endregion

        #region events
        public event EventHandler DialogClosing;
        #endregion

        #region ctr
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

        public void RequestClose()
        {
            Close();
        }

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
        public bool IsModal => true;

        public ICommand SaveCommand
        {
            get { return _changePasswordCommand; }
            set { PropertySetter(value, newValue => _changePasswordCommand = newValue); }
        }
        #endregion
    }
}
