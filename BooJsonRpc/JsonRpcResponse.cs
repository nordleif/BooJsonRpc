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
        /// A String specifying the version of the JSON-RPC protocol. MUST be exactly "2.0".
        /// </summary>
        public string JsonRpc { get; set; }

        /// <summary>
        /// This member is REQUIRED on success.
        /// This member MUST NOT exist if there was an error invoking the method.
        /// The value of this member is determined by the method invoked on the Server.
        /// </summary>
        public dynamic Result { get; set; }

        /// <summary>
        /// This member is REQUIRED on error.
        /// This member MUST NOT exist if there was no error triggered during invocation.
        /// </summary>
        public JsonRpcError Error { get; set; }

        /// <summary>
        /// An identifier established by the Client that MUST contain a String, Number, or NULL value if included. 
        /// If it is not included it is assumed to be a notification. 
        /// The value SHOULD normally not be Null [1] and Numbers SHOULD NOT contain fractional parts
        /// </summary>
        public string Id { get; set; }
    }
}
