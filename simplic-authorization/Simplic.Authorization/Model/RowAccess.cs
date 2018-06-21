using System.Collections.Generic;

namespace Simplic.Authorization
{
    public class RowAccess
    {
        public int? OwnerId { get; set; }
        public IList<int> UserFullAccess { get; set; }
        public IList<int> UserReadAccess { get; set; }
        public IList<int> UserWriteAccess { get; set; }
        public IList<int> GroupFullAccess { get; set; }
        public IList<int> GroupReadAccess { get; set; }
        public IList<int> GroupWriteAccess { get; set; }
    }
}
