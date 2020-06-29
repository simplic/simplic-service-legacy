using Simplic.UI.MVC;
using System.Collections.ObjectModel;

namespace Simplic.User.UI
{
    /// <summary>
    /// View model for the group entity
    /// </summary>
    public  class GroupViewModel : ViewModelBase, INamedEntity
    {
        #region fields
        private int _groupId;
        private string _groupName;
        private bool _isDefault;
        private int _ident;
        private ObservableCollection<UserViewModel> _users;
        #endregion

        #region ctr
        /// <summary>
        /// Default constructor for the view model
        /// </summary>
        public GroupViewModel()
        {
            Users = new ObservableCollection<UserViewModel>();
        }

        /// <summary>
        /// Constructor for the view model
        /// </summary>
        /// <param name="groupId">Group ID</param>
        /// <param name="ident">Group ident</param>
        /// <param name="groupName">Group name</param>
        /// <param name="isDefault">Group's default flag</param>
        public GroupViewModel(int groupId, int ident, string groupName, bool isDefault) : this()
        {
            GroupId = groupId;
            Ident = ident;
            Name = groupName;
            IsDefault = IsDefault;
        }

        /// <summary>
        /// Copy constructor
        /// </summary>
        /// <param name="group"></param>
        public GroupViewModel(Group.Group group) : this(group.GroupId, group.Ident, group.Name, group.IsDefaultGroup)
        {
        }
        #endregion

        #region properties
        /// <summary>
        /// Group ID
        /// </summary>
        public int GroupId
        {
            get { return _groupId; }
            set { PropertySetter(value, newValue => _groupId = newValue); }
        }

        /// <summary>
        /// Group name
        /// </summary>
        public string Name
        {
            get { return _groupName; }
            set { PropertySetter(value, newValue => _groupName = newValue); }
        }

        /// <summary>
        /// Group's default flag
        /// </summary>
        public bool IsDefault
        {
            get { return _isDefault; }
            set { PropertySetter(value, newValue => _isDefault = newValue); }
        }

        /// <summary>
        /// Group's users collection
        /// </summary>
        public ObservableCollection<UserViewModel> Users
        {
            get { return _users; }
            set { PropertySetter(value, newValue => _users = newValue); }
        }

        /// <summary>
        /// Group ident
        /// </summary>
        public int Ident
        {
            get { return _ident; }
            set { PropertySetter(value, newValue => _ident = newValue); }
        }
        #endregion
    }
}
