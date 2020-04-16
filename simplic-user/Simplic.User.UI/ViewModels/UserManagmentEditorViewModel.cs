using CommonServiceLocator;
using Simplic.BaseDAL;
using Simplic.Group;
using Simplic.TenantSystem;
using Simplic.UI.MVC;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

namespace Simplic.User.UI
{
    public class UserManagmentEditorViewModel : ViewModelBase, ISaveableViewModel
    {
        #region fields
        private readonly IUserService _userService;
        private readonly IGroupService _groupService;
        private readonly IOrganizationService _organizationService;
        private ObservableCollection<UserViewModel> _users;
        private ObservableCollection<UserViewModel> _filteredUsers;
        private ObservableCollection<UserViewModel> _filteredUsers2;
        private ObservableCollection<OrganizationViewModel> _organizations;

        private UserViewModel _selectedUser;
        private ObservableCollection<GroupViewModel> _groups;
        private ICommand _groupBindigsChangedCommand;
        private ICommand _organizationBindigsChangedCommand;
        private ICommand _deleteSelectedUserCommand;
        private ICommand _openSelectedUserDetailsCommand;
        private ICommand _editCurrentUserCommand;
        private ICommand _editCurrentGroupCommand;
        private ICommand _editCurrentOrganizationCommand;
        private ICommand _deleteCurrentUserCommand;
        private ICommand _skipSelectedUserCommand;
        private ICommand _addUserCommand;
        private string _filterString;
        private string _filterString2;
        private ObservableCollection<IDialogViewModel> _dialogs;
        #endregion

        #region ctr
        public UserManagmentEditorViewModel()
        {
            _userService = ServiceLocator.Current.GetInstance<IUserService>();
            _groupService = ServiceLocator.Current.GetInstance<IGroupService>();
            _organizationService = ServiceLocator.Current.GetInstance<IOrganizationService>();
            GroupBindigsChangedCommand = new RelayCommand(OnGroupBindigsChanged);
            OrganizationBindigsChangedCommand = new RelayCommand(OnOrganizationBindigsChanged);
            DeleteSelectedUserCommand = new RelayCommand(OnDeleteSelectedUser);
            OpenSelectedUserDetailsCommand = new RelayCommand(OnOpenSelectedUserDetails);
            DeleteCurrentUserCommand = new RelayCommand(OnDeleteCurrentUser);
            EditCurrentGroupCommand = new RelayCommand(OnEditCurrentGroup);
            EditCurrentOrganizationCommand = new RelayCommand(OnEditCurrentOrganization);
            SkipSelectedUserCommand = new RelayCommand((o) => SelectedUser = null);
            AddUserCommand = new RelayCommand(OnAddUser);
            Users = new ObservableCollection<UserViewModel>();
            FilteredUsers = new ObservableCollection<UserViewModel>();
            Groups = new ObservableCollection<GroupViewModel>();
            Organizations = new ObservableCollection<OrganizationViewModel>();
            Dialogs = new ObservableCollection<IDialogViewModel>();
            EditCurrentUserCommand = new RelayCommand(OnEditCurrentUser);
            Fill();
        }
        #endregion

        #region methods
        private void OnAddUser(object arg)
        {
            var userDetailsVm = new UserDetailsViewModel(null, true);
            userDetailsVm.DialogClosing += OnAddUserDialogClosing;
            Dialogs.Add(userDetailsVm);
        }

        private void OnAddUserDialogClosing(object sender, System.EventArgs e)
        {
            var userDetailsVm = sender as UserDetailsViewModel;
            if (userDetailsVm.User != null)
            {
                Users.Add(userDetailsVm.User);
                Users = new ObservableCollection<UserViewModel>(Users.OrderBy(a => a.UserName));
            }
            userDetailsVm.DialogClosing -= OnAddUserDialogClosing;
        }

        private void OnEditCurrentOrganization(object arg)
        {
            if (!(arg is OrganizationViewModel org))
                return;
            Dialogs.Add(new OrganizationDetailsViewModel(org));
        }

