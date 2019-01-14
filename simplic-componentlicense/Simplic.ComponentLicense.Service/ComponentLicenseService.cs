using Simplic.Cache;
using Simplic.ComponentLicense;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplic.ComponentLicense.Service
{
    /// <summary>
    /// Component license service
    /// </summary>
    public class ComponentLicenseService : IComponentLicenseService
    {
        private const string CacheKey = "ComponentLicense_Key";
        private readonly IComponentLicenseRepository repository;
        private readonly ICacheService cacheService;

        /// <summary>
        /// Initialize service
        /// </summary>
        /// <param name="repository">Repository instance</param>
        /// <param name="cacheService">Cache service</param>
        public ComponentLicenseService(IComponentLicenseRepository repository, ICacheService cacheService)
        {
            this.repository = repository;
            this.cacheService = cacheService;
        }

        /// <summary>
        /// Get component license by name
        /// </summary>
        /// <param name="name">Component name</param>
        /// <returns>License instance</returns>
        public ComponentLicense Get(string name)
        {
            var cacheKey = $"{CacheKey}_{name}";
            var license = cacheService.Get<ComponentLicense>(cacheKey);
            if (license != null)
                return license;

            license = repository.Get(name);
            cacheService.Set(cacheKey, license);

            return license;
        }
    }
}
