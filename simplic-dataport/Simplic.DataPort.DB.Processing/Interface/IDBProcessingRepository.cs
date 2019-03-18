using System.Data;
using System.Threading.Tasks;

namespace Simplic.DataPort.DB.Processing
{
    public interface IDBProcessingRepository
    {
        bool TableExists(string tableName, string connectionName = "default");
        bool ColumnExists(string tableName, string columnName, string connectionName = "default");
        bool InsertOrUpdate(string tableName, DataRow row, string connectionName = "default");
        bool CreateTable(TableSchemaModel tableSchema, string connectionName = "default");
    }
}
