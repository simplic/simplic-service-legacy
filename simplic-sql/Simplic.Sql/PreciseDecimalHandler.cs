using Dapper;
using Simplic.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplic.Sql
{
    /// <summary>
    /// <see cref="PreciseDecimal"/> dapper type handler
    /// </summary>
    internal class PreciseDecimalHandler : SqlMapper.TypeHandler<PreciseDecimal>
    {
        public override void SetValue(IDbDataParameter parameter, PreciseDecimal value)
        {
            parameter.Value = (double)value;
        }

        public override PreciseDecimal Parse(object value)
        {
            return new PreciseDecimal((double)value);
        }
    }
}
