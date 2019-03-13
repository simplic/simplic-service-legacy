using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplic.DataPort.DB.Processing.Data.Sql
{
    public class DBProcessingSqlRepository : IDBProcessingRepository
    {
        public Task<bool> ColumnExistsAsync(string tableName, string columnName)
        {
            throw new NotImplementedException();
        }

        public Task<bool> TableExistsAsync(string tableName)
        {
            throw new NotImplementedException();
        }
    }
}
