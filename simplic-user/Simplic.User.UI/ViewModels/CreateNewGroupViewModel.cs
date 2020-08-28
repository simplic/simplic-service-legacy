using CommonServiceLocator;
using Simplic.Group;
using Simplic.Localization;
using Simplic.UI.MVC;
using System;
using System.Windows;
using System.Windows.Input;

namespace Simplic.User.UI
{
    class CreateNewGroupViewModel : ViewModelBase, ISaveableViewModel, IUserDialogViewModel
    {
        #region fields
        private string _groupName;
        private bool _isDefault;
        private GroupViewModel _newGroup;
        #endregion

        #region events
        /// <summary>
        /// Dialog window close request method. Inherited from <see cref="IUserDialogViewModel"/>
        /// </summary>
        public event EventHandler DialogClosing;
        #endregion


        #region commands
        public ICommand SaveCommand => new RelayCommand(OnSave);
        #endregion

        #region methods
        private void OnSave(object arg)
        {
            if (string.IsNullOrWhiteSpace(GroupName))
            {
                var localizationService = ServiceLocator.Current.GetInstance<ILocalizationService>();
                var str = localizationService.Translate("usermanagment_empty_group_name_warning_label");
                MessageBox.Show(str);
                return;
            }
            var groupService = ServiceLocator.Current.GetInstance<IGroupService>();
            var newGroup = new Group.Group()
            {
                GroupId = 0,
                Ident = 0,
                IsDefaultGroup = IsDefault,
                Name = GroupName
            };
            groupService.Save(newGroup);
            NewGroup = new GroupViewModel(newGroup);
        }

        /// <summary>
        /// Closes dialog window
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
        #endregion

        #region properties
        /// <summary>
        /// New group's name
        /// </summary>
        public string GroupName
        {
            get { return _groupName; }
            set { PropertySetter(value, newValue => _groupName = newValue); }
        }

        /// <summary>
        /// Is default group flag
        /// </summary>
        public bool IsDefault
        {
            get { return _isDefault; }
            set { PropertySetter(value, newValue => _isDefault = newValue); }
        }

        /// <summary>
        /// Reference to newly created group
        /// </summary>
        public GroupViewModel NewGroup
        {
            get { return _newGroup; }
            set { PropertySetter(value, newValue => _newGroup = newValue); }
        }

        public bool IsModal => true;
        #endregion

    }
}