        private void OnOrganizationBindigsChanged(object arg)
        {
            if (SelectedUser == null || !(arg is OrganizationViewModel org))
                return;
            if (SelectedUser.Organizations.Contains(org))
            {
                SelectedUser.Organizations.Remove(org);
                org.Users.Remove(SelectedUser);
            }
            else
            {
                SelectedUser.Organizations.Add(org);
                org.Users.Add(SelectedUser);
            }
        }

        private void OnEditCurrentGroup(object arg)
        {
            if (!(arg is GroupViewModel group))
                return;
            Dialogs.Add(new GroupDetailsViewModel(group));            
        }

        private void OnDeleteCurrentUser(object arg)
        {
            var user = arg as UserViewModel;
            if (user == null)
                return;
            user.IsActive = false;
        }

        private void OnEditCurrentUser(object arg)
        {
            if (!(arg is UserViewModel user))
                return;
            Dialogs.Add(new UserDetailsViewModel(user, false));
        }

        private void OnOpenSelectedUserDetails(object arg)
        {
            Dialogs.Add(new UserDetailsViewModel(SelectedUser, false));
        }

        private void OnDeleteSelectedUser(object arg)
        {
            SelectedUser.IsActive = false;
            SelectedUser = null;
        }

        private void OnGroupBindigsChanged(object arg)
        {
            if (SelectedUser == null || !(arg is GroupViewModel group))
                return;
            if(SelectedUser.Groups.Contains(group))
            {
                SelectedUser.Groups.Remove(group);
                group.Users.Remove(SelectedUser);
            }
            else
            {
                SelectedUser.Groups.Add(group);
                group.Users.Add(SelectedUser);
            }
        }

        private void Fill()
        {
            Users.Clear();
            FilteredUsers.Clear();
            var users = _userService.GetAllSorted()?.ToList();
            if (users == null || !users.Any())
                return;
            users.Sort((u1, u2) => u1.UserName.CompareTo(u2.UserName));
            users.ForEach(u => Users.Add(new UserViewModel(u)));
            FilteredUsers = new ObservableCollection<UserViewModel>(Users);
            FilteredUsers2 = new ObservableCollection<UserViewModel>(Users);

            Groups.Clear();
            var groups = _groupService.GetAllSortedByName()?.ToList();
            if (groups == null || !groups.Any())
                return;
            groups.ForEach(g => Groups.Add(new GroupViewModel(g)));
            foreach (var u in Users)
            {
                var gs = _groupService.GetAllByUserId(u.UserId)?.ToList();
                if (gs == null || !gs.Any())
                    continue;
                gs.ForEach(g =>
                {
                    var groupViewModel = Groups.FirstOrDefault(gVm => gVm.GroupId == g.GroupId);
                    if (groupViewModel == null)
                        return;
                    u.Groups.Add(groupViewModel);
                    groupViewModel.Users.Add(u);
                });
            }

            Organizations.Clear();
            var organizations = _organizationService.GetAll()?.ToList();
            if (organizations == null || !organizations.Any())
                return;
            organizations.ForEach(o => Organizations.Add(new OrganizationViewModel(o)));
            foreach(var u in Users)
            {
                var orgs = _organizationService.GetByUserId(u.UserId)?.ToList();
                if (orgs == null || !orgs.Any())
                    continue;
                orgs.ForEach(o =>
                {
                    var orgViewModel = Organizations.FirstOrDefault(oVm => oVm.OrganizationId == o.Id);
                    if (orgViewModel == null)
                        return;
                    u.Organizations.Add(orgViewModel);
                    orgViewModel.Users.Add(u);
                });
            }
        }

        private void FilterUsersForGroups()
        {
            if(string.IsNullOrEmpty(FilterString))
            {
                FilteredUsers = new ObservableCollection<UserViewModel>(Users);
                return;
            }
            FilteredUsers = new ObservableCollection<UserViewModel>(Users.Where(u => u.UserName.ToLower().Contains(FilterString.ToLower())));
        }

