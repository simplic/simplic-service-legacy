using System;
using System.Collections.Generic;
using System.Data;

namespace Simplic.DataPort.DB.Processing
{
    public interface IDBProcessingRepository
    {
        bool TableExists(string tableName, string connectionName = "default");
        bool ColumnExists(string tableName, string columnName, string connectionName = "default");
        void InsertOrUpdate(string transformerName, string tableName, DataRow row, string connectionName = "default");
        bool CreateTable(TableSchemaModel tableSchema, string connectionName = "default");

        void LogTableError(TableSchemaModel tableSchema, Exception exception, string connectionName = "default");
        void LogRowError(string tableName, string transformerName, DataRow row, Exception exception, string connectionName = "default");
        ErrorLogModel GetErrorLog(long id, string connectionName = "default");
        IEnumerable<ErrorLogModel> GetAllErrorLog(string connectionName = "default");        
    }
}
