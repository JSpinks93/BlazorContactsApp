using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Data.SqlClient;
using Dapper;

namespace BlazorContactsFunction
{
    public static class DeleteContact
    {
        [FunctionName("DeleteContact")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "delete", Route = null)] HttpRequest req,
            ILogger log)
        {
            var contactId = req.Query["id"].ToString();

            var connectionString = Environment.GetEnvironmentVariable("contactsdb_connection");

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                var query = $"DELETE Contact WHERE ContactId = {contactId}";

                await connection.QueryAsync<Contact>(query);
            }

            return new OkResult();
        }
    }
}
