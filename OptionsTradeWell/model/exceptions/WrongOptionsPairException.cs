using System;
using System.Runtime.Serialization;

namespace OptionsTradeWell.model.exceptions
{
    public class WrongOptionsPairException : Exception
    {
        public WrongOptionsPairException() : base()
        {
        }

        public WrongOptionsPairException(String message): base(message)
        {
        }

        public WrongOptionsPairException(String message, Exception innerException): base(message, innerException)
        {
        }

        protected WrongOptionsPairException(SerializationInfo info, StreamingContext context): base(info, context)
        {
        }
    }
}