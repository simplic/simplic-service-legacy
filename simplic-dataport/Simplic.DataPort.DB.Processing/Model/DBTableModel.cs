using System.Collections.Generic;
using System.Data;

namespace Simplic.DataPort.DB.Processing
{
    public class DBTableModel
    {
        public string Table { get; set; }
        public IList<DBRecordTableColumnModel> Columns { get; set; } = new List<DBRecordTableColumnModel>();
        public DataTable Data { get; set; }
    }
}
