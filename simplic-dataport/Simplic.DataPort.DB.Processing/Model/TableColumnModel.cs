namespace Simplic.DataPort.DB.Processing
{
    public class TableColumnModel
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public bool PrimaryKey { get; set; }
        public bool Null { get; set; } = true;
    }
}
