using System;
using System.Runtime.Serialization;

namespace OptionsTradeWell.model.exceptions
{
    public class IllegalViewDataException : Exception
    {
        public IllegalViewDataException() : base()
        {
        }

        public IllegalViewDataException(String message): base(message)
        {
        }

        public IllegalViewDataException(String message, Exception innerException): base(message, innerException)
        {
        }

        protected IllegalViewDataException(SerializationInfo info, StreamingContext context): base(info, context)
        {
        }
    }
}