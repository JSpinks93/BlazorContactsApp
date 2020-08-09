using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Data.SqlClient;
using Dapper;

namespace BlazorContactsFunction
{
    public static class InsertContact
    {
        [FunctionName("InsertContact")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            int contactId = 0;

            var contactJson = await req.ReadAsStringAsync();

            var contactToInsert = JsonConvert.DeserializeObject<Contact>(contactJson);

            var connectionString = Environment.GetEnvironmentVariable("contactsdb_connection");

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                var query = $"INSERT INTO Contact (FirstName, LastName) OUTPUT INSERTED.ContactID VALUES ('{contactToInsert.FirstName}', '{contactToInsert.LastName}')";

                contactId = await connection.QuerySingleAsync<int>(query);
            }

            return new OkObjectResult(contactId);
        }
    }
}
