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
    
    public class ScriptingTests
    {
        [Fact]
        public void EvalTest()
        {
            using (var mock = new FakeRedisSocket("*4\r\n$4\r\nkey1\r\n$4\r\nkey2\r\n$5\r\nfirst\r\n$6\r\nsecond\r\n"))
            using (var redis = new RedisClient(mock, new DnsEndPoint("fakehost", 9999)))
            {
                var response = redis.Eval("return {KEYS[1],KEYS[2],ARGV[1],ARGV[2]}", new[] { "key1", "key2" }, "first", "second");
                Assert.True(response is object[]);
                Assert.Equal(4, (response as object[]).Length);
                Assert.Equal("key1", (response as object[])[0]);
                Assert.Equal("key2", (response as object[])[1]);
                Assert.Equal("first", (response as object[])[2]);
                Assert.Equal("second", (response as object[])[3]);
                Assert.Equal("*7\r\n$4\r\nEVAL\r\n$40\r\nreturn {KEYS[1],KEYS[2],ARGV[1],ARGV[2]}\r\n$1\r\n2\r\n$4\r\nkey1\r\n$4\r\nkey2\r\n$5\r\nfirst\r\n$6\r\nsecond\r\n", mock.GetMessage());
            }
        }

        [Fact]
        public void EvalSHATest()
        {
            using (var mock = new FakeRedisSocket("*4\r\n$4\r\nkey1\r\n$4\r\nkey2\r\n$5\r\nfirst\r\n$6\r\nsecond\r\n"))
            using (var redis = new RedisClient(mock, new DnsEndPoint("fakehost", 9999)))
            {
                var response = redis.EvalSHA("checksum", new[] { "key1", "key2" }, "first", "second");
                Assert.True(response is object[]);
                Assert.Equal(4, (response as object[]).Length);
                Assert.Equal("key1", (response as object[])[0]);
                Assert.Equal("key2", (response as object[])[1]);
                Assert.Equal("first", (response as object[])[2]);
                Assert.Equal("second", (response as object[])[3]);
                Assert.Equal("*7\r\n$7\r\nEVALSHA\r\n$8\r\nchecksum\r\n$1\r\n2\r\n$4\r\nkey1\r\n$4\r\nkey2\r\n$5\r\nfirst\r\n$6\r\nsecond\r\n", mock.GetMessage());
            }
        }

        [Fact]
        public void ScriptExistsTests()
        {
            using (var mock = new FakeRedisSocket("*2\r\n:1\r\n:0\r\n"))
            using (var redis = new RedisClient(mock, new DnsEndPoint("fakehost", 9999)))
            {
                var response = redis.ScriptExists("checksum1", "checksum2");
                Assert.Equal(2, response.Length);
                Assert.True(response[0]);
                Assert.False(response[1]);

                Assert.Equal("*4\r\n$6\r\nSCRIPT\r\n$6\r\nEXISTS\r\n$9\r\nchecksum1\r\n$9\r\nchecksum2\r\n", mock.GetMessage());
            }
        }

        [Fact]
        public void ScriptFlushTest()
        {
            using (var mock = new FakeRedisSocket("+OK\r\n"))
            using (var redis = new RedisClient(mock, new DnsEndPoint("fakehost", 9999)))
            {
                Assert.Equal("OK", redis.ScriptFlush());
                Assert.Equal("*2\r\n$6\r\nSCRIPT\r\n$5\r\nFLUSH\r\n", mock.GetMessage());
            }
        }

        [Fact]
        public void ScriptKillTest()
        {
            using (var mock = new FakeRedisSocket("+OK\r\n"))
            using (var redis = new RedisClient(mock, new DnsEndPoint("fakehost", 9999)))
            {
                Assert.Equal("OK", redis.ScriptKill());
                Assert.Equal("*2\r\n$6\r\nSCRIPT\r\n$4\r\nKILL\r\n", mock.GetMessage());
            }
        }

        [Fact]
        public void ScriptLoadTest()
        {
            using (var mock = new FakeRedisSocket("$8\r\nchecksum\r\n"))
            using (var redis = new RedisClient(mock, new DnsEndPoint("fakehost", 9999)))
            {
                Assert.Equal("checksum", redis.ScriptLoad("return 1"));
                Assert.Equal("*3\r\n$6\r\nSCRIPT\r\n$4\r\nLOAD\r\n$8\r\nreturn 1\r\n", mock.GetMessage());
            }
        }
    }
}
