using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BooJsonRpc
{
    public class JsonRpcError : JsonRpcObject
    {
        /// <summary>
        /// The error codes from and including -32768 to -32000 are reserved for pre-defined errors. 
        /// Any code within this range, but not defined explicitly below is reserved for future use. 
        /// -32000 to -32099 (Server error) are reserved for implementation-defined server-errors.
        /// </summary>
        public JsonRpcError(JsonRpcErrorCode errorCode, object data = null)
        {
            Number = errorCode.Number;
            Message = errorCode.Message;
            Data = data?.ToString();
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
        public string Data { get; set; }
    }
}
