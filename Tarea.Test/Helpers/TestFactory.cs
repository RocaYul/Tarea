using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
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


        private static Stream GenerateStreamFromString(string request)
        {
            throw new NotImplementedException();
        }
    }
}
