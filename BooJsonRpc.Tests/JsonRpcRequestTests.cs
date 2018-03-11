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
        [Test]
        public void JsonRpcRequest_Invoke_PositionalParameters()
        {
            var obj = new MockObject();
            var json = "{'jsonrpc': '2.0', 'method': 'subtract', 'params': [42, 23], 'id': 1}";
            var request = (JsonRpcRequest)JsonRpcConvert.Deserialize(json).First();
            var result = request.Invoke(obj);
            Assert.AreEqual(19, result);
        }

        [Test]
        public void JsonRpcRequest_Invoke_NamedParameters()
        {
            var obj = new MockObject();
            var json = "{'jsonrpc': '2.0', 'method': 'subtract', 'params': {'subtrahend': 23, 'minuend': 42}, 'id': 3}";
            var request = (JsonRpcRequest)JsonRpcConvert.Deserialize(json).First();
            var result = request.Invoke(obj);
            Assert.AreEqual(19, result);
        }
    }
}
