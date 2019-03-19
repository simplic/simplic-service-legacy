using System.Data;

namespace Simplic.DataPort.DB.Processing.Service
{
    public class DBProcessingService : IDBProcessingService
    {
        private readonly IDBProcessingRepository repository;

        public DBProcessingService(IDBProcessingRepository repository)
        {
            this.repository = repository;
        }

        public bool ColumnExists(string tableName, string columnName, string connectionName = "default")
        {
            return repository.ColumnExists(tableName, columnName, connectionName);
        }

        public bool CreateTable(TableSchemaModel tableSchema, string connectionName = "default")
        {
            return repository.CreateTable(tableSchema, connectionName);
        }
        
        public void InsertOrUpdate(string tableName, DataRow row, string connectionName = "default")
        {
            repository.InsertOrUpdate(tableName, row, connectionName);
        }

        public bool TableExists(string tableName, string connectionName = "default")
        {
            return repository.TableExists(tableName, connectionName);
        }
    }
}
