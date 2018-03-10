using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace BooJsonRpc
{
    internal static class JsonRpcWriter
    {
        private static readonly JsonSerializerSettings m_settings = new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver(), NullValueHandling = NullValueHandling.Ignore };

        public static string Write(IEnumerable<JsonRpcResponse> responses)
        {
            if (responses == null || !responses.Any())
                throw new ArgumentNullException(nameof(responses));

            if (responses.Count() == 1)
                return JsonConvert.SerializeObject(responses.First());
            else
                return JsonConvert.SerializeObject(responses, m_settings);
        }
    }
}
