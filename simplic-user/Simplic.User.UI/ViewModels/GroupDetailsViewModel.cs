using Simplic.UI.MVC;
using System;
using System.Linq;
using System.Windows.Input;

namespace Simplic.User.UI
{
    class GroupDetailsViewModel : ViewModelBase, IUserDialogViewModel
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

        private void OnOk(object arg)
        {
            Group.Name = NewGroupName;
            Group.Users.ToList().ForEach(u => u.RaisePropertyChanged("Groups"));
            Close();
        }

        private void OnCancel(object arg)
        {
            Close();
        }
        #endregion

        #region properties
        public ICommand OkCommand { get { return new RelayCommand(OnOk); } }

        public ICommand CancelCommand { get { return new RelayCommand(OnCancel); } }

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
