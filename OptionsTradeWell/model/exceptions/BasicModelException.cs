using System;
using System.Runtime.Serialization;

namespace OptionsTradeWell.model.exceptions
{
    public class BasicModelException :Exception
    {
        public BasicModelException() : base()
        {
        }

        public BasicModelException(String message): base(message)
        {
        }

        public BasicModelException(String message, Exception innerException): base(message, innerException)
        {
        }

        protected BasicModelException(SerializationInfo info, StreamingContext context): base(info, context)
        {
        }
    }
}