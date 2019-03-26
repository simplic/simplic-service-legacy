using Dapper;

namespace Simplic.DataPort.DB.Processing.Data
{
    public class SqlStatement
    {
        public string Sql { get; set; }
        public DynamicParameters Parameters { get; set; }
    }
}
