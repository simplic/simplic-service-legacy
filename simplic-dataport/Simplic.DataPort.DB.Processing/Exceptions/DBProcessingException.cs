using System;
using System.Runtime.Serialization;

namespace Simplic.DataPort.DB.Processing
{
    public class DBProcessingException : Exception
    {
        public DBProcessingException()
        {
        }

        public DBProcessingException(string message) : base(message)
        {
        }

        public DBProcessingException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected DBProcessingException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
