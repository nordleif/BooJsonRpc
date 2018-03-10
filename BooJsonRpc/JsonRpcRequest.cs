using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BooJsonRpc
{
    /// <summary>
    /// A rpc call is represented by sending a Request object to a Server.
    /// </summary>
    public class JsonRpcRequest
    {
        #region Static Members

        public static string Process(string json, Func<JsonRpcRequest, object> func = null)
        {
            if (string.IsNullOrWhiteSpace(json))
                throw new ArgumentNullException(nameof(json));

            var responses = new List<JsonRpcResponse>();

            var obj = JsonRpcReader.ReadJson(json);
            if (obj is JArray jArray)
            {
                foreach (var token in jArray)
                {
                    var response = Process(token, func);
                    if (response != null)
                        responses.Add(response);
                }
            }
            else if (obj is JToken token)
            {
                var response = Process(token, func);
                if (response != null)
                    responses.Add(response);
            }

            return JsonRpcWriter.Write(responses);
        }

        private static JsonRpcResponse Process(JToken token, Func<JsonRpcRequest, object> func = null)
        {
            var response = new JsonRpcResponse { JsonRpc = "2.0" };
            try
            {
                var request = JsonRpcReader.ReadRequest(token);
                response.Id = request.Id;
                response.Result = func?.Invoke(request);
            }
            catch (JsonRpcException ex)
            {
                response.Error = ex.Error;
            }
            catch (Exception)
            {
                response.Error = JsonRpcError.InternalError;
            }

            // The Server MUST NOT reply to a Notification, including those that are within a batch request.
            if (response.Id == null)
                return null;

            return response;
        }

        #endregion

        public JsonRpcRequest()
        {

        }

        /// <summary>
        /// A String specifying the version of the JSON-RPC protocol. MUST be exactly "2.0".
        /// </summary>
        public string JsonRpc { get; set; }

        /// <summary>
        /// A String containing the name of the method to be invoked. 
        /// Method names that begin with the word rpc followed by a period character (U+002E or ASCII 46) 
        /// are reserved for rpc-internal methods and extensions and MUST NOT be used for anything else.
        /// </summary>
        public string Method { get; set; }

        /// <summary>
        /// A Structured value that holds the parameter values to be used during the invocation of the method. 
        /// This member MAY be omitted.
        /// </summary>
        public JsonRpcParams Params { get; set; }

        /// <summary>
        /// An identifier established by the Client that MUST contain a String, Number, or NULL value if included. 
        /// If it is not included it is assumed to be a notification. 
        /// The value SHOULD normally not be Null [1] and Numbers SHOULD NOT contain fractional parts
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Invokes the method or constructor represented by the current instance, using the specified parameters.
        /// </summary>
        public object Invoke(object obj)
        {
            if (obj == null)
                throw new ArgumentNullException(nameof(obj));

            var type = obj.GetType();
            var method = type.GetMethod(Method);
            if (method == null)
                throw new JsonRpcException(JsonRpcError.MethodNotFound);

            var parameters = method.GetParameters();
            if (!parameters.Any() && Params != null)
                throw new JsonRpcException(JsonRpcError.InvalidParams);

            return null;
        }
    }
}
