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
        public void JsonRpcConvert_DeserializePositionalParameters(string json, string method, object @params, string id)
        {
            var request = (JsonRpcRequest)JsonRpcConvert.Deserialize(json).First();
            Assert.AreEqual("2.0", request.JsonRpc);
            Assert.AreEqual(method, request.Method);
            AssertAreEqualJson(@params, request.Params);
            Assert.AreEqual(id, request.Id);
        }

        [Test]
        public void JsonRpcConvert_DeserializeNamedParameters()
        {
            var json = "{'jsonrpc': '2.0', 'method': 'subtract', 'params': {'subtrahend': 23, 'minuend': 42}, 'id': 3}";
            var request = (JsonRpcRequest)JsonRpcConvert.Deserialize(json).First();
            Assert.AreEqual("2.0", request.JsonRpc);
            Assert.AreEqual("subtract", request.Method);
            AssertAreEqualJson(new { subtrahend = 23, minuend = 42 }, request.Params);
            Assert.AreEqual("3", request.Id);
        }

        private void AssertAreEqualJson(object expected, object actual)
        {
            var expectedJson = JsonConvert.SerializeObject(expected);
            var actualJson = actual is string ? actual : JsonConvert.SerializeObject(actual);
            Assert.AreEqual(expectedJson, actualJson);
        }


    }
}
