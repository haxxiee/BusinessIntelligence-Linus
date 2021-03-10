
using Microsoft.Azure.Cosmos.Table;
using System;
using System.Collections.Generic;
using System.Text;

namespace AzureFunctions.Models
{
    public class ShkMessage : TableEntity
    {
        public string DeviceId { get; set; }
        public long Time { get; set; }
        public string Message { get; set; }

        public DateTime UtcTime { get; set; }
    }
}