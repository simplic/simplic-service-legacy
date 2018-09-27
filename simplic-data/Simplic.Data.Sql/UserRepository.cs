using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Simplic.Cache;
using Simplic.Sql;

namespace Simplic.Data.Sql
{
    public class UserRepository : SqlRepositoryBase<Guid, object>
    {
        public UserRepository(ISqlService sqlService, ISqlColumnService sqlColumnService, ICacheService cacheService) : base(sqlService, sqlColumnService, cacheService)
        {
        }

        public override string PrimaryKeyColumn
        {
            get
            {
                return "Guid";
            }
        }

        public IEnumerable<object> GetAllofType(string type)
        {
            return GetAllByColumn("typeName", type);
        }

        public override string TableName
        {
            get
            {
                return "User";
            }
        }

        public override Guid GetId(object obj)
        {
            return Guid.Empty;
        }
    }
}
