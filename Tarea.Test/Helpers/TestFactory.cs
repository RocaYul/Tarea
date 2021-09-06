using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Newtonsoft.Json;
using System;
using System.IO;
using Tarea.Common.Models;
using Tarea.Function.Entities;

namespace Tarea.Test.Helpers
{
    public class TestFactory
    {
        public static ConsolidatedEntity GetConsolidated()
        {
            return new ConsolidatedEntity
            {
                ETag = "*",
                PartitionKey = "CONSOLIDATED",
                RowKey = Guid.NewGuid().ToString(),
                IdEmployed = 1,
                WorkDate = DateTime.UtcNow,
                WorkMinutes = 0
            };
        }
        public static EmployedEntity GetEmployedEntity()
        {
            return new EmployedEntity
            {
                ETag = "*",
                PartitionKey = "EMPLOYED",
                RowKey = Guid.NewGuid().ToString(),
                IdEmployed = 1,
                InputOutput = DateTime.UtcNow,
                Consolidated = false,
                Type = 0
            };
        }

        //update
        public static DefaultHttpRequest CreateHttpRequest(Guid employedId, Employed employedRequest)
        {
            string request = JsonConvert.SerializeObject(employedRequest);
            return new DefaultHttpRequest(new DefaultHttpContext())
            {
                Body = GenerateStreamFromString(request),
                Path = $"/{employedId}"
            };
        }

        //Delete
        public static DefaultHttpRequest CreateHttpRequest(Guid employedId)
        {
            return new DefaultHttpRequest(new DefaultHttpContext())
            {
                Path = $"/{employedId}"
            };
        }

        //Created
        public static DefaultHttpRequest CreateHttpRequest(Employed employedRequest)
        {
            string request = JsonConvert.SerializeObject(employedRequest);
            return new DefaultHttpRequest(new DefaultHttpContext())
            {
                Body = GenerateStreamFromString(request)
            };
        }

        //GetAll
        public static DefaultHttpRequest CreateHttpRequest()
        {
            return new DefaultHttpRequest(new DefaultHttpContext());
        }

        public static DefaultHttpRequest CreateHttpRequest(DateTime work)
        {
            return new DefaultHttpRequest(new DefaultHttpContext())
            {
                Path = $"/{work}"
            };
        }

        public static Employed GetEmployedRequest()
        {
            return new Employed
            {
                IdEmployed = 1,
                InputOutput = DateTime.UtcNow,
                Consolidated = false,
                Type = 0
            };
        }

        public static Consolidated GetConsolidateRequest()
        {
            return new Consolidated
            {
                IdEmployed = 1,
                WorkDate = DateTime.UtcNow,
                WorkMinutes = 0
            };
        }

        public static Stream GenerateStreamFromString(string stringToConvert)
        {
            MemoryStream stream = new MemoryStream();
            StreamWriter writer = new StreamWriter(stream);
            writer.Write(stringToConvert);
            writer.Flush();
            stream.Position = 0;
            return stream;
        }

        public static ILogger CreateLogger(LoggerTypes type = LoggerTypes.Null)
        {
            ILogger logger;
            if (type == LoggerTypes.List)
            {
                logger = new ListLogger();
            }
            else
            {
                logger = (ILogger)NullLoggerFactory.Instance.CreateLogger("Null Logger");
            }
            return logger;
        }
    }
}
