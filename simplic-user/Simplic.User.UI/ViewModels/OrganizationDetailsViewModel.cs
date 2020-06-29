using CommonServiceLocator;
using Simplic.TenantSystem;
using Simplic.UI.MVC;
using System;
using System.Linq;
using System.Windows.Input;

namespace Simplic.User.UI
{
    /// <summary>
    /// View model for tenant details dialog window
    /// </summary>
    class OrganizationDetailsViewModel : ViewModelBase, IUserDialogViewModel, ISaveableViewModel
    {
        #region fields
        private OrganizationViewModel _organization;
        private string _newOrganizationName;
        #endregion

        #region ctr
        /// <summary>
        /// Constructor for the view model
        /// </summary>
        /// <param name="organization">Current tenant</param>
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

        /// <summary>
        /// Dialog window close request method. Inherited from <see cref="IUserDialogViewModel"/>
        /// </summary>
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
        /// <summary>
        /// Save state command. Inherited from <see cref="ISaveableViewModel"/>
        /// </summary>
        public ICommand SaveCommand { get { return new RelayCommand(OnSave); } }

        /// <summary>
        /// Dialog window modality flag. Inherited from <see cref="IUserDialogViewModel"/>
        /// </summary>
        public bool IsModal => true;

        /// <summary>
        /// Current tenant
        /// </summary>
        public OrganizationViewModel Organization
        {
            get { return _organization; }
            set { PropertySetter(value, newValue => _organization = newValue); }
        }

        /// <summary>
        /// The name of the tenant, which can be changed through the UI
        /// </summary>
        public string NewOrganizationName
        {
            get { return _newOrganizationName; }
            set { PropertySetter(value, newValue => _newOrganizationName = newValue); }
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
