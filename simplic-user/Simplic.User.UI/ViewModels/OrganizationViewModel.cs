using Simplic.TenantSystem;
using Simplic.UI.MVC;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Simplic.User.UI
{
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
        public OrganizationViewModel()
        {
            Users = new ObservableCollection<UserViewModel>();
        }

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

        public OrganizationViewModel(Organization org) : this(org.Id, org.Name, org.MatchCode, org.SubOrganizationCount,
            org.SubOrganizations, org.CloudOrganizationId, org.IsActive, org.IsGroup)
        {
        }
        #endregion

        #region properties
        public Guid OrganizationId
        {
            get { return _organizationId; }
            set { PropertySetter(value, newValue => _organizationId = newValue); }
        }

        public string Name
        {
            get { return _organizationName; }
            set { PropertySetter(value, newValue => _organizationName = newValue); }
        }

        public string MatchCode
        {
            get { return _matchCode; }
            set { PropertySetter(value, newValue => _matchCode = newValue); }
        }

        public bool IsActive
        {
            get { return _isActive; }
            set { PropertySetter(value, newValue => _isActive = newValue); }
        }

        public IList<Guid> SubOrganizations
        {
            get { return _subOrganizations; }
            set { PropertySetter(value, newValue => _subOrganizations = newValue); }
        }

        public bool IsGroup
        {
            get { return _isGroup; }
            set { PropertySetter(value, newValue => _isGroup = newValue); }
        }

        public ObservableCollection<UserViewModel> Users
        {
            get { return _users; }
            set { PropertySetter(value, newValue => _users = newValue); }
        }

        public int SubOrganizationCount
        {
            get { return _subOrganizationCount; }
            set { PropertySetter(value, newValue => _subOrganizationCount = newValue); }
        }

        public Guid? CloudOrganizationId
        {
            get { return _cloudOrganizationId; }
            set { PropertySetter(value, newValue => _cloudOrganizationId = newValue); }
        }
        #endregion
    }
}
