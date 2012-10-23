#region usings
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

using My.Common;


#endregion



namespace My.Example.Web
{
    [Serializable]
    public class DislogoutException : MyException
    {
        public DislogoutException() : base(null) {}


        public DislogoutException(string message)
                : base(message) {}


        public DislogoutException(string message, Exception innerException)
                : base(message, innerException) {}


        protected DislogoutException(SerializationInfo info, StreamingContext context)
                : base(info, context) {}
    }
}