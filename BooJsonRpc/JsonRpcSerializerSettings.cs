using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BooJsonRpc
{
    public class JsonRpcSerializerSettings
    {
        #region Static Members

        static JsonRpcSerializerSettings()
        {
            Default = new JsonRpcSerializerSettings { };
        }

        public static JsonRpcSerializerSettings Default { get; private set; }

        #endregion

        public JsonRpcSerializerSettings()
        {
            JsonSerializerSettings = new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                NullValueHandling = NullValueHandling.Ignore
            };
        }

        public JsonSerializerSettings JsonSerializerSettings { get; private set; }
    }
}
