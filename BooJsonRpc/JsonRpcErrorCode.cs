using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BooJsonRpc
{
    public struct JsonRpcErrorCode
    {
        #region Static Members

        /// <summary>
        /// Invalid JSON was received by the server.
        /// An error occurred on the server while parsing the JSON text.
        /// </summary>
        public static readonly JsonRpcErrorCode ParseError = new JsonRpcErrorCode { Number = -32700, Message = "Parse error" };

        /// <summary>
        /// The JSON sent is not a valid Request object.
        /// </summary>
        public static readonly JsonRpcErrorCode InvalidRequest = new JsonRpcErrorCode { Number = -32600, Message = "Invalid Request" };

        /// <summary>
        /// The method does not exist / is not available.
        /// </summary>
        public static readonly JsonRpcErrorCode MethodNotFound = new JsonRpcErrorCode { Number = -32601, Message = "Method not found" };

        /// <summary>
        /// Invalid method parameter(s).
        /// </summary>
        public static readonly JsonRpcErrorCode InvalidParams = new JsonRpcErrorCode { Number = -32602, Message = "Invalid params" };

        /// <summary>
        /// Internal JSON-RPC error.
        /// </summary>
        public static readonly JsonRpcErrorCode InternalError = new JsonRpcErrorCode { Number = -32603, Message = "Internal error" };

        #endregion

        public int Number { get; set; }

        public string Message { get; set; }
    }
}
