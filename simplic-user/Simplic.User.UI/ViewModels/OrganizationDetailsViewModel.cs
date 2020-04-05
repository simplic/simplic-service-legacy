using Simplic.UI.MVC;
using System;
using System.Linq;
using System.Windows.Input;

namespace Simplic.User.UI
{
    class OrganizationDetailsViewModel : ViewModelBase, IUserDialogViewModel
    {
        #region fields
        private OrganizationViewModel _organization;
        private string _newOrganizationName;
        #endregion

        #region ctr
        public OrganizationDetailsViewModel(OrganizationViewModel organization)
        {
            Organization = organization;
            NewOrganizationName = Organization.Name;
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
            Organization.Name = NewOrganizationName;
            Organization.Users.ToList().ForEach(u => u.RaisePropertyChanged("Organizations"));
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

        public OrganizationViewModel Organization
        {
            get { return _organization; }
            set { PropertySetter(value, newValue => _organization = newValue); }
        }

        public string NewOrganizationName
        {
            get { return _newOrganizationName; }
            set { PropertySetter(value, newValue => _newOrganizationName = newValue); }
        }
        #endregion

        #region events
        public event EventHandler DialogClosing;
        #endregion
    }
}
