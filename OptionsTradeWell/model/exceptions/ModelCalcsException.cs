using System;
using System.Runtime.Serialization;

namespace OptionsTradeWell.model.exceptions
{
    public class ModelCalcsException :Exception
    {
        public ModelCalcsException() : base()
        {
        }

        public ModelCalcsException(String message): base(message)
        {
        }

        public ModelCalcsException(String message, Exception innerException): base(message, innerException)
        {
        }

        protected ModelCalcsException(SerializationInfo info, StreamingContext context): base(info, context)
        {
        }
    }
}