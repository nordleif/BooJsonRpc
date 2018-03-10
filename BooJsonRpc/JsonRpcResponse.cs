using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BooJsonRpc
{
    /// <summary>
    /// When a rpc call is made, the Server MUST reply with a Response, except for in the case of Notifications.
    /// </summary>
    public class JsonRpcResponse : JsonRpcObject
    {
        public JsonRpcResponse()
        {

        }

        /// <summary>
        /// This member is REQUIRED on success.
        /// This member MUST NOT exist if there was an error invoking the method.
        /// The value of this member is determined by the method invoked on the Server.
        /// </summary>
        public object Result { get; set; }

        /// <summary>
        /// This member is REQUIRED on error.
        /// This member MUST NOT exist if there was no error triggered during invocation.
        /// </summary>
        public JsonRpcError Error { get; set; }
    }
}
