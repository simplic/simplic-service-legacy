using CommonServiceLocator;
using Simplic.TenantSystem;
using Simplic.UI.MVC;
using System;
using System.Linq;
using System.Windows.Input;

namespace Simplic.User.UI
{
    class OrganizationDetailsViewModel : ViewModelBase, IUserDialogViewModel, ISaveableViewModel
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

        private void OnSave(object arg)
        {
            Organization.Name = NewOrganizationName;
            Organization.Users.ToList().ForEach(u => u.RaisePropertyChanged("Organizations"));

            var organizationService = ServiceLocator.Current.GetInstance<IOrganizationService>();
            organizationService.Save(new Organization
            {
                Id = Organization.OrganizationId,
                CloudOrganizationId = Organization.CloudOrganizationId,
                IsActive = Organization.IsActive,
                MatchCode = Organization.MatchCode,
                Name = Organization.Name,
                SubOrganizations = Organization.SubOrganizations
            });
        }
        #endregion

        #region properties
        public ICommand SaveCommand { get { return new RelayCommand(OnSave); } }

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
