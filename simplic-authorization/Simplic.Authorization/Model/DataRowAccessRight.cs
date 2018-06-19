namespace Simplic.Authorization
{
    public class DataRowAccessRight
    {
        public int? OwnerId { get; set; }
        public string UserFullAccess { get; set; }
        public string UserReadAccess { get; set; }
        public string UserWriteAccess { get; set; }
        public string GroupFullAccess { get; set; }
        public string GroupReadAccess { get; set; }
        public string GroupWriteAccess { get; set; }
    }
}
