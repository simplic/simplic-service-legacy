using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplic.Sql
{
    /// <summary>
    /// Typehandler
    /// </summary>
    public static class TypeHandler
    {
        /// <summary>
        /// Register dapper type handler
        /// </summary>
        public static void RegisterTypeHandler()
        {
            SqlMapper.AddTypeHandler(new PreciseDecimalHandler());
        }
    }
}
