using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BooJsonRpc
{
    public class JsonRpcObject
    {
        public JsonRpcObject()
        {

        }

        /// <summary>
        /// A String specifying the version of the JSON-RPC protocol. MUST be exactly "2.0".
        /// </summary>
        public string JsonRpc { get; set; }

        /// <summary>
        /// An identifier established by the Client that MUST contain a String, Number, or NULL value if included. 
        /// If it is not included it is assumed to be a notification. 
        /// The value SHOULD normally not be Null [1] and Numbers SHOULD NOT contain fractional parts
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Creates a shallow copy of the current object.
        /// </summary>
        /// <returns></returns>
        public JsonRpcObject Clone()
        {
            return (JsonRpcObject)this.MemberwiseClone();
        }
    }
}
