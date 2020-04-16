using Simplic.UI.MVC;
using System.Collections.ObjectModel;
using System.Linq;

namespace Simplic.User.UI
{
    public class UserViewModel : ViewModelBase, IDraggableUser
    {
        #region fields
        private int _useId;
        private string _userName;
        private string _password;
        private string _firstName;
        private string _lastName;
        private string _email;
        private bool _isActive;
        private bool _isADUser;
        private string _phone;
        private ObservableCollection<GroupViewModel> _groups;
        private ObservableCollection<OrganizationViewModel> _organizations;
        #endregion

        #region ctr
        public UserViewModel()
        {
            Groups = new ObservableCollection<GroupViewModel>();
            Organizations = new ObservableCollection<OrganizationViewModel>();
        }

        public UserViewModel(int id, string userName, string password, string firstName, string lastName, string email,
            bool isActive, string phone, bool isADUser) : this()
        {
            UserId = id;
            UserName = userName;
            Password = password;
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            IsActive = isActive;
            Phone = phone;
            IsADUser = isADUser;
        }

        public UserViewModel(User user) : this(user.Ident, user.UserName, user.Password, user.FirstName, user.LastName,
            user.EMail, user.IsActive, string.Empty, user.IsADUser)
        {
        }
        #endregion

        #region methods
        public void Join(ViewModelBase vm)
        {
            if (vm is GroupViewModel groupVm)
            {
                if (groupVm.Users.FirstOrDefault(u => u.UserId == UserId) != null)
                    return;
                groupVm.Users.Add(this);
                Groups.Add(groupVm);
                RaisePropertyChanged("Groups");
            }
            else if(vm is OrganizationViewModel orgVm)
            {
                if (orgVm.Users.FirstOrDefault(u => u.UserId == UserId) != null)
                    return;
                orgVm.Users.Add(this);
                Organizations.Add(orgVm);
                RaisePropertyChanged("Organizations");
            }
        }
        #endregion

        #region properties
        public int UserId
        {
            get { return _useId; }
            set { PropertySetter(value, newValue => _useId = newValue); }
        }

        public string UserName
        {
            get { return _userName; }
            set { PropertySetter(value, newValue => _userName = newValue); }
        }

        public string Password
        {
            get { return _password; }
            set { PropertySetter(value, newValue => _password = newValue); }
        }

        public string FirstName
        {
            get { return _firstName; }
            set { PropertySetter(value, newValue => _firstName = newValue); }
        }

        public string LastName
        {
            get { return _lastName; }
            set { PropertySetter(value, newValue => _lastName = newValue); }
        }

        public string Email
        {
            get { return _email; }
            set { PropertySetter(value, newValue => _email = newValue); }
        }

        public bool IsActive
        {
            get { return _isActive; }
            set { PropertySetter(value, newValue => _isActive = newValue); }
        }

        public string Phone
        {
            get { return _phone; }
            set { PropertySetter(value, newValue => _phone = newValue); }
        }

        public ObservableCollection<GroupViewModel> Groups
        {
            get { return _groups; }
            set { PropertySetter(value, newValue => _groups = newValue); }
        }

        public ObservableCollection<OrganizationViewModel> Organizations
        {
            get { return _organizations; }
            set { PropertySetter(value, newValue => _organizations = newValue); }
        }

        public bool IsADUser
        {
            get { return _isADUser; }
            set { PropertySetter(value, newValue => _isADUser = newValue); }
        }
        #endregion
    }
}
