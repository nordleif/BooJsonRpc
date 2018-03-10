using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace BooJsonRpc
{
    internal static class JsonRpcReader
    {
        public static object ReadJson(string json)
        {
            if (string.IsNullOrWhiteSpace(json))
                throw new ArgumentNullException(nameof(json));

            try
            {
                return JsonConvert.DeserializeObject(json);
            }
            catch (Exception ex)
            {
                throw new JsonRpcException(JsonRpcError.ParseError, ex);
            }
        }

        public static JsonRpcRequest ReadRequest(JToken token)
        {
            try
            {
                if (token == null)
                    throw new ArgumentNullException(nameof(token));

                var request = new JsonRpcRequest();
                request.JsonRpc = (string)token["jsonrpc"];
                request.Id = (string)token["id"];
                request.Method = (string)token["method"];
                request.Params = ReadParams(token["params"]);

                // MUST be exactly "2.0".
                if (request.JsonRpc != "2.0")
                    throw new JsonRpcException(JsonRpcError.InvalidRequest);

                return request;
            }
            catch (JsonRpcException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new JsonRpcException(JsonRpcError.ParseError, ex);
            }
        }

        public static JsonRpcParams ReadParams(JToken token)
        {
            // This member MAY be omitted.
            if (token == null)
                return null;

            return new JsonRpcParams();
        }
    }
}
