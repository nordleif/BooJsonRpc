using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BooJsonRpc.Tests
{
    public class MockObject
    {
        public MockObject()
        {

        }

        public int subtract(int subtrahend, int minuend)
        {
            return subtrahend - minuend;
        }
    }
}
