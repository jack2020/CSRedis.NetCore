using System;
using CSRedis.NetCore;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
			using (var redis = new RedisClient("192.168.1.67", 6379))
			{
				string ping = redis.Ping();
				var s = redis.Info();
				string echo = redis.Echo("hello world");
				DateTime time = redis.Time();
			}

			//while (true)
			{
				using (var redisPool = new RedisConnectionPool("192.168.1.67", 6379, 50))
				{
					var redis = redisPool.GetClient();
					var r = redis.Select(15);
					var r1 = redis.Set("csredis_netcore", DateTime.Now, 600);
					var r2 = redis.Get("csredis_netcore");
					Console.WriteLine(r2);
				}
			}

			RedisClient redis2 = null;
	        using (var sentinel = new RedisSentinelManager("192.168.1.170:26379"))
	        {
		        //sentinel.Add("192.168.1.400"); // add host using default port 
		        //sentinel.Add("192.168.1.400", 36379); // add host using specific port
		        sentinel.Connected += (s, e) => sentinel.Call(x => x.Host); // this will be called each time a master connects
		        sentinel.Connect("session-master"); // open connection
		        var test2 = sentinel.Call(x => x.Time()); // use the Call() lambda to access the current master connection
		        redis2 = sentinel.Call(x => x);
		        var r = redis2.Select(15);
				var r1 = redis2.Set("csredis_netcore2", DateTime.Now, 600);
				var r2 = redis2.Get("csredis_netcore2");
		        var r3 = redis2.Set("csredis_netcore3", DateTime.Now);
		        Console.WriteLine(r2);
			}

	        //while (true)
	        {
		        var r0 = redis2.Select(15);
				var r1 = redis2.Set("csredis_netcore4", DateTime.Now);
		        var r2 = redis2.Get("csredis_netcore4");
		        Console.WriteLine(r2);
			}

			Console.ReadKey();
        }
    }
}
