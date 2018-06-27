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
    
    public class SentinelTests
    {
        [Fact]
        public void PingTest()
        {
            TestSentinel(
                "+PONG\r\n",
                x => x.Ping(),
                x => x.PingAsync(),
                (m, r) =>
                {
                    Assert.Equal("PONG", r);
                    Assert.Equal(Compile("PING"), m.GetMessage());
                });
        }

        [Fact]
        public void MastersTest()
        {
            TestSentinel(
                "*1\r\n" + Compile(
                    "name", "mymaster",
                    "ip", "127.0.0.1",
                    "port", "6379",
                    "runid", "0e4c05a7b29fdb5dffa054f151bca9ed6a113c38",
                    "flags", "master",
                    "pending-commands", "0",
                    "last-ping-sent", "0",
                    "last-ping-reply", "50",
                    "last-ok-ping-reply", "50",
                    "down-after-milliseconds", "30000",
                    "info-refresh", "1913",
                    "role-reported", "master",
                    "role-reported-time", "1838781",
                    "config-epoch", "0",
                    "num-slaves", "2",
                    "num-other-sentinels", "1",
                    "quorum", "2",
                    "failover-timeout", "180000",
                    "parallel-syncs", "1"),
                x => x.Masters(),
                x => x.MastersAsync(),
                (m, r) =>
                {
                    Assert.Equal(1, r.Length);
                    Assert.Equal("mymaster", r[0].Name);
                    Assert.Equal("127.0.0.1", r[0].Ip);
                    Assert.Equal(6379, r[0].Port);
                    Assert.Equal("0e4c05a7b29fdb5dffa054f151bca9ed6a113c38", r[0].RunId);
                    Assert.Equal(1, r[0].Flags.Length);
                    Assert.Equal("master", r[0].Flags[0]);
                    Assert.Equal(0, r[0].PendingCommands);
                    Assert.Equal(0, r[0].LastPingSent);
                    Assert.Equal(50, r[0].LastPingReply);
                    Assert.Equal(50, r[0].LastOkPingReply);
                    Assert.Equal(30000, r[0].DownAfterMilliseconds);
                    Assert.Equal(1913, r[0].InfoRefresh);
                    Assert.Equal("master", r[0].RoleReported);
                    Assert.Equal(1838781, r[0].RoleReportedTime);
                    Assert.Equal(0, r[0].ConfigEpoch);
                    Assert.Equal(2, r[0].NumSlaves);
                    Assert.Equal(1, r[0].NumOtherSentinels);
                    Assert.Equal(2, r[0].Quorum);
                    Assert.Equal(180000, r[0].FailoverTimeout);
                    Assert.Equal(1, r[0].ParallelSyncs);
                    Assert.Equal(Compile("SENTINEL", "masters"), m.GetMessage());
                });
        }

        [Fact]
        public void MasterTest()
        {
            TestSentinel(
                Compile(
                    "name", "mymaster",
                    "ip", "127.0.0.1",
                    "port", "6379",
                    "runid", "0e4c05a7b29fdb5dffa054f151bca9ed6a113c38",
                    "flags", "master",
                    "pending-commands", "0",
                    "last-ping-sent", "0",
                    "last-ping-reply", "50",
                    "last-ok-ping-reply", "50",
                    "down-after-milliseconds", "30000",
                    "info-refresh", "1913",
                    "role-reported", "master",
                    "role-reported-time", "1838781",
                    "config-epoch", "0",
                    "num-slaves", "2",
                    "num-other-sentinels", "1",
                    "quorum", "2",
                    "failover-timeout", "180000",
                    "parallel-syncs", "1"),
                x => x.Master("mymaster"),
                x => x.MasterAsync("mymaster"),
                (m, r) =>
                {
                    Assert.Equal("mymaster", r.Name);
                    Assert.Equal("127.0.0.1", r.Ip);
                    Assert.Equal(6379, r.Port);
                    Assert.Equal("0e4c05a7b29fdb5dffa054f151bca9ed6a113c38", r.RunId);
                    Assert.Equal(1, r.Flags.Length);
                    Assert.Equal("master", r.Flags[0]);
                    Assert.Equal(0, r.PendingCommands);
                    Assert.Equal(0, r.LastPingSent);
                    Assert.Equal(50, r.LastPingReply);
                    Assert.Equal(50, r.LastOkPingReply);
                    Assert.Equal(30000, r.DownAfterMilliseconds);
                    Assert.Equal(1913, r.InfoRefresh);
                    Assert.Equal("master", r.RoleReported);
                    Assert.Equal(1838781, r.RoleReportedTime);
                    Assert.Equal(0, r.ConfigEpoch);
                    Assert.Equal(2, r.NumSlaves);
                    Assert.Equal(1, r.NumOtherSentinels);
                    Assert.Equal(2, r.Quorum);
                    Assert.Equal(180000, r.FailoverTimeout);
                    Assert.Equal(1, r.ParallelSyncs);
                    Assert.Equal(Compile("SENTINEL", "master", "mymaster"), m.GetMessage());
                });
        }

        [Fact]
        public void SlavesTest()
        {
            TestSentinel(
                "*1\r\n" + Compile(
                    "name", "127.0.0.1:7379",
                    "ip", "127.0.0.1",
                    "port", "7379",
                    "runid", "0e4c05a7b29fdb5dffa054f151bca9ed6a113c38",
                    "flags", "slave",
                    "pending-commands", "0",
                    "last-ping-sent", "0",
                    "last-ok-ping-reply", "50",
                    "last-ping-reply", "50",
                    "down-after-milliseconds", "1000",
                    "info-refresh", "9036",
                    "role-reported", "slave",
                    "role-reported-time", "4396444",
                    "master-link-down-time", "0",
                    "master-link-status", "ok",
                    "master-host", "127.0.0.1",
                    "master-port", "6379",
                    "slave-priority", "100",
                    "slave-repl-offset", "509813"),
                x => x.Slaves("mymaster"),
                x => x.SlavesAsync("mymaster"),
                (m, r) =>
                {
                    Assert.Equal(1, r.Length);
                    Assert.Equal("127.0.0.1:7379", r[0].Name);
                    Assert.Equal("127.0.0.1", r[0].Ip);
                    Assert.Equal(7379, r[0].Port);
                    Assert.Equal("0e4c05a7b29fdb5dffa054f151bca9ed6a113c38", r[0].RunId);
                    Assert.Equal(1, r[0].Flags.Length);
                    Assert.Equal("slave", r[0].Flags[0]);
                    Assert.Equal(0, r[0].PendingCommands);
                    Assert.Equal(50, r[0].LastPingReply);
                    Assert.Equal(50, r[0].LastOkPingReply);
                    Assert.Equal(1000, r[0].DownAfterMilliseconds);
                    Assert.Equal(9036, r[0].InfoRefresh);
                    Assert.Equal("slave", r[0].RoleReported);
                    Assert.Equal(4396444, r[0].RoleReportedTime);
                    Assert.Equal(0, r[0].MasterLinkDownTime);
                    Assert.Equal("ok", r[0].MasterLinkStatus);
                    Assert.Equal("127.0.0.1", r[0].MasterHost);
                    Assert.Equal(6379, r[0].MasterPort);
                    Assert.Equal(100, r[0].SlavePriority);
                    Assert.Equal(509813, r[0].SlaveReplOffset);
                    Assert.Equal(Compile("SENTINEL", "slaves", "mymaster"), m.GetMessage());
                });
        }

        [Fact]
        public void GetMasterAddrByNameTest()
        {
            TestSentinel(
                Compile("127.0.0.1", "6379"),
                x => x.GetMasterAddrByName("mymaster"),
                x => x.GetMasterAddrByNameAsync("mymaster"),
                (m, r) =>
                {
                    Assert.Equal("127.0.0.1", r.Item1);
                    Assert.Equal(6379, r.Item2);
                    Assert.Equal(Compile("SENTINEL", "get-master-addr-by-name", "mymaster"), m.GetMessage());
                });
        }

        [Fact]
        public void IsMasterDownByAddrTest()
        {
            TestSentinel(
                Compile(0, "*", 0),
                x => x.IsMasterDownByAddr("127.0.0.1", 6379, 123, "abc"),
                x => x.IsMasterDownByAddrAsync("127.0.0.1", 6379, 123, "abc"),
                (m, r) =>
                {
                    Assert.Equal(0, r.DownState);
                    Assert.Equal("*", r.Leader);
                    Assert.Equal(0, r.VoteEpoch);
                    Assert.Equal(Compile("SENTINEL", "is-master-down-by-addr", "127.0.0.1", "6379", "123", "abc"), m.GetMessage());
                });
        }

        [Fact]
        public void ResetTest()
        {
            TestSentinel(
                ":10\r\n",
                x => x.Reset("pattern*"),
                x => x.ResetAsync("pattern*"),
                (m, r) =>
                {
                    Assert.Equal(10, r);
                    Assert.Equal(Compile("SENTINEL", "reset", "pattern*"), m.GetMessage());
                });
        }

        [Fact]
        public void FailoverTest()
        {
            TestSentinel(
                "+OK\r\n",
                x => x.Failover("mymaster"),
                x => x.FailoverAsync("mymaster"),
                (m, r) =>
                {
                    Assert.Equal("OK", r);
                    Assert.Equal(Compile("SENTINEL", "failover", "mymaster"), m.GetMessage());
                });
        }

        [Fact]
        public void MonitorTest()
        {
            TestSentinel(
                "+OK\r\n",
                x => x.Monitor("mymaster", 6379, 2),
                x => x.MonitorAsync("mymaster", 6379, 2),
                (m, r) =>
                {
                    Assert.Equal("OK", r);
                    Assert.Equal(Compile("SENTINEL", "MONITOR", "mymaster", "6379", "2"), m.GetMessage());
                });
        }

        [Fact]
        public void RemoveTest()
        {
            TestSentinel(
                "+OK\r\n",
                x => x.Remove("mymaster"),
                x => x.RemoveAsync("mymaster"),
                (m, r) =>
                {
                    Assert.Equal("OK", r);
                    Assert.Equal(Compile("SENTINEL", "REMOVE", "mymaster"), m.GetMessage());
                });
        }

        [Fact]
        public void SetTest()
        {
            TestSentinel(
                "+OK\r\n",
                x => x.Set("mymaster", "my-option", "my-value"),
                x => x.SetAsync("mymaster", "my-option", "my-value"),
                (m, r) =>
                {
                    Assert.Equal("OK", r);
                    Assert.Equal(Compile("SENTINEL", "SET", "mymaster", "my-option", "my-value"), m.GetMessage());
                });
        }



        static void TestSentinel<T>(string reply, Func<RedisSentinelClient, T> syncFunc, Func<RedisSentinelClient, Task<T>> asyncFunc, Action<FakeRedisSocket, T> test)
        {
            using (var mock = new FakeRedisSocket(reply, reply))
            using (var sentinel = new RedisSentinelClient(mock, new DnsEndPoint("fakehost", 9999)))
            {
                var sync_result = syncFunc(sentinel);
                var async_result = asyncFunc(sentinel);
                test(mock, sync_result);
                test(mock, async_result.Result);
            }
        }

        static string Compile(params string[] parts)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append('*').Append(parts.Length).Append("\r\n");
            for (int i = 0; i < parts.Length; i++)
                sb.Append('$')
                    .Append(parts[i].Length).Append("\r\n")
                    .Append(parts[i]).Append("\r\n");
            return sb.ToString();
        }

        static string Compile(params object[] parts)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append('*').Append(parts.Length).Append("\r\n");
            for (int i = 0; i < parts.Length; i++)
            {
                if (parts[i] is String)
                    sb.Append('$')
                        .Append((parts[i] as String).Length).Append("\r\n")
                        .Append((parts[i] as String)).Append("\r\n");
                else if ((parts[i] is Int64) || (parts[i] is Int32))
                    sb.Append(':')
                        .Append(parts[i]).Append("\r\n");
                else
                    throw new Exception();
            }
            return sb.ToString();
        }
    }
}
