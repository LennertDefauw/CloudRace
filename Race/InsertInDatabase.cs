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
using System.Data.SqlClient;

namespace Race
{
    public static class InsertInDatabase
    {
        [FunctionName("AddCar")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "insert")] HttpRequest req, 
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            Car c = JsonConvert.DeserializeObject<Car>(requestBody);

            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
            builder.DataSource = Environment.GetEnvironmentVariable("ConnectionDatabase");
            builder.UserID = Environment.GetEnvironmentVariable("UsernameDB");
            builder.Password = Environment.GetEnvironmentVariable("Password");
            builder.InitialCatalog = "dblennert";

            Guid id = Guid.NewGuid();

            using (SqlConnection connection = new SqlConnection(builder.ConnectionString))
            {

                connection.Open();

                String sql = "INSERT INTO REGISTRATIONS (CarId, TeamId, Brand, Team, StartTime) VALUES (@carid, @teamid, @brand, @team, @starttime)";

                using (SqlCommand command = new SqlCommand(sql, connection))
                {

                    command.Parameters.AddWithValue("@carid", id);
                    command.Parameters.AddWithValue("@teamid", c.TeamId);
                    command.Parameters.AddWithValue("@brand", c.Brand);
                    command.Parameters.AddWithValue("@team", c.Team);
                    command.Parameters.AddWithValue("@starttime", c.StartTime);

                    command.ExecuteNonQueryAsync();
                }
            }

            return new StatusCodeResult(200);

        }
    }
}
