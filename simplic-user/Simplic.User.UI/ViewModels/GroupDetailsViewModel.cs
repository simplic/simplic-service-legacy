using CommonServiceLocator;
using Simplic.Group;
using Simplic.UI.MVC;
using System;
using System.Linq;
using System.Windows.Input;

namespace Simplic.User.UI
{
    /// <summary>
    /// View model for group details dialog window
    /// </summary>
    class GroupDetailsViewModel : ViewModelBase, IUserDialogViewModel, ISaveableViewModel
    {
        #region fields
        private GroupViewModel _group;
        private string _newGroupName;
        #endregion

        #region ctr
        /// <summary>
        /// Constructor for the view model
        /// </summary>
        /// <param name="group">Current group</param>
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

        /// <summary>
        /// Dialog window close request method. Inherited from <see cref="IUserDialogViewModel"/>
        /// </summary>
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
        /// <summary>
        /// Save state command. Inherited from <see cref="ISaveableViewModel"/>
        /// </summary>
        public ICommand SaveCommand { get { return new RelayCommand(OnSave); } }

        /// <summary>
        /// Dialog window modality flag. Inherited from <see cref="IUserDialogViewModel"/>
        /// </summary>
        public bool IsModal => true;

        /// <summary>
        /// Current group
        /// </summary>
        public GroupViewModel Group
        {
            get { return _group; }
            set { PropertySetter(value, newValue => _group = newValue); }
        }

        /// <summary>
        /// The name of the group, which can be changed through the UI
        /// </summary>
        public string NewGroupName
        {
            get { return _newGroupName; }
            set { PropertySetter(value, newValue => _newGroupName = newValue); }
        }
        #endregion

        #region events
        /// <summary>
        /// Dialog window close request method. Inherited from <see cref="IUserDialogViewModel"/>
        /// </summary>
        public event EventHandler DialogClosing;
        #endregion
    }
}
