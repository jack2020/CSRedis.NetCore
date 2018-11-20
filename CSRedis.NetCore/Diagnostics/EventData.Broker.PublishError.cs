// Copyright (c) .NET Core Community. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System;

namespace CSRedis.NetCore.Diagnostics
{
    public class BrokerPublishErrorEventData : BrokerPublishEndEventData, IErrorEventData
    {
        public BrokerPublishErrorEventData(Guid operationId, string operation, string address,string content, Exception exception, DateTimeOffset startTime,
            TimeSpan duration)
            : base(operationId, operation, address, content, startTime, duration)
        {
            Exception = exception;
        }

        public Exception Exception { get; }
    }
}