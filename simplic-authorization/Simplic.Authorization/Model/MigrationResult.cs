using System;

namespace Simplic.Authorization
{
    public class MigrationResult
    {
        public string TableName { get; set; }
        public MigrationStatus Status { get; set; }
        public Exception Exception { get; set; }
    }
}
