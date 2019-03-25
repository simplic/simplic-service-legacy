using System;

namespace Simplic.DataPort.DB.Processing
{
    public class ErrorLogModel
    {
        public int Id { get; set; }
        public string TableName { get; set; }
        public string SqlQuery { get; set; }
        public string Data { get; set; }
        public string ExceptionDetails { get; set; }
        public string TransformerName { get; set; }
        public DateTime CreateTime { get; set; } = DateTime.Now;
        public bool Handled { get; set; }
    }
}
