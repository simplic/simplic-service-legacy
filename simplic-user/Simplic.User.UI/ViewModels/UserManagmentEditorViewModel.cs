using CommonServiceLocator;
using Simplic.Group;
using Simplic.TenantSystem;
using Simplic.UI.MVC;
using Simplic.User.UI.Utils;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

namespace Simplic.User.UI
{
    /// <summary>
    /// Main view model. Controls list of users, groups and organizations view models
    /// </summary>
    public class UserManagmentEditorViewModel : ViewModelBase, ISaveableViewModel, IOpenSelectedUserDetails
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
        private ICommand _addNewGroup;
        private string _filterString;
        private string _filterString2;
        private ObservableCollection<IDialogViewModel> _dialogs;
        #endregion

        #region ctr
        /// <summary>
        /// Default constructor
        /// </summary>
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
            AddNewGroup = new RelayCommand(OnAddNewGroup);
            Fill();
        }
        #endregion

        #region methods
        /// <summary>
        /// Adds group new user. Opens <see cref=""/> dialog window
        /// </summary>
        /// <param name="arg"></param>
        private void OnAddNewGroup(object arg)
        {
            var viewModel = new CreateNewGroupViewModel();
            Dialogs.Add(viewModel);
            if (viewModel.NewGroup != null)
            {
                var groups = Groups.ToList();
                groups.Add(viewModel.NewGroup);
                Groups.Clear();
                Groups = new ObservableCollection<GroupViewModel>(groups.OrderBy(g => g.Name));
            }
        }

        /// <summary>
        /// Adds new user. Opens <see cref="UserDetailsView"/> dialog window
        /// </summary>
        /// <param name="arg"></param>
        private void OnAddUser(object arg)
        {
            var userDetailsVm = new UserDetailsViewModel(null, true);
            userDetailsVm.DialogClosing += OnAddUserDialogClosing;
            Dialogs.Add(userDetailsVm);
        }

        /// <summary>
        /// Invokes during <see cref="UserDetailsView"/> dialog window closing
        /// </summary>
        /// <param name="sender">Reference to <see cref="UserDetailsViewModel"/> entity</param>
        /// <param name="e"></param>
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

        /// <summary>
        /// Edits selected organization. Opens <see cref="OrganizationDetailsView"/> dialog window
        /// </summary>
        /// <param name="arg">Reference to selected <see cref="OrganizationViewModel"/> entity.</param>
        private void OnEditCurrentOrganization(object arg)
        {
            if (!(arg is OrganizationViewModel org))
                return;
            Dialogs.Add(new OrganizationDetailsViewModel(org));
        }

        /// <summary>
        /// Delets or adds links from organization to user and vice versa
        /// </summary>
        /// <param name="arg">>Reference to selected <see cref="OrganizationViewModel"/> entity.</param>
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

        /// <summary>
        /// Opens <see cref="GroupDetailsView"/> dialog window
        /// </summary>
        /// <param name="arg">Reference to selected <see cref="GroupViewModel"/> entity</param>
        private void OnEditCurrentGroup(object arg)
        {
            if (!(arg is GroupViewModel group))
                return;
            Dialogs.Add(new GroupDetailsViewModel(group));            
        }

        /// <summary>
        /// Deletes current user
        /// </summary>
        /// <param name="arg">Reference to selected <see cref="UserViewModel"/> entity</param>
        private void OnDeleteCurrentUser(object arg)
        {
            var user = arg as UserViewModel;
            if (user == null)
                return;
            user.IsActive = false;
        }

        /// <summary>
        /// Opens <see cref="UserDetailsView"/> dialog window
        /// </summary>
        /// <param name="arg">Reference to selected <see cref="UserViewModel"/> entity</param>
        private void OnEditCurrentUser(object arg)
        {
            if (!(arg is UserViewModel user))
                return;
            Dialogs.Add(new UserDetailsViewModel(user, false));
        }

        /// <summary>
        /// Opens <see cref="UserDetailsView"/> dialog window
        /// </summary>
        /// <param name="arg"></param>
        private void OnOpenSelectedUserDetails(object arg)
        {
            Dialogs.Add(new UserDetailsViewModel(SelectedUser, false));
        }

        /// <summary>
        /// Marks selected user as inactive
        /// </summary>
        /// <param name="arg"></param>
        private void OnDeleteSelectedUser(object arg)
        {
            SelectedUser.IsActive = false;
            SelectedUser = null;
        }

        /// <summary>
        /// Delets or adds links from groups to user and vice versa
        /// </summary>
        /// <param name="arg">>Reference to selected <see cref="GroupViewModel"/> entity.</param>
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

        /// <summary>
        /// Initialization users, groups and organizations collections. Initialization links between these entities
        /// </summary>
        private void Fill()
        {
            Users.Clear();
            FilteredUsers.Clear();
            var users = _userService.GetAllSorted(false)?.ToList();
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

        /// <summary>
        /// Filtes users for groups
        /// </summary>
        private void FilterUsersForGroups()
        {
            if(string.IsNullOrEmpty(FilterString))
            {
                FilteredUsers = new ObservableCollection<UserViewModel>(Users);
                return;
            }
            FilteredUsers = new ObservableCollection<UserViewModel>(Users.Where(u => u.UserName.ToLower().Contains(FilterString.ToLower())));
        }

        /// <summary>
        /// Filtes users for organizations
        /// </summary>
        private void FilterUsersForOrganizations()
        {
            if (string.IsNullOrEmpty(FilterString2))
            {
                FilteredUsers2 = new ObservableCollection<UserViewModel>(Users);
                return;
            }
            FilteredUsers2 = new ObservableCollection<UserViewModel>(Users.Where(u => u.UserName.ToLower().Contains(FilterString2.ToLower())));
        }

        /// <summary>
        /// Save changes
        /// </summary>
        /// <param name="arg"></param>
        private void OnSave(object arg)
        {
            foreach (var user in Users)
                user.SaveAll();
        }
        #endregion

        #region properties
        /// <summary>
        /// Users collection
        /// </summary>
        public ObservableCollection<UserViewModel> Users
        {
            get { return _users; }
            set { PropertySetter(value, newValue => _users = newValue); }
        }

        /// <summary>
        /// Groups collection
        /// </summary>
        public ObservableCollection<GroupViewModel> Groups
        {
            get { return _groups; }
            set { PropertySetter(value, newValue => _groups = newValue); }
        }

        /// <summary>
        /// Delets or adds links from groups to user and vice versa command
        /// </summary>
        public ICommand GroupBindigsChangedCommand
        {
            get { return _groupBindigsChangedCommand; }
            set { PropertySetter(value, newValue => _groupBindigsChangedCommand = newValue); }
        }

        /// <summary>
        /// Adds new group
        /// </summary>
        public ICommand AddNewGroup
        {
            get { return _addNewGroup; }
            set { PropertySetter(value, newValue => _addNewGroup = newValue); }
        }

        /// <summary>
        /// Delete selected user command
        /// </summary>
        public ICommand DeleteSelectedUserCommand
        {
            get { return _deleteSelectedUserCommand; }
            set { PropertySetter(value, newValue => _deleteSelectedUserCommand = newValue); }
        }

        /// <summary>
        /// Selected user
        /// </summary>
        public UserViewModel SelectedUser
        {
            get { return _selectedUser; }
            set { PropertySetter(value, newValue => _selectedUser = newValue); }
        }

        /// <summary>
        /// Dialog view models collection opened by this entity
        /// </summary>
        public ObservableCollection<IDialogViewModel> Dialogs
        {
            get { return _dialogs; }
            set { PropertySetter(value, newValue => _dialogs = newValue); }
        }

        /// <summary>
        /// Open selected user details dialog window command
        /// </summary>
        public ICommand OpenSelectedUserDetailsCommand
        {
            get { return _openSelectedUserDetailsCommand; }
            set { PropertySetter(value, newValue => _openSelectedUserDetailsCommand = newValue); }
        }

        /// <summary>
        /// Edit current user command
        /// </summary>
        public ICommand EditCurrentUserCommand
        {
            get { return _editCurrentUserCommand; }
            set { PropertySetter(value, newValue => _editCurrentUserCommand = newValue); }
        }

        /// <summary>
        /// Delete current user command
        /// </summary>
        public ICommand DeleteCurrentUserCommand
        {
            get { return _deleteCurrentUserCommand; }
            set { PropertySetter(value, newValue => _deleteCurrentUserCommand = newValue); }
        }

        /// <summary>
        /// Filtered users collection for groups
        /// </summary>
        public ObservableCollection<UserViewModel> FilteredUsers
        {
            get { return _filteredUsers; }
            set { PropertySetter(value, newValue => _filteredUsers = newValue); }
        }

        /// <summary>
        /// Groups's filter string
        /// </summary>
        public string FilterString
        {
            get { return _filterString; }
            set
            {
                PropertySetter(value, newValue => _filterString = newValue);
                FilterUsersForGroups();
            }
        }

        /// <summary>
        /// Edit current group command
        /// </summary>
        public ICommand EditCurrentGroupCommand
        {
            get { return _editCurrentGroupCommand; }
            set { PropertySetter(value, newValue => _editCurrentGroupCommand = newValue); }
        }

        /// <summary>
        /// Organizations collection
        /// </summary>
        public ObservableCollection<OrganizationViewModel> Organizations
        {
            get { return _organizations; }
            set { PropertySetter(value, newValue => _organizations = newValue); }
        }

        /// <summary>
        /// Delets or adds links from organizations to user and vice versa command
        /// </summary>
        public ICommand OrganizationBindigsChangedCommand
        {
            get { return _organizationBindigsChangedCommand; }
            set { PropertySetter(value, newValue => _organizationBindigsChangedCommand = newValue); }
        }

        /// <summary>
        /// Edit current organization command
        /// </summary>
        public ICommand EditCurrentOrganizationCommand
        {
            get { return _editCurrentOrganizationCommand; }
            set { PropertySetter(value, newValue => _editCurrentOrganizationCommand = newValue); }
        }

        /// <summary>
        /// Organization's filter string
        /// </summary>
        public string FilterString2
        {
            get { return _filterString2; }
            set
            {
                PropertySetter(value, newValue => _filterString2 = newValue);
                FilterUsersForOrganizations();
            }
        }

        /// <summary>
        /// Filtered users for organizations
        /// </summary>
        public ObservableCollection<UserViewModel> FilteredUsers2
        {
            get { return _filteredUsers2; }
            set { PropertySetter(value, newValue => _filteredUsers2 = newValue); }
        }

        /// <summary>
        /// Skip selected user command
        /// </summary>
        public ICommand SkipSelectedUserCommand
        {
            get { return _skipSelectedUserCommand; }
            set { PropertySetter(value, newValue => _skipSelectedUserCommand = newValue); }
        }

        /// <summary>
        /// Add new user command
        /// </summary>
        public ICommand AddUserCommand
        {
            get { return _addUserCommand; }
            set { PropertySetter(value, newValue => _addUserCommand = newValue); }
        }

        /// <summary>
        /// Save state command. Inherited from <see cref="ISaveableViewModel"/>
        /// </summary>
        public ICommand SaveCommand { get { return new RelayCommand(OnSave); } }
        #endregion
    }
}
