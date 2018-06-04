using Dapper;
using Simplic.Data;
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
            return new PreciseDecimal((double)value);
        }
    }
}
