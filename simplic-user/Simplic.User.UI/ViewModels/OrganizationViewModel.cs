using Simplic.TenantSystem;
using Simplic.UI.MVC;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Simplic.User.UI
{
    /// <summary>
    /// View model for the tenant entity
    /// </summary>
    public class OrganizationViewModel : ViewModelBase, INamedEntity
    {
        #region fields
        private Guid _organizationId;
        private string _organizationName;
        private string _matchCode;
        private bool _isActive;
        private bool _isGroup;
        private ObservableCollection<UserViewModel> _users;
        public int _subOrganizationCount;
        public IList<Guid> _subOrganizations;
        public Guid? _cloudOrganizationId;
        #endregion

        #region ctr
        /// <summary>
        /// Default constructor for the view model
        /// </summary>
        public OrganizationViewModel()
        {
            Users = new ObservableCollection<UserViewModel>();
        }

        /// <summary>
        /// Constructor for the view model
        /// </summary>
        /// <param name="id">Tenant ID</param>
        /// <param name="name">Tenant name</param>
        /// <param name="matchCode">Tenant match code</param>
        /// <param name="subOrganizationCount">Tenant suborganizations count</param>
        /// <param name="subOrganizations">Tenant suborganizations IDs collection</param>
        /// <param name="cloudOrganizationId">Tenant cloud organization id</param>
        /// <param name="isActive">Tenant active flag</param>
        /// <param name="isGroup">Tenant group flag</param>
        public OrganizationViewModel(Guid id, string name, string matchCode, int subOrganizationCount, 
            IList<Guid> subOrganizations, Guid? cloudOrganizationId, bool isActive, bool isGroup) : this()
        {
            OrganizationId = id;
            Name = name;
            MatchCode = matchCode;
            IsActive = isActive;
            IsGroup = isGroup;
            SubOrganizationCount = subOrganizationCount;
            SubOrganizations = subOrganizations;
            CloudOrganizationId = cloudOrganizationId;
        }

        /// <summary>
        /// Copy constructor
        /// </summary>
        /// <param name="org"></param>
        public OrganizationViewModel(Organization org) : this(org.Id, org.Name, org.MatchCode, org.SubOrganizationCount,
            org.SubOrganizations, org.CloudOrganizationId, org.IsActive, org.IsGroup)
        {
        }
        #endregion

        #region properties
        /// <summary>
        /// Tenant organization id
        /// </summary>
        public Guid OrganizationId
        {
            get { return _organizationId; }
            set { PropertySetter(value, newValue => _organizationId = newValue); }
        }

        /// <summary>
        /// Tenant name
        /// </summary>
        public string Name
        {
            get { return _organizationName; }
            set { PropertySetter(value, newValue => _organizationName = newValue); }
        }

        /// <summary>
        /// Tenant match code
        /// </summary>
        public string MatchCode
        {
            get { return _matchCode; }
            set { PropertySetter(value, newValue => _matchCode = newValue); }
        }

        /// <summary>
        /// Tenant active flag
        /// </summary>
        public bool IsActive
        {
            get { return _isActive; }
            set { PropertySetter(value, newValue => _isActive = newValue); }
        }

        /// <summary>
        /// Tenant suborganizations IDs collection
        /// </summary>
        public IList<Guid> SubOrganizations
        {
            get { return _subOrganizations; }
            set { PropertySetter(value, newValue => _subOrganizations = newValue); }
        }

        /// <summary>
        /// Tenant group flag
        /// </summary>
        public bool IsGroup
        {
            get { return _isGroup; }
            set { PropertySetter(value, newValue => _isGroup = newValue); }
        }

        /// <summary>
        /// Tenant's users collection
        /// </summary>
        public ObservableCollection<UserViewModel> Users
        {
            get { return _users; }
            set { PropertySetter(value, newValue => _users = newValue); }
        }

        /// <summary>
        /// Tenant suborganizations count
        /// </summary>
        public int SubOrganizationCount
        {
            get { return _subOrganizationCount; }
            set { PropertySetter(value, newValue => _subOrganizationCount = newValue); }
        }

        /// <summary>
        /// Tenant cloud organization id
        /// </summary>
        public Guid? CloudOrganizationId
        {
            get { return _cloudOrganizationId; }
            set { PropertySetter(value, newValue => _cloudOrganizationId = newValue); }
        }
        #endregion
    }
}
