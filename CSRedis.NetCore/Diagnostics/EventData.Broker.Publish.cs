// Copyright (c) .NET Core Community. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System;

namespace CSRedis.NetCore.Diagnostics
{
    public class BrokerPublishEventData 
    {
        public BrokerPublishEventData(Guid operationId, string operation, string Address, string Content, DateTimeOffset startTime)
            
        {
            StartTime = startTime;
            this.Address = Address;
            this.Content = Content;
            OperationId = operationId;
            Operation = operation;
        }
        public Guid OperationId { get; set; }
        public DateTimeOffset StartTime { get; }

        public TracingHeaders Headers { get; set; }

        public string Address { get; set; }

        public string Content { get; set; }
        public string Operation { get; set; }
    }
}