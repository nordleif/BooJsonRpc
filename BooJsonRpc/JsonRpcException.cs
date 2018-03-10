using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BooJsonRpc
{
    public class JsonRpcException : Exception
    {
        public JsonRpcException(JsonRpcErrorCode errorCode)
            : base(errorCode.Message)
        {
            Error = new JsonRpcError(errorCode);
        }

        public JsonRpcException(JsonRpcErrorCode errorCode, Exception innerException)
            : base(errorCode.Message, innerException)
        {
            Error = new JsonRpcError(errorCode, innerException);
        }

        public JsonRpcError Error { get; private set; }
    }
}
