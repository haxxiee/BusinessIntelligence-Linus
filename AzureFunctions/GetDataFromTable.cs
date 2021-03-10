using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Microsoft.Azure.Cosmos.Table;
using System.Collections.Generic;
using AzureFunctions.Models;
using System.Linq;

namespace AzureFunctions
{
    public static class GetDataFromTable
    {
        [FunctionName("GetDataFromTable")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = null)] HttpRequest req,
            [Table("StorageTable")] CloudTable cloudTable,
            ILogger log)
        {
            string limit = req.Query["limit"];
            string orderby = req.Query["orderby"];

            IEnumerable<ShkMessage> results = await cloudTable.ExecuteQuerySegmentedAsync(new TableQuery<ShkMessage>(), null);

            results = results.OrderBy(ts => ts.Timestamp);

            if (orderby == "desc")
                results = results.OrderByDescending(ts => ts.Timestamp);

            if (limit != null)
                results = results.Take(int.Parse(limit));

            return new OkObjectResult(results);
        }
    }
}
