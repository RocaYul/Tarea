using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Microsoft.WindowsAzure.Storage.Table;
using Tarea.Common.Models;
using Tarea.Common.Response;
using Tarea.Function.Entities;

namespace Tarea.Function.Funtion
{
    public static class EmployedFuntion
    {
        //Created of employed
        [FunctionName(nameof(CreateEmployed))]
        public static async Task<IActionResult> CreateEmployed(
             [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "time")] HttpRequest req,
             [Table("EmployedTime", Connection = "AzureWebJobsStorage")] CloudTable employedTable,
             ILogger log)
        {
            log.LogInformation("Received a new employed");

            string name = req.Query["name"];

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            Employed employed = JsonConvert.DeserializeObject<Employed>(requestBody); //Read body of message


            if (string.IsNullOrEmpty(employed?.IdEmployed.ToString()))
            {
                return new BadRequestObjectResult(new Response
                {
                    IsSuccess = false,
                    Message = "The request must have a id employed"
                });
            }

            //Input in table
            EmployedEntity employedEntity = new EmployedEntity
            {
                IdEmployed = employed.IdEmployed,
                InputOutput = employed.InputOutput, //London time
                Type = employed.Type,
                Consolidated = false,
                ETag = "*",
                PartitionKey = "EMPLOYED",
                RowKey = Guid.NewGuid().ToString()
            };

            //Save the entity
            TableOperation addOperation = TableOperation.Insert(employedEntity);
            await employedTable.ExecuteAsync(addOperation);

            string message = "New employed stored in table";
            log.LogInformation(message);

            return new OkObjectResult(new Response
            {
                IsSuccess = true,
                Message = message,
                Result = employedEntity
            });
        }

        //Update a Task, this is with put because is update a register  
        [FunctionName(nameof(EmployedUpdate))]
        public static async Task<IActionResult>EmployedUpdate(
            [HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = "time/{id}")] HttpRequest req,
            [Table("EmployedTime", Connection = "AzureWebJobsStorage")] CloudTable employedTable,
            string id,
            ILogger log)
        {
            log.LogInformation($"Update for empleado:{id}, received");

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            Employed employed = JsonConvert.DeserializeObject<Employed>(requestBody); //Read body of message

            //Validate employed id
            TableOperation findOperation = TableOperation.Retrieve<EmployedEntity>("EMPLOYED", id);
            TableResult findResult = await employedTable.ExecuteAsync(findOperation);
            if (findResult.Result == null)
            {
                return new BadRequestObjectResult(new Response
                {
                    IsSuccess = false,
                    Message = "Employed not found."
                });
            }

            //Update  tarea
            EmployedEntity employedEntity = (EmployedEntity)findResult.Result;
            employedEntity.IdEmployed = employed.IdEmployed;
            if (!string.IsNullOrEmpty(employed?.IdEmployed.ToString()))
            {
                employedEntity.InputOutput = employed.InputOutput;
                employedEntity.Type = employed.Type;
            }

            //Guardar la entidad
            TableOperation addOperation = TableOperation.Replace(employedEntity);
            await employedTable.ExecuteAsync(addOperation);

            string message = $"Employed: {id}, update in table ";
            log.LogInformation(message);

            return new OkObjectResult(new Response
            {
                IsSuccess = true,
                Message = message,
                Result = employedEntity
            });
        }

        //Recuperate task- get all task
        [FunctionName(nameof(GetAllEmployed))]
        public static async Task<IActionResult> GetAllEmployed(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "time")] HttpRequest req,
            [Table("EmployedTime", Connection = "AzureWebJobsStorage")] CloudTable employedTable,
            ILogger log)
        //Siempre inyectar request aunque no se necesite
        {
            log.LogInformation("Get all employed received");

            TableQuery<EmployedEntity> query = new TableQuery<EmployedEntity>();
            TableQuerySegment<EmployedEntity> employed = await employedTable.ExecuteQuerySegmentedAsync(query, null);

            string message = "Retrieved all employed";
            log.LogInformation(message);

            return new OkObjectResult(new Response
            {
                IsSuccess = true,
                Message = message,
                Result = employed
            });
        }

        //Recuperate task- get one element of the task
        [FunctionName(nameof(GetEmployedByDate))]
        public static IActionResult GetEmployedByDate(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "time/{date}")] HttpRequest req,
            [Table("Consolidate", "CONSOLIDATED", "{date}", Connection = "AzureWebJobsStorage")] ConsolidatedEntity consolidateEntity,
            DateTime date,
            ILogger log)
        //Siempre inyectar request aunque no se necesite
        {
            log.LogInformation($"Get task: {date}, received");

            if (consolidateEntity == null)
            {
                return new BadRequestObjectResult(new Response
                {
                    IsSuccess = false,
                    Message = "Date not found."
                });
            }

            string message = $"Task: {consolidateEntity.WorkDate}, retreived";
            log.LogInformation(message);

            return new OkObjectResult(new Response
            {
                IsSuccess = true,
                Message = message,
                Result = consolidateEntity
            });
        }

        [FunctionName(nameof(DeleteEmployed))]
        public static async Task<IActionResult> DeleteEmployed(
           [HttpTrigger(AuthorizationLevel.Anonymous, "delete", Route = "time/{id}")] HttpRequest req,
           [Table("EmployedTime", "EMPLOYED", "{id}", Connection = "AzureWebJobsStorage")] EmployedEntity employedEntity,
           [Table("EmployedTime", Connection = "AzureWebJobsStorage")] CloudTable employedTable,
           string id,
           ILogger log)
        //Siempre inyectar request aunque no se necesite
        {
            log.LogInformation($"Delete employed: {id}, received");

            if (employedEntity == null)
            {
                return new BadRequestObjectResult(new Response
                {
                    IsSuccess = false,
                    Message = "Employed not found."
                });
            }

            await employedTable.ExecuteAsync(TableOperation.Delete(employedEntity));
            string message = $"Task delete: {employedEntity.RowKey}, retreived";
            log.LogInformation(message);

            return new OkObjectResult(new Response
            {
                IsSuccess = true,
                Message = message,
                Result = employedEntity
            });
        }
    }
}
