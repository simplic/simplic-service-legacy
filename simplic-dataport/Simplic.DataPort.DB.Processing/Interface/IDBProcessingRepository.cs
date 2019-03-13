using System.Threading.Tasks;

namespace Simplic.DataPort.DB.Processing
{
    public interface IDBProcessingRepository
    {
        Task<bool> TableExistsAsync(string tableName);
        Task<bool> ColumnExistsAsync(string tableName, string columnName);
    }
}
