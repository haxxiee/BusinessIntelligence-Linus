using IoTHubTrigger = Microsoft.Azure.WebJobs.EventHubTriggerAttribute;

using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Azure.EventHubs;
using System.Text;
using System.Net.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using AzureFunctions.Models;

namespace AzureFunctions
{
    public static class SaveToTableStorage
    {
        private static HttpClient client = new HttpClient();

        [FunctionName("SaveToTableStorage")]
        [return: Table("StorageTable")]
        public static ShkMessage Run([IoTHubTrigger("messages/events", Connection = "IotHubConnection")] EventData message, ILogger log)
        {

            if (message.Properties["Type"].ToString() == "ShockSensor")
			{
                try
                {

                    var data = JsonConvert.DeserializeObject<ShkMessage>(Encoding.UTF8.GetString(message.Body.Array));
                    data.PartitionKey = "ShkSensor";
                    data.RowKey = Guid.NewGuid().ToString();

                    DateTimeOffset dateTimeOffset = DateTimeOffset.FromUnixTimeSeconds(data.Time);
                    data.UtcTime = dateTimeOffset.UtcDateTime;

                    log.LogInformation("Saving data to Table Storage.");
                    return data;
                }
                catch
                {
                    log.LogInformation("Failed to Deserialize message. No data was save to Table Storage");
                }

                return null;
            }
            else
			{
                return null;
			}

            
        }
    }
}