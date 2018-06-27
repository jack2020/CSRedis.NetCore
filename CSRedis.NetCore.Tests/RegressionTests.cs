using System;
using CSRedis.NetCore.Internal;
using CSRedis.NetCore.Internal.Fakes;
using System.Net;
using CSRedis.NetCore;
using Xunit;

namespace CSRedis.Tests
{
    
    public class RegressionTests
    {
        [Fact]
        public void SetUTF8Test()
        {
            using (var mock = new FakeRedisSocket("+OK\r\n", "+OK\r\n"))
            using (var redis = new RedisClient(mock, new DnsEndPoint("fakehost", 9999)))
            {
                Assert.Equal("OK", redis.Set("test", "é"));
                Assert.Equal("*3\r\n$3\r\nSET\r\n$4\r\ntest\r\n$2\r\né\r\n", mock.GetMessage());

                Assert.Equal("OK", redis.SetAsync("test", "é").Result);
                Assert.Equal("*3\r\n$3\r\nSET\r\n$4\r\ntest\r\n$2\r\né\r\n", mock.GetMessage());
            }
        }
    }
}
