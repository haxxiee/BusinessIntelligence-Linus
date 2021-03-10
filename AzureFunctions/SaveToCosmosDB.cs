using IoTHubTrigger = Microsoft.Azure.WebJobs.EventHubTriggerAttribute;

using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Azure.EventHubs;
using System.Text;
using System.Net.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using Newtonsoft.Json.Linq;

namespace AzureFunctions
{
    public static class SaveToCosmosDB
    {
        private static HttpClient client = new HttpClient();

        [FunctionName("SaveToCosmosDB")]
        public static void Run(
            [IoTHubTrigger("messages/events", Connection = "IotHubConnection", ConsumerGroup = "cosmosdb")] EventData message,
            [CosmosDB(
                databaseName: "IOT20",
                collectionName: "Messages",
                ConnectionStringSetting = "CosmosDbConnection",
                CreateIfNotExists = true
            )]out dynamic cosmos,
            ILogger log
            
        )
        {
            log.LogInformation($"messages/events: {Encoding.UTF8.GetString(message.Body.Array)}");

            if (message.Properties["Type"].ToString() == "DHT")
			{

                cosmos = Encoding.UTF8.GetString(message.Body.Array);

                dynamic data = JsonConvert.DeserializeObject(cosmos);

                string hej = data["time"];

                long time = long.Parse(hej);

                DateTimeOffset dateTimeOffset = DateTimeOffset.FromUnixTimeSeconds(time);
                var UtcTime = dateTimeOffset.UtcDateTime;

                data.UtcTime = UtcTime;

                string newdata = JsonConvert.SerializeObject(data);

                cosmos = newdata;



            }
			else
			{
                cosmos = null;
			}
        }
    }
}