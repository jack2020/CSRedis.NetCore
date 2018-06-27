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
    
    public class PubSubTests
    {
        /*[TestMethod, TestCategory("PubSub")]
        public void PSubscriptionTest()
        {
            using (var mock = new FakeRedisSocket(true,
                "*3\r\n$10\r\npsubscribe\r\n$2\r\nf*\r\n:1\r\n"
                    + "*3\r\n$10\r\npsubscribe\r\n$2\r\ns*\r\n:2\r\n"
                    + "*4\r\n$8\r\npmessage\r\n$2\r\nf*\r\n$5\r\nfirst\r\n$5\r\nHello\r\n",
                "*3\r\n$12\r\npunsubscribe\r\n$2\r\ns*\r\n:1\r\n*3\r\n$12\r\npunsubscribe\r\n$2\r\nf*\r\n:0\r\n"))
            using (var redis = new RedisClient(mock, new DnsEndPoint("fakehost", 9999)))
            {
                var changes = new List<RedisSubscriptionChannel>();
                var messages = new List<RedisSubscriptionMessage>();
                redis.SubscriptionChanged += (s,a) => changes.Add(a.Response);
                redis.SubscriptionReceived += (s, a) => messages.Add(a.Message);
                Task.Delay(500)
                    .ContinueWith(t => redis.PUnsubscribe())
                    .ContinueWith(t =>
                    {
                        Assert.Equal(4, changes.Count);
                        Assert.Equal("f*", changes[0].Pattern);
                        Assert.Equal(1, changes[0].Count);
                        Assert.IsNull(changes[0].Channel);
                        Assert.Equal("psubscribe", changes[0].Type);

                        Assert.Equal("s*", changes[1].Pattern);
                        Assert.Equal(2, changes[1].Count);
                        Assert.IsNull(changes[1].Channel);
                        Assert.Equal("psubscribe", changes[1].Type);

                        Assert.Equal("s*", changes[2].Pattern);
                        Assert.Equal(1, changes[2].Count);
                        Assert.IsNull(changes[2].Channel);
                        Assert.Equal("punsubscribe", changes[2].Type);

                        Assert.Equal("f*", changes[3].Pattern);
                        Assert.Equal(0, changes[3].Count);
                        Assert.IsNull(changes[3].Channel);
                        Assert.Equal("punsubscribe", changes[3].Type);

                        Assert.Equal(1, messages.Count);
                        Assert.Equal("f*", messages[0].Pattern);
                        Assert.Equal("first", messages[0].Channel);
                        Assert.Equal("Hello", messages[0].Body);
                        Assert.Equal("pmessage", messages[0].Type);
                    });
                redis.PSubscribe("f*", "s*");
            }
        }*/

       [Fact]
        public void PublishTest()
        {
            using (var mock = new FakeRedisSocket(":3\r\n"))
            using (var redis = new RedisClient(mock, new DnsEndPoint("fakehost", 9999)))
            {
                Assert.Equal(3, redis.Publish("test", "message"));
                Assert.Equal("*3\r\n$7\r\nPUBLISH\r\n$4\r\ntest\r\n$7\r\nmessage\r\n", mock.GetMessage());
            }
        }

       [Fact]
        public void PubSubChannelsTest()
        {
            using (var mock = new FakeRedisSocket("*2\r\n$5\r\ntest1\r\n$5\r\ntest2\r\n"))
            using (var redis = new RedisClient(mock, new DnsEndPoint("fakehost", 9999)))
            {
                var response = redis.PubSubChannels("pattern");
                Assert.Equal(2, response.Length);
                Assert.Equal("test1", response[0]);
                Assert.Equal("test2", response[1]);
                Assert.Equal("*3\r\n$6\r\nPUBSUB\r\n$8\r\nCHANNELS\r\n$7\r\npattern\r\n", mock.GetMessage());
            }
        }

       [Fact]
        public void PubSubNumSubTest()
        {
            using (var mock = new FakeRedisSocket("*4\r\n$5\r\ntest1\r\n:1\r\n$5\r\ntest2\r\n:5\r\n"))
            using (var redis = new RedisClient(mock, new DnsEndPoint("fakehost", 9999)))
            {
                var response = redis.PubSubNumSub("channel1", "channel2");
                Assert.Equal(2, response.Length);
                Assert.Equal("test1", response[0].Item1);
                Assert.Equal(1, response[0].Item2);
                Assert.Equal("test2", response[1].Item1);
                Assert.Equal(5, response[1].Item2);
                Assert.Equal("*4\r\n$6\r\nPUBSUB\r\n$6\r\nNUMSUB\r\n$8\r\nchannel1\r\n$8\r\nchannel2\r\n", mock.GetMessage());
            }
        }

       [Fact]
        public void PubSubNumPatTest()
        {
            using (var mock = new FakeRedisSocket(":3\r\n"))
            using (var redis = new RedisClient(mock, new DnsEndPoint("fakehost", 9999)))
            {
                Assert.Equal(3, redis.PubSubNumPat());
                Assert.Equal("*2\r\n$6\r\nPUBSUB\r\n$6\r\nNUMPAT\r\n", mock.GetMessage());
            }
        }

        /*[TestMethod, TestCategory("PubSub")]
        public void SubscriptionTest()
        {
            using (var mock = new FakeRedisSocket(true,
                "*3\r\n$9\r\nsubscribe\r\n$5\r\nfirst\r\n:1\r\n"
                    + "*3\r\n$9\r\nsubscribe\r\n$6\r\nsecond\r\n:2\r\n"
                    + "*3\r\n$7\r\nmessage\r\n$5\r\nfirst\r\n$5\r\nHello\r\n",
                "*3\r\n$11\r\nunsubscribe\r\n$6\r\nsecond\r\n:1\r\n*3\r\n$11\r\nunsubscribe\r\n$5\r\nfirst\r\n:0\r\n"))
            using (var redis = new RedisClient(mock, new DnsEndPoint("fakehost", 9999)))
            {
                var changes = new List<RedisSubscriptionChannel>();
                var messages = new List<RedisSubscriptionMessage>();
                redis.SubscriptionChanged += (s, a) => changes.Add(a.Response);
                redis.SubscriptionReceived += (s, a) => messages.Add(a.Message);
                Task.Delay(500)
                    .ContinueWith(t => redis.Unsubscribe())
                    .ContinueWith(t =>
                {
                    Assert.Equal(4, changes.Count);
                    Assert.Equal("first", changes[0].Channel);
                    Assert.Equal(1, changes[0].Count);
                    Assert.IsNull(changes[0].Pattern);
                    Assert.Equal("subscribe", changes[0].Type);

                    Assert.Equal("second", changes[1].Channel);
                    Assert.Equal(2, changes[1].Count);
                    Assert.IsNull(changes[1].Pattern);
                    Assert.Equal("subscribe", changes[1].Type);

                    Assert.Equal("second", changes[2].Channel);
                    Assert.Equal(1, changes[2].Count);
                    Assert.IsNull(changes[2].Pattern);
                    Assert.Equal("unsubscribe", changes[2].Type);

                    Assert.Equal("first", changes[3].Channel);
                    Assert.Equal(0, changes[3].Count);
                    Assert.IsNull(changes[3].Pattern);
                    Assert.Equal("unsubscribe", changes[3].Type);

                    Assert.Equal(1, messages.Count);
                    Assert.IsNull(messages[0].Pattern);
                    Assert.Equal("first", messages[0].Channel);
                    Assert.Equal("Hello", messages[0].Body);
                    Assert.Equal("message", messages[0].Type);
                });
                redis.Subscribe("first", "second");
            }
        }*/
    }
}
