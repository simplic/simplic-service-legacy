using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Simplic.UI.MVC;

namespace Simplic.User.UI
{
    /// <summary>
    /// View model for user details dialog window
    /// </summary>
    class UserDetailsViewModel : ViewModelBase, IUserDialogViewModel, ISaveableViewModel, IDeletableViewModel
    {
        #region fields
        private string _userName;
        private string _firstName;
        private string _lastName;
        private string _email;
        private bool _isActive;
        private string _phone;
        private bool _isADUser;
        private UserViewModel _user;
        private ObservableCollection<IDialogViewModel> _dialogs;
        private ICommand _changePasswordCommand;
        private bool _isCreate;
        private string _password;
        #endregion

        #region ctr
        /// <summary>
        /// Constructor for the view model
        /// </summary>
        /// <param name="user">Current user</param>
        /// <param name="isCreate">Is to create or edit user entity</param>
        public UserDetailsViewModel(UserViewModel user, bool isCreate)
        {
            if (!isCreate)
            {
                User = user;
                UserName = User.UserName;
                FirstName = User.FirstName;
                LastName = User.LastName;
                Email = User.Email;
                Phone = User.Phone;
                IsActive = User.IsActive;
            }
            else
                User = null;
            
            Dialogs = new ObservableCollection<IDialogViewModel>();
            ChangePasswordCommand = new RelayCommand(OnChangePassword);
            IsCreate = isCreate;
        }
        #endregion

        #region methods
        /// <summary>
        /// Creates <see cref="ChangeUserPasswordViewModel"/> entity and opens password dialog window
        /// </summary>
        /// <param name="arg"></param>
        private void OnChangePassword(object arg)
        {
            var changePwdDialog = new ChangeUserPasswordViewModel(User);
            changePwdDialog.DialogClosing += OnChangePasswordDialogDialogClosing;
            Dialogs.Add(changePwdDialog);
        }

        /// <summary>
        /// Invokes during closing password dialog window
        /// </summary>
        /// <param name="sender">Referrence to current <see cref="ChangeUserPasswordViewModel"/> entity</param>
        /// <param name="e">Event arguments</param>
        private void OnChangePasswordDialogDialogClosing(object sender, EventArgs e)
        {
            var changePwdVm = sender as ChangeUserPasswordViewModel;
            changePwdVm.DialogClosing -= OnChangePasswordDialogDialogClosing;
            if (IsCreate)
                _password = changePwdVm.GetPassword();
        }

        /// <summary>
        /// Close user detail dialog window
        /// </summary>
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
        /// Marks user entity as inactive
        /// </summary>
        /// <param name="arg"></param>
        private void OnDelete(object arg)
        {
            IsActive = false;
        }

        /// <summary>
        /// Save user details changes
        /// </summary>
        /// <param name="arg"></param>
        private void OnSave(object arg)
        {
            if (IsCreate)
                User = new UserViewModel
                {
                    UserName = UserName,
                    IsADUser = this.IsADUser,
                    Password = _password
                };
            User.FirstName = FirstName;
            User.LastName = LastName;
            User.Email = Email;
            User.Phone = Phone;
            User.IsActive = IsActive;
            User.SaveUser();
            IsCreate = false;
        }
        #endregion

        #region events
        /// <summary>
        /// Dialog window close request method. Inherited from <see cref="IUserDialogViewModel"/>
        /// </summary>
        public event EventHandler DialogClosing;
        #endregion

        #region properties
        /// <summary>
        /// Save state command. Inherited from <see cref="ISaveableViewModel"/>
        /// </summary>
        public ICommand SaveCommand { get { return new RelayCommand(OnSave); } }

        /// <summary>
        /// Current user
        /// </summary>
        public UserViewModel User
        {
            get { return _user; }
            set { PropertySetter(value, newValue => _user = newValue); }
        }

        /// <summary>
        /// User name
        /// </summary>
        public string UserName
        {
            get { return _userName; }
            set { PropertySetter(value, newValue => _userName = newValue); }
        }

        /// <summary>
        /// User first name
        /// </summary>
        public string FirstName
        {
            get { return _firstName; }
            set { PropertySetter(value, newValue => _firstName = newValue); }
        }

        /// <summary>
        /// User last name
        /// </summary>
        public string LastName
        {
            get { return _lastName; }
            set { PropertySetter(value, newValue => _lastName = newValue); }
        }

        /// <summary>
        /// User email
        /// </summary>
        public string Email
        {
            get { return _email; }
            set { PropertySetter(value, newValue => _email = newValue); }
        }

        /// <summary>
        /// User active flag
        /// </summary>
        public bool IsActive
        {
            get { return _isActive; }
            set { PropertySetter(value, newValue => _isActive = newValue); }
        }

        /// <summary>
        /// User phone
        /// </summary>
        public string Phone
        {
            get { return _phone; }
            set { PropertySetter(value, newValue => _phone = newValue); }
        }

        /// <summary>
        /// Dialog window modality flag. Inherited from <see cref="IUserDialogViewModel"/>
        /// </summary>
        public bool IsModal => true;

        /// <summary>
        /// Dialog view models collection opened by this entity
        /// </summary>
        public ObservableCollection<IDialogViewModel> Dialogs
        {
            get { return _dialogs; }
            set { PropertySetter(value, newValue => _dialogs = newValue); }
        }

        /// <summary>
        /// Change user password command
        /// </summary>
        public ICommand ChangePasswordCommand
        {
            get { return _changePasswordCommand; }
            set { PropertySetter(value, newValue => _changePasswordCommand = newValue); }
        }

        /// <summary>
        /// Create or edit user entity flag
        /// </summary>
        public bool IsCreate
        {
            get { return _isCreate; }
            set { PropertySetter(value, newValue => _isCreate = newValue); }
        }

        /// <summary>
        /// User AD flag
        /// </summary>
        public bool IsADUser
        {
            get { return _isADUser; }
            set { PropertySetter(value, newValue => _isADUser = newValue); }
        }

        /// <summary>
        /// Delete user entity command. Inherited from <see cref="IDeletableViewModel"/>
        /// </summary>
        public ICommand DeleteCommand { get { return new RelayCommand(OnDelete); } }
        #endregion
    }
}
