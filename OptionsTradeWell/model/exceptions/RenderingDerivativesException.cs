using System;
using System.Runtime.Serialization;

namespace OptionsTradeWell.model.exceptions
{
    public class RenderingDerivativesException : Exception
    {
        public RenderingDerivativesException() : base()
        {
        }

        public RenderingDerivativesException(String message): base(message)
        {
        }

        public RenderingDerivativesException(String message, Exception innerException): base(message, innerException)
        {
        }

        protected RenderingDerivativesException(SerializationInfo info, StreamingContext context): base(info, context)
        {
        }
    }
}