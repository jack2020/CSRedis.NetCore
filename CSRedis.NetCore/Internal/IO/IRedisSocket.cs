using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace CSRedis.NetCore.Internal.IO
{
    public interface IRedisSocket : IDisposable
    {
        bool Connected { get; }
        int ReceiveTimeout { get; set; }
        int SendTimeout { get; set; }
        void Connect(EndPoint endpoint);
        bool ConnectAsync(SocketAsyncEventArgs args);
        bool SendAsync(SocketAsyncEventArgs args);
        Stream GetStream();
    }
}
