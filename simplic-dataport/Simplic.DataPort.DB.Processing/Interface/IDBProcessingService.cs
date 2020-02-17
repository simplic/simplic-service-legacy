using System;
using System.Collections.Generic;
using System.Data;

namespace Simplic.DataPort.DB.Processing
{
    public interface IDBProcessingService
    {
        /// <summary>
        /// Checks if table and columns exist, creates them if not and goes through the data and saves each data into the db. 
        /// If any error is caused, will be logged to be retried later.
        /// </summary>
        /// <param name="transformerName">Transformer used to produce this db processing data</param>
        /// <param name="data">Data to save</param>
        /// <param name="connectionName">Connection Name</param>
        void SaveData(string transformerName, FileTypeDBModel data, string connectionName = "default");

        /// <summary>
        /// Tries to save data again with the corrected sql or data. 
        /// Updates the current error log if this one fails too. 
        /// Deletes the error log entry if the corrected sql runs successfully.
        /// </summary>
        /// <param name="errorLogModel">Corrected data & sql</param>
        /// <param name="connectionName">Connection Name</param>
        bool Retry(ErrorLogModel errorLogModel, string connectionName = "default");

        void LogTableError(TableSchemaModel tableSchema, Exception exception, string connectionName = "default");
        //void LogRowError(string tableName, string transformerName, DataRow row, Exception exception, string connectionName = "default");
        void LogRowError(string tableName, string transformerName, IDictionary<string, string> row, Exception exception, string connectionName = "default");
        
        ErrorLogModel GetErrorLog(long id, string connectionName = "default");
        IEnumerable<ErrorLogModel> GetAllErrorLog(string connectionName = "default");
    }
}
