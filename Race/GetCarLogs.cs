using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Race.Models;
using System.Collections.Generic;
using Microsoft.Azure.Cosmos;

namespace Race
{
    public static class GetCarLogs
    {
        [FunctionName("GetCarLogs")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "cars")] HttpRequest req,
            ILogger log)
        {
            double price = 0;

            List<Car> cars = new List<Car>();

            CosmosClientOptions options = new CosmosClientOptions();
            options.ConnectionMode = ConnectionMode.Gateway;

            CosmosClient client = new CosmosClient(Environment.GetEnvironmentVariable("CosmosConnection"), options);

            var container = client.GetContainer("cosmosdblennert", "cars");


            QueryDefinition def = new QueryDefinition("SELECT * FROM c WHERE c.EmailAdress = 'lennert.defauw@student.howest.be'");

            FeedIterator<Car> feedIterator = container.GetItemQueryIterator<Car>(def, null);

            while (feedIterator.HasMoreResults)
            {
                foreach (Car item in await feedIterator.ReadNextAsync())
                {
                    {
                        cars.Add(item);
                    }
                }
            }
            return new OkObjectResult(cars);
        }
    }
}
