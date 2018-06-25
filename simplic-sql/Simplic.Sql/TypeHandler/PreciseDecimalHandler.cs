using Dapper;
using Simplic.Data;
using System;
using System.Data;

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
            if(value is decimal)
            {
                return new PreciseDecimal(Convert.ToDouble((decimal)value));
            }
            return new PreciseDecimal((double)value);
        }
    }
}
