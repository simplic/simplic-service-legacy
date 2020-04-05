using Simplic.UI.MVC;
using System.Collections.ObjectModel;

namespace Simplic.User.UI
{
    public  class GroupViewModel : ViewModelBase, INamedEntity
    {
        #region fields
        private int _groupId;
        private string _groupName;
        private bool _isDefault;
        private ObservableCollection<UserViewModel> _users;
        #endregion

        #region ctr
        public GroupViewModel()
        {
            Users = new ObservableCollection<UserViewModel>();
        }

        public GroupViewModel(int groupId, string groupName, bool isDefault) : this()
        {
            GroupId = groupId;
            Name = groupName;
            IsDefault = IsDefault;
        }

        public GroupViewModel(Group.Group group) : this(group.GroupId, group.Name, group.IsDefaultGroup)
        {
        }
        #endregion

        #region properties
        public int GroupId
        {
            get { return _groupId; }
            set { PropertySetter(value, newValue => _groupId = newValue); }
        }

        public string Name
        {
            get { return _groupName; }
            set { PropertySetter(value, newValue => _groupName = newValue); }
        }

        public bool IsDefault
        {
            get { return _isDefault; }
            set { PropertySetter(value, newValue => _isDefault = newValue); }
        }

        public ObservableCollection<UserViewModel> Users
        {
            get { return _users; }
            set { PropertySetter(value, newValue => _users = newValue); }
        }
        #endregion
    }
}
