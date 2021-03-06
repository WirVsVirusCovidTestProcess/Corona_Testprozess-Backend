using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.Documents;
using System.Collections.ObjectModel;

namespace CT.Backend.Functions
{
    public static class WarmUp
    {
        [FunctionName("WarmUp")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            [CosmosDB(
                databaseName: "QuestionsData",
                collectionName: "QuestionsData",
                ConnectionStringSetting = "QuestionsDBConnection")] DocumentClient questionsTable,
            [CosmosDB(
                databaseName: "Appointment",
                collectionName: "AppointmentForUsers",
                ConnectionStringSetting = "AppointmentDBConnection")] DocumentClient appointmentsTable,
            [CosmosDB(
                databaseName: "UserInformation",
                collectionName: "UserInformation",
                ConnectionStringSetting = "UserInformationDBConnection")] DocumentClient usersTable,
            ILogger log)
        {
            log.LogInformation("Create questions tables.");

            await questionsTable.CreateDatabaseIfNotExistsAsync(new Database() { Id = "QuestionsData" });
            await questionsTable.CreateDocumentCollectionIfNotExistsAsync(UriFactory.CreateDatabaseUri("QuestionsData"), new DocumentCollection()
            {
                Id = "QuestionsData",
                PartitionKey = new PartitionKeyDefinition()
                {
                    Paths = new Collection<string>() { "/Source" }
                },
                UniqueKeyPolicy = new UniqueKeyPolicy()
                {
                    UniqueKeys = new Collection<UniqueKey>() {
                        new UniqueKey() {
                            Paths = new Collection<string>() { "/Token"}
                        }
                    }
                }
            });
            await questionsTable.CreateDocumentCollectionIfNotExistsAsync(UriFactory.CreateDatabaseUri("QuestionsData"), new DocumentCollection()
            {
                Id = "leases",
                PartitionKey = new PartitionKeyDefinition()
                {
                    Paths = new Collection<string>() { "/id" }
                }
            });

            log.LogInformation("Create appointment tables.");

            await questionsTable.CreateDatabaseIfNotExistsAsync(new Database() { Id = "Appointment" });
            await questionsTable.CreateDocumentCollectionIfNotExistsAsync(UriFactory.CreateDatabaseUri("Appointment"), new DocumentCollection()
            {
                Id = "AppointmentForUsers",
                PartitionKey = new PartitionKeyDefinition()
                {
                    Paths = new Collection<string>() { "/id" }
                },
                UniqueKeyPolicy = new UniqueKeyPolicy()
                {
                    UniqueKeys = new Collection<UniqueKey>() {
                        new UniqueKey() {
                            Paths = new Collection<string>() { "/Token"}
                        }
                    }
                }
            });
            await questionsTable.CreateDocumentCollectionIfNotExistsAsync(UriFactory.CreateDatabaseUri("Appointment"), new DocumentCollection()
            {
                Id = "leases",
                PartitionKey = new PartitionKeyDefinition()
                {
                    Paths = new Collection<string>() { "/id" }
                }
            });

            log.LogInformation("Create users tables.");

            await questionsTable.CreateDatabaseIfNotExistsAsync(new Database() { Id = "UserInformation" });
            await questionsTable.CreateDocumentCollectionIfNotExistsAsync(UriFactory.CreateDatabaseUri("UserInformation"), new DocumentCollection()
            {
                Id = "UserInformation",
                PartitionKey = new PartitionKeyDefinition()
                {
                    Paths = new Collection<string>() { "/Source" }
                },
                UniqueKeyPolicy = new UniqueKeyPolicy()
                {
                    UniqueKeys = new Collection<UniqueKey>() {
                        new UniqueKey() {
                            Paths = new Collection<string>() { "/Token"}
                        }
                    }
                }
            });
            await questionsTable.CreateDocumentCollectionIfNotExistsAsync(UriFactory.CreateDatabaseUri("UserInformation"), new DocumentCollection()
            {
                Id = "leases",
                PartitionKey = new PartitionKeyDefinition()
                {
                    Paths = new Collection<string>() { "/id" }
                }
            });
            return new OkObjectResult("Successfully create the database schema");
        }
    }
}
