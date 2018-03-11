using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace BooJsonRpc
{
    /// <summary>
    /// A rpc call is represented by sending a Request object to a Server.
    /// </summary>
    public class JsonRpcRequest : JsonRpcObject
    {
        #region Static Members

        public static IEnumerable<JsonRpcObject> Process(IEnumerable<JsonRpcObject> items, Func<JsonRpcRequest, object> func = null)
        {
            if (items == null)
                throw new ArgumentNullException(nameof(items));

            var result = new List<JsonRpcObject>();
            foreach (var item in items)
            {
                if (item is JsonRpcRequest request)
                {
                    var response = new JsonRpcResponse { JsonRpc = "2.0", Id = request.Id };
                    try
                    {
                        response.Result = func?.Invoke(request);
                    }
                    catch (JsonRpcException ex)
                    {
                        response.Error = ex.Error;
                    }
                    catch (Exception ex)
                    {
                        response.Error = new JsonRpcError(JsonRpcErrorCode.InternalError, ex);
                    }

                    // A Notification is a Request object without an "id" member. 
                    // The Server MUST NOT reply to a Notification, including those that are within a batch request.
                    if (!string.IsNullOrWhiteSpace(response.Id))
                        result.Add(response);       
                }
                else if (item is JsonRpcError error)
                {
                    result.Add(new JsonRpcResponse { JsonRpc = "2.0", Error = error });
                }
                else
                {
                    result.Add(new JsonRpcResponse { JsonRpc = "2.0", Error = new JsonRpcError(JsonRpcErrorCode.InternalError) });
                }
            }
            return result;
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
        public dynamic Params { get; set; }

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
                throw new JsonRpcException(JsonRpcErrorCode.MethodNotFound);

            object result = null;
            var methodParameters = method.GetParameters();
            if (Params is JArray jArray)
            {
                var requestParameters = jArray.ToArray();
                if (methodParameters.Length != requestParameters.Length)
                    throw new JsonRpcException(JsonRpcErrorCode.InvalidParams);

                var invokeParameters = new object[methodParameters.Length];
                for(var i = 0; i < methodParameters.Length; i++)
                {
                    var methodParameter = methodParameters[i];
                    var requestParameter = requestParameters[i];
                    try
                    {
                        invokeParameters[i] = Convert.ChangeType(requestParameter, methodParameter.ParameterType);
                    }
                    catch(Exception ex)
                    {
                        throw new JsonRpcException(JsonRpcErrorCode.InvalidParams, ex);
                    }
                }

                result = method.Invoke(obj, invokeParameters);
            }
            else if (Params is JToken token)
            {
                var invokeParameters = new object[methodParameters.Length];
                for (var i = 0; i < methodParameters.Length; i++)
                {
                    var methodParameter = methodParameters[i];
                    var requestParameter = token[methodParameter.Name];
                    if (requestParameter == null)
                        continue;
                    try
                    {
                        invokeParameters[i] = Convert.ChangeType(requestParameter, methodParameter.ParameterType);
                    }
                    catch (Exception ex)
                    {
                        throw new JsonRpcException(JsonRpcErrorCode.InvalidParams, ex);
                    }
                }

                result = method.Invoke(obj, invokeParameters);
            }
            
            return result;
        }
    }
}
