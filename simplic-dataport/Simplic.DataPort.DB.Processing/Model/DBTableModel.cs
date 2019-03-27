using System.Collections.Generic;

namespace Simplic.DataPort.DB.Processing
{
    public class DBTableModel
    {
        public string Table { get; set; }
        public IList<DBRecordTableColumnModel> Columns { get; set; } = new List<DBRecordTableColumnModel>();        
        public IList<IDictionary<string, string>> Data { get; set; }
    }
}
