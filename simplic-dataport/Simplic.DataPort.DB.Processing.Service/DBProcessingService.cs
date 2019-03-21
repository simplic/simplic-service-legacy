using System.Collections.Generic;
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

        public IEnumerable<ErrorLogModel> GetAllErrorLog(string connectionName = "default")
        {
            return repository.GetAllErrorLog(connectionName);
        }

        public ErrorLogModel GetErrorLog(long id, string connectionName = "default")
        {
            return repository.GetErrorLog(id, connectionName);
        }

        public void InsertOrUpdate(string tableName, DataRow row, string connectionName = "default")
        {
            repository.InsertOrUpdate(tableName, row, connectionName);
        }

        public bool Retry(ErrorLogModel errorLogModel, string connectionName = "default")
        {
            throw new System.NotImplementedException();
        }

        public bool TableExists(string tableName, string connectionName = "default")
        {
            return repository.TableExists(tableName, connectionName);
        }
    }
}
