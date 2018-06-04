using Dapper;

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
