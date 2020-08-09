using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Data.SqlClient;
using Dapper;
using System.Collections.Generic;
using System.Linq;

namespace BlazorContactsFunction
{
    public static class GetContact
    {
        [FunctionName("GetContact")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)] HttpRequest req,
            ILogger log)
        {
            var contactId = req.Query["id"].ToString();

            var contacts = new List<Contact>(); 

            var connectionString = Environment.GetEnvironmentVariable("contactsdb_connection");

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                var query = "SELECT * FROM Contact";

                if (string.IsNullOrWhiteSpace(contactId) == false)
                {
                    query = $"{query} WHERE ContactId = {contactId}";
                }

                var result = await connection.QueryAsync<Contact>(query);

                contacts = result.ToList();
            }

            return new OkObjectResult(contacts);
        }
    }
}
