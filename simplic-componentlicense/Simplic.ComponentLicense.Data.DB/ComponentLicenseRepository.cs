using Simplic.Sql;
using Dapper;
using System.Linq;

namespace Simplic.ComponentLicense.Data.DB
{
    /// <summary>
    /// Component license repository (data)
    /// </summary>
    public class ComponentLicenseRepository : IComponentLicenseRepository
    {
        private readonly ISqlService sqlService;

        /// <summary>
        /// Initialize repository
        /// </summary>
        /// <param name="sqlService"></param>
        public ComponentLicenseRepository(ISqlService sqlService)
        {
            this.sqlService = sqlService;
        }

        /// <summary>
        /// Get component license by name
        /// </summary>
        /// <param name="name">Component name</param>
        /// <returns>License instance</returns>
        public ComponentLicense Get(string name)
        {
            return sqlService.OpenConnection((connection) => 
            {
                return connection.Query<ComponentLicense>("SELECT * FROM License_Component WHERE ComponentName = :name", new { name = name }).FirstOrDefault();
            });
        }
    }
}
