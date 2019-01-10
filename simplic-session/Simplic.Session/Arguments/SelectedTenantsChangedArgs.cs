using System;
using System.Collections.Generic;

namespace Simplic.Session
{
    /// <summary>
    /// Tenant selection event args
    /// </summary>
    public class SelectedTenantsChangedArgs : EventArgs
    {
        /// <summary>
        /// Gets all selected tenants
        /// </summary>
        public IList<Tenant.OrganizationTenant> NewOrganizationTenants { get; internal set; }

        /// <summary>
        /// Gets all previously selected tenants
        /// </summary>
        public IList<Tenant.OrganizationTenant> OldOrganizationTenants { get; internal set; }
    }
}
