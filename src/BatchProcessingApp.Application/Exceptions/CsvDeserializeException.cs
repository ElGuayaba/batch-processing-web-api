using System;
using System.Collections.Generic;
using System.Text;

namespace BatchProcessingApp.Application.Exceptions
{
    public class CsvDeserializeException : Exception
    {
        public CsvDeserializeException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
