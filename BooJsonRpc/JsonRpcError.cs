using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BooJsonRpc
{
    public class JsonRpcError
    {
        #region Static Members

        /// <summary>
        /// Invalid JSON was received by the server.
        /// An error occurred on the server while parsing the JSON text.
        /// </summary>
        public static readonly JsonRpcError ParseError = new JsonRpcError { Number = -32700, Message = "Parse error" };

        /// <summary>
        /// The JSON sent is not a valid Request object.
        /// </summary>
        public static readonly JsonRpcError InvalidRequest = new JsonRpcError { Number = -32600, Message = "Invalid Request" };

        /// <summary>
        /// The method does not exist / is not available.
        /// </summary>
        public static readonly JsonRpcError MethodNotFound = new JsonRpcError { Number = -32601, Message = "Method not found" };

        /// <summary>
        /// Invalid method parameter(s).
        /// </summary>
        public static readonly JsonRpcError InvalidParams = new JsonRpcError { Number = -32602, Message = "Invalid params" };

        /// <summary>
        /// Internal JSON-RPC error.
        /// </summary>
        public static readonly JsonRpcError InternalError = new JsonRpcError { Number = -32603, Message = "Internal error" };

        #endregion

        /// <summary>
        /// The error codes from and including -32768 to -32000 are reserved for pre-defined errors. 
        /// Any code within this range, but not defined explicitly below is reserved for future use. 
        /// -32000 to -32099 (Server error) are reserved for implementation-defined server-errors.
        /// </summary>
        public JsonRpcError()
        {

        }

        /// <summary>
        /// A Number that indicates the error type that occurred.
        /// This MUST be an integer.
        /// </summary>
        public int Number { get; set; }

        /// <summary>
        /// A String providing a short description of the error.
        /// The message SHOULD be limited to a concise single sentence.
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// A Primitive or Structured value that contains additional information about the error.
        /// This may be omitted.
        /// The value of this member is defined by the Server (e.g. detailed error information, nested errors etc.).
        /// </summary>
        public object Data { get; set; }
    }
}
