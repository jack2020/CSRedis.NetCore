using CSRedis.NetCore.Internal;
using CSRedis.NetCore.Internal.Fakes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using CSRedis.NetCore;
using Xunit;

namespace CSRedis.Tests
{
    
    public class HyperLogLogTests
    {
        [Fact]
        public void PfAddTest()
        {
            using (var mock = new FakeRedisSocket(":1\r\n"))
            using (var redis = new RedisClient(mock, new DnsEndPoint("fakehost", 9999)))
            {
                Assert.True(redis.PfAdd("test", "test1", "test2"));
                Assert.Equal("*4\r\n$5\r\nPFADD\r\n$4\r\ntest\r\n$5\r\ntest1\r\n$5\r\ntest2\r\n", mock.GetMessage());
            }
        }

        [Fact]
        public void PfCountTest()
        {
            using (var mock = new FakeRedisSocket(":2\r\n"))
            using (var redis = new RedisClient(mock, new DnsEndPoint("fakehost", 9999)))
            {
                Assert.Equal(2, redis.PfCount("test1", "test2"));
                Assert.Equal("*3\r\n$7\r\nPFCOUNT\r\n$5\r\ntest1\r\n$5\r\ntest2\r\n", mock.GetMessage());
            }
        }

        [Fact]
        public void PfMergeTest()
        {
            using (var mock = new FakeRedisSocket("+OK\r\n"))
            using (var redis = new RedisClient(mock, new DnsEndPoint("fakehost", 9999)))
            {
                Assert.Equal("OK", redis.PfMerge("destination", "source1", "source2"));
                Assert.Equal("*4\r\n$7\r\nPFMERGE\r\n$11\r\ndestination\r\n$7\r\nsource1\r\n$7\r\nsource2\r\n", mock.GetMessage());
            }
        }
    }
}
