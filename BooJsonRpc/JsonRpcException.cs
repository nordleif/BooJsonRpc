using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BooJsonRpc
{
    public class JsonRpcException : Exception
    {
        public JsonRpcException(JsonRpcError error)
            : base(error.Message)
        {
            if (error == null)
                throw new ArgumentNullException(nameof(error));

            Error = error;
        }

        public JsonRpcException(JsonRpcError error, Exception innerException)
            : base(error.Message, innerException)
        {
            if (error == null)
                throw new ArgumentNullException(nameof(error));

            Error = error;
        }

        public JsonRpcError Error { get; private set; }
    }
}
