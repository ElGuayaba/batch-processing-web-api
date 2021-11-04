using System;
using System.Collections.Generic;
using System.Text;

namespace BatchProcessingApp.Application.Exceptions
{
    public class DataProcessingException : Exception
    {
        public DataProcessingException(string message) : base(message)
        {
        }
    }
}
