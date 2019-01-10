using System.Collections.Generic;

namespace Simplic.Session
{
    /// <summary>
    /// An object holding information about the current logged in user 
    /// </summary>
    public class Session
    {
        private IList<Tenant.OrganizationTenant> tenantOrganizations = new List<Tenant.OrganizationTenant>();

        /// <summary>
        /// Tenant changed event
        /// </summary>
        public event TenantSelectionChangedEventHandler TenantOrganizationSelectionChanged;

        /// <summary>
        /// Gets the currently logged in user id
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// Gets the currently logged in user name
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// Gets the currently logged in users access groups
        /// </summary>
        public IList<int> UserAccessGroups { get; set; }

        /// <summary>
        /// Gets the currently logged in users id as bit mask
        /// </summary>
        public string UserBitMask { get; set; }

        /// <summary>
        /// Gets the currently logged in users access groups ids as bit mask
        /// </summary>
        public string UserAccessGroupsBitMask { get; set; }

        /// <summary>
        /// Gets the currently logged in users role (wether its a super user or not)
        /// </summary>
        public bool IsSuperUser { get; set; }

        /// <summary>
        /// Gets or sets whether the current user is an active directory user
        /// </summary>
        public bool IsADUser { get; set; }

        /// <summary>
        /// Gets or sets all available tenant organizations
        /// </summary>
        private IList<Tenant.OrganizationTenant> TenantOrganizations
        {
            get => tenantOrganizations;
            set
            {
                var args = new SelectedTenantsChangedArgs
                {
                    NewOrganizationTenants = value,
                    OldOrganizationTenants = tenantOrganizations
                };

                // Set new organization tenants
                tenantOrganizations = value;

                TenantOrganizationSelectionChanged?.Invoke(this, args);
            }
        }
    }
}