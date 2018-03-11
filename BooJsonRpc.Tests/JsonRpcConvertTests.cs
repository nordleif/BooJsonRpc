using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Newtonsoft.Json;

namespace BooJsonRpc.Tests
{
    [TestFixture]
    public class JsonRpcConvertTests
    {
        [TestCase("{'jsonrpc': '2.0', 'method': 'subtract', 'params': [42, 23], 'id': 1}", "subtract", new int[] { 42, 23 }, "1")]
        [TestCase("{'jsonrpc': '2.0', 'method': 'subtract', 'params': [23, 42], 'id': 2}", "subtract", new int[] { 23, 42 }, "2")]
        [TestCase("{'jsonrpc': '2.0', 'method': 'update', 'params': [1,2,3,4,5]}", "update", new int[] { 1, 2, 3, 4, 5 }, null)]
        [TestCase("{'jsonrpc': '2.0', 'method': 'foobar', 'id': '1'}", "foobar", null, "1")]
        [Test]
        public void JsonRpcConvert_DeserializeRequest_PositionalParameters(string json, string method, object @params, string id)
        {
            var request = (JsonRpcRequest)JsonRpcConvert.Deserialize(json).First();
            Assert.AreEqual("2.0", request.JsonRpc);
            Assert.AreEqual(method, request.Method);
            AssertAreEqualJson(@params, request.Params);
            Assert.AreEqual(id, request.Id);
        }

        [Test]
        public void JsonRpcConvert_DeserializeRequest_NamedParameter()
        {
            var json = "{'jsonrpc': '2.0', 'method': 'subtract', 'params': {'subtrahend': 23, 'minuend': 42}, 'id': 3}";
            var request = (JsonRpcRequest)JsonRpcConvert.Deserialize(json).First();
            Assert.AreEqual("2.0", request.JsonRpc);
            Assert.AreEqual("subtract", request.Method);
            AssertAreEqualJson(new { subtrahend = 23, minuend = 42 }, request.Params);
            Assert.AreEqual("3", request.Id);
        }

        [TestCase("{'jsonrpc': '2.0', 'method': 'foobar, 'params': 'bar', 'baz]")]
        [TestCase("[ {'jsonrpc': '2.0', 'method': 'sum', 'params': [1,2,4], 'id': '1'}, {'jsonrpc': '2.0', 'method' ]")]
        [Test]
        public void JsonRpcConvert_DeserializeInvalidJson(string json)
        {
            var error = (JsonRpcError)JsonRpcConvert.Deserialize(json).First();
            Assert.AreEqual(JsonRpcErrorCode.ParseError.Code, error.Code);
            Assert.AreEqual(JsonRpcErrorCode.ParseError.Message, error.Message);
            Assert.IsNotNull(error.Data);
        }

        [Test]
        public void JsonRpcConvert_DeserializeRequest_Batch()
        {
            var json = "[ {'jsonrpc': '2.0', 'method': 'sum', 'params': [1,2,4], 'id': '1'}, {'jsonrpc': '2.0', 'method': 'notify_hello', 'params': [7] }, {'jsonrpc': '2.0', 'method': 'subtract', 'params': [42,23], 'id': '2'}, {'foo': 'boo'}, {'jsonrpc': '2.0', 'method': 'foo.get', 'params': {'name': 'myself'}, 'id': '5'}, {'jsonrpc': '2.0', 'method': 'get_data', 'id': '9'} ]";
            var items = JsonRpcConvert.Deserialize(json).ToArray();
            Assert.AreEqual(6, items.Length);
            Assert.IsInstanceOf<JsonRpcRequest>(items[0]);
            Assert.IsInstanceOf<JsonRpcRequest>(items[1]);
            Assert.IsInstanceOf<JsonRpcRequest>(items[2]);
            Assert.IsInstanceOf<JsonRpcError>(items[3]);
            Assert.IsInstanceOf<JsonRpcRequest>(items[4]);
            Assert.IsInstanceOf<JsonRpcRequest>(items[5]);
        }

        [Test]
        public void JsonRpcConvert_DeserializeRequest_BatchAllNotifications()
        {
            var json = "[ {'jsonrpc': '2.0', 'method': 'notify_sum', 'params': [1,2,4] }, {'jsonrpc': '2.0', 'method': 'notify_hello', 'params': [7] } ]";
            var items = JsonRpcConvert.Deserialize(json).ToArray();
            Assert.AreEqual(2, items.Length);
            Assert.IsInstanceOf<JsonRpcRequest>(items[0]);
            Assert.IsInstanceOf<JsonRpcRequest>(items[1]);
        }

        [Test]
        public void JsonRpcConvert_Deserialize_EmptyArray()
        {
            var json = "[]";
            var error = (JsonRpcError)JsonRpcConvert.Deserialize(json).First();
            Assert.AreEqual(JsonRpcErrorCode.InvalidRequest.Code, error.Code);
            Assert.AreEqual(JsonRpcErrorCode.InvalidRequest.Message, error.Message);
            Assert.IsNull(error.Data);
        }

        [Test]
        public void JsonRpcConvert_Deserialize_InvalidBatchNotEmpty()
        {
            var json = "[1]";
            var error = (JsonRpcError)JsonRpcConvert.Deserialize(json).First();
            Assert.AreEqual(JsonRpcErrorCode.InvalidRequest.Code, error.Code);
            Assert.AreEqual(JsonRpcErrorCode.InvalidRequest.Message, error.Message);
            Assert.IsNull(error.Data);
        }

        [Test]
        public void JsonRpcConvert_Deserialize_InvalidBatch()
        {
            var json = "[1,2,3]";
            var items = JsonRpcConvert.Deserialize(json);
            Assert.AreEqual(3, items.Count());
            foreach (var item in items)
            {
                var error = (JsonRpcError)item;
                Assert.AreEqual(JsonRpcErrorCode.InvalidRequest.Code, error.Code);
                Assert.AreEqual(JsonRpcErrorCode.InvalidRequest.Message, error.Message);
                Assert.IsNull(error.Data);
            }
        }

        [TestCase("{'jsonrpc': '2.0', 'result': 19, 'id': 1}", 19, null, null, "1")]
        [TestCase("{'jsonrpc': '2.0', 'error': {'code': -32601, 'message': 'Method not found'}, 'id': '1'}", null, -32601, "Method not found", "1")]
        [Test]
        public void JsonRpcConvert_DeserializeResponse(string json, object result, int? errorNumber, string errorMessage, string id)
        {
            var response = (JsonRpcResponse)JsonRpcConvert.Deserialize(json).First();
            AssertAreEqualJson(result, response.Result);
            Assert.AreEqual(errorNumber, response.Error?.Code);
            Assert.AreEqual(errorMessage, response.Error?.Message);
        }
        
        private void AssertAreEqualJson(object expected, object actual)
        {
            var expectedJson = JsonConvert.SerializeObject(expected);
            var actualJson = actual is string ? actual : JsonConvert.SerializeObject(actual);
            Assert.AreEqual(expectedJson, actualJson);
        }
    }
}
