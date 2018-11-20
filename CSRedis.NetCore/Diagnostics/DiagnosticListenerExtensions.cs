
using CSRedis.NetCore.Models;
using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
namespace CSRedis.NetCore.Diagnostics
{
    /// <summary>

    /// </summary>
    public static class CSRedisDiagnosticListenerExtensions
    {
        public const string DiagnosticListenerName = "CSRedisDiagnosticListener";

        private const string CSRedisPrefix = "DotNetCore.CSRedis.";

        public const string CSRedisBeforePublishMessageStore = CSRedisPrefix + nameof(WritePublishMessageStoreBefore);
        public const string CSRedisAfterPublishMessageStore = CSRedisPrefix + nameof(WritePublishMessageStoreAfter);
        public const string CSRedisErrorPublishMessageStore = CSRedisPrefix + nameof(WritePublishMessageStoreError);
        public static void WritePublishMessageStoreBefore(this DiagnosticListener @this, BrokerPublishEventData eventData)
        {
            if (@this.IsEnabled(CSRedisBeforePublishMessageStore))
            {
                eventData.Headers = new TracingHeaders();
                @this.Write(CSRedisBeforePublishMessageStore, eventData);
            }
        }

        public static void WritePublishMessageStoreAfter(this DiagnosticListener @this, BrokerPublishEndEventData eventData)
        {
            if (@this.IsEnabled(CSRedisAfterPublishMessageStore))
            {
                eventData.Headers = new TracingHeaders();
                @this.Write(CSRedisAfterPublishMessageStore, eventData);
            }
        }

        public static void WritePublishMessageStoreError(this DiagnosticListener @this, BrokerPublishErrorEventData eventData)
        {
            if (@this.IsEnabled(CSRedisErrorPublishMessageStore))
            {
                eventData.Headers = new TracingHeaders();
                @this.Write(CSRedisErrorPublishMessageStore, eventData);
            }
        }
    }
}