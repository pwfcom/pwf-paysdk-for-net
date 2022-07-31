using System;
using System.Runtime.Serialization;

namespace Pwf.PaySDK.Base
{
    public class PwfError : Exception
    {
        public PwfError()
        {
        }

        public PwfError(string message)
            : base(message)
        {
        }

        protected PwfError(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        public PwfError(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        public PwfError(string errorCode, string errorMsg)
            : base(errorCode + ":" + errorMsg)
        {
            this.ErrorCode = errorCode;
            this.ErrorMsg = errorMsg;
        }

        public string ErrorCode { get; set; }

        public string ErrorMsg { get; set; }
    }
}

