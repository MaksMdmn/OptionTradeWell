using System;
using System.Runtime.Serialization;

namespace OptionsTradeWell.model.exceptions
{
    public class QuikDdeException : Exception
    {
        public QuikDdeException() : base()
        {
        }

        public QuikDdeException(String message): base(message)
        {
        }

        public QuikDdeException(String message, Exception innerException): base(message, innerException)
        {
        }

        protected QuikDdeException(SerializationInfo info, StreamingContext context): base(info, context)
        {
        }
    }
}