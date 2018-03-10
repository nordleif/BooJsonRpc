using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace BooJsonRpc
{
    public static class JsonRpcConvert
    {
        public static IEnumerable<JsonRpcObject> Deserialize(string json, JsonRpcSerializerSettings settings = null)
        {
            if (string.IsNullOrWhiteSpace(json))
                throw new ArgumentNullException(nameof(json));

            if (settings == null)
                settings = JsonRpcSerializerSettings.Default;

            try
            {
                var items = new List<JsonRpcObject>();
                var obj = ParseJson(json);
                if (obj is JArray array)
                {
                    foreach (var item in array)
                        items.Add(ParseObject(item));
                }
                else
                {
                    items.Add(ParseObject((JToken)obj));
                }
                return items;
            }
            catch(JsonRpcException ex)
            {
                return new[] { new JsonRpcResponse { JsonRpc = "2.0", Error = ex.Error } };
            }
            catch(Exception ex)
            {
                return new[] { new JsonRpcResponse { JsonRpc = "2.0", Error = new JsonRpcError(JsonRpcErrorCode.InternalError, ex) } };
            }
        }

        public static string Serialize(IEnumerable<JsonRpcObject> items, JsonRpcSerializerSettings settings = null)
        {
            if (items == null)
                throw new ArgumentNullException(nameof(items));

            if (settings == null)
                settings = JsonRpcSerializerSettings.Default;

            var length = items.Count();
            if (length == 0)
                return JsonConvert.SerializeObject(new object(), settings.JsonSerializerSettings);
            else if (length == 1)
                return JsonConvert.SerializeObject(items.First(), settings.JsonSerializerSettings);
            else
                return JsonConvert.SerializeObject(items, settings.JsonSerializerSettings);
        }

        private static object ParseJson(string json)
        {
            if (string.IsNullOrWhiteSpace(json))
                throw new ArgumentNullException(nameof(json));

            try
            {
                return JsonConvert.DeserializeObject(json);
            }
            catch (Exception ex)
            {
                throw new JsonRpcException(JsonRpcErrorCode.ParseError, ex);
            }
        }

        private static JsonRpcObject ParseObject(JToken token)
        {
            try
            {
                if (token == null)
                    throw new ArgumentNullException(nameof(token));

                JsonRpcObject obj = null;
                if (token.Contains("method"))
                {
                    obj = new JsonRpcRequest
                    {
                        JsonRpc = (string)token["jsonrpc"],
                        Method = (string)token["method"],
                        Params = ParseParams(token["params"]),
                        Id = (string)token["id"],
                    };
                }
                else if (token.Contains("result") || token.Contains("error"))
                {
                    obj = new JsonRpcResponse
                    {
                        JsonRpc = (string)token["jsonrpc"],
                        Result = ParseError(token["result"]),
                        Error = ParseError(token["error"]),
                        Id = (string)token["id"],
                    };
                }
                else
                {
                    obj = new JsonRpcResponse { JsonRpc = "2.0", Error = new JsonRpcError(JsonRpcErrorCode.InvalidRequest) };
                }

                if (obj.JsonRpc != "2.0")
                    throw new JsonRpcException(JsonRpcErrorCode.InvalidRequest, new Exception("The jsonrpc member MUST be exactly 2.0."));

                if (obj is JsonRpcResponse response)
                {
                    if (response.Result != null && response.Error != null)
                        throw new JsonRpcException(JsonRpcErrorCode.InvalidRequest, new Exception("Either the result member or error member MUST be included, but both members MUST NOT be included."));
                }

                return obj;
            }
            catch (JsonRpcException ex)
            {
                return new JsonRpcResponse { JsonRpc = "2.0", Error = ex.Error };
            }
            catch (Exception ex)
            {
                return new JsonRpcResponse { JsonRpc = "2.0", Error = new JsonRpcError(JsonRpcErrorCode.InternalError, ex) };
            }
        }

        public static JsonRpcParams ParseParams(JToken token)
        {
            // This member MAY be omitted.
            if (token == null)
                return null;

            return new JsonRpcParams();
        }

        public static object ParseResult(JToken token)
        {
            return null;
        }

        public static JsonRpcError ParseError(JToken token)
        {
            return null;
        }
    }
}
