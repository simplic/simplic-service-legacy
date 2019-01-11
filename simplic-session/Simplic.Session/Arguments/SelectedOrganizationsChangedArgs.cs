using System;
using System.Collections.Generic;

namespace Simplic.Session
{
    /// <summary>
    /// Tenant selection event args
    /// </summary>
    public class SelectedOrganizationsChangedArgs : EventArgs
    {
        /// <summary>
        /// Gets all selected organizations
        /// </summary>
        public IList<Tenant.Organization> NewOrganizations { get; internal set; }

        /// <summary>
        /// Gets all previously selected organizations
        /// </summary>
        public IList<Tenant.Organization> OldOrganizations { get; internal set; }
    }
}
