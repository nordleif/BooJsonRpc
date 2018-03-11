using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BooJsonRpc
{
    public abstract class JsonRpcObject
    {
        public JsonRpcObject()
        {

        }

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
