using System.Collections.Generic;

namespace Simplic.DataPort.DB.Processing
{
    public class TableSchemaModel
    {
        public string TableName { get; set; }
        public IList<TableColumnModel> Columns { get; set; }
    }
}
