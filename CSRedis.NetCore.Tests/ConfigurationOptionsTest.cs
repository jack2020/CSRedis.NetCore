using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace CSRedis.NetCore.Tests
{
    public class ConfigurationOptionsTest
    {
		[Fact]
		public void Test()
		{
			var str = "192.168.1.166:7001,192.168.1.166:7002,defaultDatabase=1,serviceName=session-master,password=123456,ssl=false";
			var config = ConfigurationOptions.Parse(str);
		}
    }
}
