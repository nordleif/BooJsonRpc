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
        public static IEnumerable<JsonRpcObject> Deserialize(string json)
        {
            if (string.IsNullOrWhiteSpace(json))
                throw new ArgumentNullException(nameof(json));

            try
            {
                var obj = ReadJson(json);
                if (obj is JsonRpcError error)
                    return new[] { error };

                if (obj is JArray jArray)
                {
                    if (!jArray.Any())
                        return new[] { new JsonRpcError(JsonRpcErrorCode.InvalidRequest) };

                    var items = new List<JsonRpcObject>();
                    foreach (var item in jArray)
                        items.Add(ReadObject(item));
                    return items;
                }
                else
                {
                    return new[] { ReadObject((JToken)obj) };
                }
            }
            catch(Exception ex)
            {
                return new[] { new JsonRpcError(JsonRpcErrorCode.InternalError, ex) };
            }
        }

        public static string Serialize(IEnumerable<JsonRpcObject> items)
        {
            if (items == null)
                throw new ArgumentNullException(nameof(items));

            var json = string.Empty;
            var length = items.Count();
            if (length == 0)
                json = JsonConvert.SerializeObject(new object());
            else if (length == 1)
                json = JsonConvert.SerializeObject(Write(items.First()));
            else
            {
                var jArray = new JArray();
                foreach (var item in items)
                    jArray.Add(Write(item));
                json = JsonConvert.SerializeObject(jArray); 
            }
            return json;
        }

        private static object ReadJson(string json)
        {
            if (string.IsNullOrWhiteSpace(json))
                throw new ArgumentNullException(nameof(json));

            try
            {
                return JsonConvert.DeserializeObject(json);
            }
            catch (Exception ex)
            {
                return new JsonRpcError(JsonRpcErrorCode.ParseError, ex);
            }
        }

        private static JsonRpcObject ReadObject(JToken token)
        {
            try
            {
                if (token == null)
                    throw new ArgumentNullException(nameof(token));

                if (!token.Any())
                    return new JsonRpcError(JsonRpcErrorCode.InvalidRequest);

                JsonRpcObject obj = null;
                if (token["method"] != null)
                {
                    obj = new JsonRpcRequest
                    {
                        JsonRpc = (string)token["jsonrpc"],
                        Method = (string)token["method"],
                        Params = token["params"],
                        Id = (string)token["id"],
                    };
                }
                else if (token["result"] != null || token["error"] != null)
                {
                    obj = new JsonRpcResponse
                    {
                        JsonRpc = (string)token["jsonrpc"],
                        Result = token["result"],
                        Error = token["error"] != null ? ReadError(token["error"]) : null,
                        Id = (string)token["id"],
                    };
                }
                else
                {
                    obj = new JsonRpcError(JsonRpcErrorCode.InvalidRequest);
                }

                //if (obj.JsonRpc != "2.0")
                //    throw new JsonRpcException(JsonRpcErrorCode.InvalidRequest, new Exception("The jsonrpc member MUST be exactly 2.0."));

                //if (obj is JsonRpcResponse response)
                //{
                //    if (response.Result != null && response.Error != null)
                //        throw new JsonRpcException(JsonRpcErrorCode.InvalidRequest, new Exception("Either the result member or error member MUST be included, but both members MUST NOT be included."));
                //}

                return obj;
            }
            catch (Exception ex)
            {
                return new JsonRpcError(JsonRpcErrorCode.InternalError, ex);
            }
        }

        private static JsonRpcError ReadError(JToken token)
        {
            if (token == null)
                throw new ArgumentNullException(nameof(token));

            var errorCode = new JsonRpcErrorCode {
                Code = (int)token["code"],
                Message = (string)token["message"],
            };
            var data = (string)token["data"];
            return new JsonRpcError(errorCode, data);
        }

        private static JToken Write(JsonRpcObject obj)
        {
            if (obj == null)
                throw new ArgumentNullException(nameof(obj));
            
            var jObject = new JObject();
            if (obj is JsonRpcRequest request)
            {
                if (!string.IsNullOrWhiteSpace(request.JsonRpc))
                    jObject["jsonrpc"] = request.JsonRpc;
                if (!string.IsNullOrWhiteSpace(request.Method))
                    jObject["method"] = request.Method;
                if (request.Params != null)
                    jObject["params"] = request.Params;
                if (!string.IsNullOrWhiteSpace(request.Id))
                    jObject["id"] = request.Id;
            }
            else if (obj is JsonRpcResponse response)
            {
                if (!string.IsNullOrWhiteSpace(response.JsonRpc))
                    jObject["jsonrpc"] = response.JsonRpc;
                if (response.Result != null || (response.Result == null && response.Error == null))
                    jObject["result"] = response.Result;
                if (response.Error != null)
                    jObject["error"] = Write(response.Error);
                if (!string.IsNullOrWhiteSpace(response.Id))
                    jObject["id"] = response.Id;
            }
            else if (obj is JsonRpcError error)
            {
                jObject["code"] = error.Code;
                jObject["message"] = error.Message;
                if (!string.IsNullOrWhiteSpace(error.Data))
                    jObject["data"] = error.Data;
            }
            else
            {
                throw new NotSupportedException();
            }
            return jObject;
        }
    }
}
