using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
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

        //Mapear los metodos de crear
        public static DefaultHttpRequest CreateHttpRequest(Guid employedId, Employed tareaRequest)
        {
            string request = JsonConvert.SerializeObject(tareaRequest);
            return new DefaultHttpRequest(new DefaultHttpContext())
            {
                Body = GenerateStreamFromString(request),
                Path = $"/{employedId}"
            };
        }

        public static DefaultHttpRequest CreateHttpRequest(Guid employedId)
        {
            return new DefaultHttpRequest(new DefaultHttpContext())
            {
                Path = $"/{employedId}"
            };
        }

        //Mapear los metodos de Eliminar
        public static DefaultHttpRequest CreateHttpRequest(Employed employedRequest)
        {
            string request = JsonConvert.SerializeObject(employedRequest);
            return new DefaultHttpRequest(new DefaultHttpContext())
            {
                Body = GenerateStreamFromString(request)
            };
        }

        public static DefaultHttpRequest CreateHttpRequest()
        {
            return new DefaultHttpRequest(new DefaultHttpContext());
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

        private static Stream GenerateStreamFromString(string stringToConvert)
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
