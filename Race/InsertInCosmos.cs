using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Microsoft.Azure.Cosmos;
using Race.Models;

namespace Race
{
    public static class InsertInCosmos
    {
        [FunctionName("AddCarLog")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "insertincosmos")] HttpRequest req,
            ILogger log)
        {
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            Car c = JsonConvert.DeserializeObject<Car>(requestBody);

            CosmosClientOptions options = new CosmosClientOptions();
            options.ConnectionMode = ConnectionMode.Gateway;

            CosmosClient client = new CosmosClient(Environment.GetEnvironmentVariable("CosmosConnection"), options);

            var container = client.GetContainer("cosmosdblennert", "cars");

            c.CarId = Guid.NewGuid();
            c.EmailAdress = "lennert.defauw@student.howest.be";
            c.id = Guid.NewGuid().ToString();

            await container.CreateItemAsync<Car>(c, new PartitionKey(c.EmailAdress));

            return new StatusCodeResult(200);


        }
    }
}
