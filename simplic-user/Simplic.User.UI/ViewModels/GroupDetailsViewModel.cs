using CommonServiceLocator;
using Simplic.Group;
using Simplic.UI.MVC;
using System;
using System.Linq;
using System.Windows.Input;

namespace Simplic.User.UI
{
    class GroupDetailsViewModel : ViewModelBase, IUserDialogViewModel, ISaveableViewModel
    {
        #region fields
        private GroupViewModel _group;
        private string _newGroupName;
        #endregion

        #region ctr
        public GroupDetailsViewModel(GroupViewModel group)
        {
            Group = group;
            NewGroupName = Group.Name;
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

        private void OnSave(object arg)
        {
            Group.Name = NewGroupName;
            Group.Users.ToList().ForEach(u => u.RaisePropertyChanged("Groups"));

            var groupService = ServiceLocator.Current.GetInstance<IGroupService>();
            groupService.Save(new Group.Group()
            {
                GroupId = Group.GroupId,
                Ident = Group.Ident,
                IsDefaultGroup = Group.IsDefault,
                Name = Group.Name
            });
        }
        #endregion

        #region properties
        public ICommand SaveCommand { get { return new RelayCommand(OnSave); } }

        public bool IsModal => true;

        public GroupViewModel Group
        {
            get { return _group; }
            set { PropertySetter(value, newValue => _group = newValue); }
        }

        public string NewGroupName
        {
            get { return _newGroupName; }
            set { PropertySetter(value, newValue => _newGroupName = newValue); }
        }
        #endregion

        #region events
        public event EventHandler DialogClosing;
        #endregion
    }
}
