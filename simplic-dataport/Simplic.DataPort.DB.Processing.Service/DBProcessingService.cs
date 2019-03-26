using System;
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

        public void Retry(ErrorLogModel errorLogModel, string connectionName = "default")
        {
            throw new System.NotImplementedException();
        }

        public void SaveData(string transformerName, FileTypeDBModel data, string connectionName = "default")
        {
            foreach (var table in data.Tables)
            {
                if (!repository.TableExists(table.Table, connectionName))
                {
                    var tableSchema = CreateSchema(table);
                    try
                    {
                       repository.CreateTable(tableSchema, connectionName);
                    }
                    catch (System.Exception ex)
                    {
                        repository.LogTableError(tableSchema, ex, connectionName);                                                
                    }
                }

                for (int i = 0; i < table.Data.Rows.Count; i++)
                {
                    var row = table.Data.Rows[i];

                    try
                    {
                        repository.InsertOrUpdate(transformerName, table.Table, row, connectionName);
                    }
                    catch (System.Exception ex)
                    {
                        repository.LogRowError(table.Table, transformerName, row, ex, connectionName);                        
                    }
                }
            }
        }

        private TableSchemaModel CreateSchema(DBTableModel table)
        {
            return new TableSchemaModel
            {
                TableName = table.Table,
                Columns = CreateColumns(table.Columns)
            };
        }

        private IList<TableColumnModel> CreateColumns(IList<DBRecordTableColumnModel> columns)
        {
            var columnList = new List<TableColumnModel>();

            foreach (var column in columns)
            {
                columnList.Add(new TableColumnModel
                {
                    Name = column.Name,
                    Null = column.Null,
                    PrimaryKey = column.PrimaryKey,
                    Type = GetType(column)
                });
            }

            return columnList;
        }

        private string GetType(DBRecordTableColumnModel column)
        {
            if (column.Type == "string")
                return "varchar(255)";
            else if (column.Type == "number")
                return "double";
            else
                return column.Type;
        }

        public void LogTableError(TableSchemaModel tableSchema, Exception exception, string connectionName = "default")
        {
            repository.LogTableError(tableSchema, exception, connectionName);
        }

        public void LogRowError(string tableName, string transformerName, DataRow row, Exception exception, string connectionName = "default")
        {
            repository.LogRowError(tableName, transformerName, row, exception, connectionName);
        }

        public ErrorLogModel GetErrorLog(long id, string connectionName = "default")
        {
            return repository.GetErrorLog(id, connectionName);
        }

        public IEnumerable<ErrorLogModel> GetAllErrorLog(string connectionName = "default")
        {
            return repository.GetAllErrorLog(connectionName);
        }
    }
}