        private void FilterUsersForOrganizations()
        {
            if (string.IsNullOrEmpty(FilterString2))
            {
                FilteredUsers2 = new ObservableCollection<UserViewModel>(Users);
                return;
            }
            FilteredUsers2 = new ObservableCollection<UserViewModel>(Users.Where(u => u.UserName.ToLower().Contains(FilterString2.ToLower())));
        }

        private void OnSave(object arg)
        {
            foreach (var user in Users)
                user.SaveAll();
        }
        #endregion

        #region properties
        public ObservableCollection<UserViewModel> Users
        {
            get { return _users; }
            set { PropertySetter(value, newValue => _users = newValue); }
        }

        public ObservableCollection<GroupViewModel> Groups
        {
            get { return _groups; }
            set { PropertySetter(value, newValue => _groups = newValue); }
        }

        public ICommand GroupBindigsChangedCommand
        {
            get { return _groupBindigsChangedCommand; }
            set { PropertySetter(value, newValue => _groupBindigsChangedCommand = newValue); }
        }

        public ICommand DeleteSelectedUserCommand
        {
            get { return _deleteSelectedUserCommand; }
            set { PropertySetter(value, newValue => _deleteSelectedUserCommand = newValue); }
        }

        public UserViewModel SelectedUser
        {
            get { return _selectedUser; }
            set { PropertySetter(value, newValue => _selectedUser = newValue); }
        }

        public ObservableCollection<IDialogViewModel> Dialogs
        {
            get { return _dialogs; }
            set { PropertySetter(value, newValue => _dialogs = newValue); }
        }

        public ICommand OpenSelectedUserDetailsCommand
        {
            get { return _openSelectedUserDetailsCommand; }
            set { PropertySetter(value, newValue => _openSelectedUserDetailsCommand = newValue); }
        }

        public ICommand EditCurrentUserCommand
        {
            get { return _editCurrentUserCommand; }
            set { PropertySetter(value, newValue => _editCurrentUserCommand = newValue); }
        }

        public ICommand DeleteCurrentUserCommand
        {
            get { return _deleteCurrentUserCommand; }
            set { PropertySetter(value, newValue => _deleteCurrentUserCommand = newValue); }
        }

        public ObservableCollection<UserViewModel> FilteredUsers
        {
            get { return _filteredUsers; }
            set { PropertySetter(value, newValue => _filteredUsers = newValue); }
        }

        public string FilterString
        {
            get { return _filterString; }
            set
            {
                PropertySetter(value, newValue => _filterString = newValue);
                FilterUsersForGroups();
            }
        }

        public ICommand EditCurrentGroupCommand
        {
            get { return _editCurrentGroupCommand; }
            set { PropertySetter(value, newValue => _editCurrentGroupCommand = newValue); }
        }

        public ObservableCollection<OrganizationViewModel> Organizations
        {
            get { return _organizations; }
            set { PropertySetter(value, newValue => _organizations = newValue); }
        }

        public ICommand OrganizationBindigsChangedCommand
        {
            get { return _organizationBindigsChangedCommand; }
            set { PropertySetter(value, newValue => _organizationBindigsChangedCommand = newValue); }
        }

        public ICommand EditCurrentOrganizationCommand
        {
            get { return _editCurrentOrganizationCommand; }
            set { PropertySetter(value, newValue => _editCurrentOrganizationCommand = newValue); }
        }

        public string FilterString2
        {
            get { return _filterString2; }
            set
            {
                PropertySetter(value, newValue => _filterString2 = newValue);
                FilterUsersForOrganizations();
            }
        }

        public ObservableCollection<UserViewModel> FilteredUsers2
        {
            get { return _filteredUsers2; }
            set { PropertySetter(value, newValue => _filteredUsers2 = newValue); }
        }

        public ICommand SkipSelectedUserCommand
        {
            get { return _skipSelectedUserCommand; }
            set { PropertySetter(value, newValue => _skipSelectedUserCommand = newValue); }
        }

        public ICommand AddUserCommand
        {
            get { return _addUserCommand; }
            set { PropertySetter(value, newValue => _addUserCommand = newValue); }
        }

        public ICommand SaveCommand { get { return new RelayCommand(OnSave); } }
        #endregion
    }
}
