using System;
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
    public static class UpdateContact
    {
        [FunctionName("UpdateContact")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = null)] HttpRequest req,
            ILogger log)
        {

            var contactJson = await req.ReadAsStringAsync();

            var contact = JsonConvert.DeserializeObject<Contact>(contactJson);

            var connectionString = Environment.GetEnvironmentVariable("contactsdb_connection");

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                var query = @$"
                            UPDATE Contact 
                            SET  FirstName = '{contact.FirstName}'
                            ,LastName = '{contact.LastName}'
                            WHERE ContactId = {contact.ContactId}";

                await connection.QueryAsync(query);
            }

            return new OkResult();
        }
    }
}
