using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace BooJsonRpc.Tests
{
    [TestFixture]
    public class JsonRpcRequestTests
    {
        [TestCase("{'jsonrpc': '2.0', 'method': 'subtract', 'params': [42, 23], 'id': 1}", "{'jsonrpc': '2.0', 'result': 19, 'id': 1}")]
        [TestCase("{'jsonrpc': '2.0', 'method': 'subtract', 'params': [23, 42], 'id': 2}", "{'jsonrpc': '2.0', 'result': -19, 'id': 2}")]
        [Test]
        public void JsonRpcRequest_ProcessRpcCallWithPositionalParameters(string request, string expectedResponse)
        {
            //var obj = new MockObject();
            //var actualResponse = JsonRpcRequest.Process(request, r => r.Invoke(obj));
            //Assert.AreEqual(actualResponse, expectedResponse);
        }

        [TestCase("{'jsonrpc': '2.0', 'method': 'sum', 'params': [1,2,4], 'id': '1'}, {'jsonrpc': '2.0', 'method': 'notify_hello', 'params': [7] }, {'jsonrpc': '2.0', 'method': 'subtract', 'params': [42,23], 'id': '2'}, {'foo': 'boo'}, {'jsonrpc': '2.0', 'method': 'foo.get', 'params': {'name': 'myself'}, 'id': '5'}, {'jsonrpc': '2.0', 'method': 'get_data', 'id': '9'}")]
        [Test]
        public void JsonRpcRequest_ProcessRpcCallBatch(string json)
        {
            //var responses = JsonRpcRequest.Process(json);
            //Assert.AreEqual(responses.Count(), 5);
        }
    }
}
