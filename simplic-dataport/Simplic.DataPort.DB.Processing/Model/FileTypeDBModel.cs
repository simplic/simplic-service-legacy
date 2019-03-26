using System.Collections.Generic;

namespace Simplic.DataPort.DB.Processing
{
    public class FileTypeDBModel
    {
        public IList<DBTableModel> Tables { get; set; } = new List<DBTableModel>();
    }        
}
