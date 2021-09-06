using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.AspNetCore.Mvc;
using System;
using Tarea.Common.Models;
using Tarea.Function.Entities;
using Tarea.Function.Funtion;
using Tarea.Test.Helpers;
using Xunit;
namespace Tarea.Test.Tests
{
    public class EmployedAPITest
    {
        //Se crea este vlaor por el suso reiteartivo del logger
        private readonly Microsoft.Extensions.Logging.ILogger logger = TestFactory.CreateLogger();

        /*[Fact]
        public async void GetAllEmployed_Should_Return_200()
        {
            //Arrenge inicio de las pruebas
            MockCloudTableTarea mockTarea = new MockCloudTableTarea(new Uri("http://127.0.0.1:10002/devstoreaccount1/EmployedTime?comp=acl"));
            DefaultHttpRequest request = TestFactory.CreateHttpRequest();

            //Act procedimiento
            IActionResult response = await EmployedFuntion.GetAllEmployed(request, mockTarea, logger);

            //Asert si la prueba si hace lo que necesita
            OkObjectResult result = (OkObjectResult)response;
            Assert.Equal(StatusCodes.Status200OK, result.StatusCode);
        }
        */

        [Fact]
        public async void CreateEmployed_Should_Return_200()
        {
            //Arrenge inicio de las pruebas
            MockCloudTableTarea mockTarea = new MockCloudTableTarea(new Uri("http://127.0.0.1:10002/devstoreaccount1/reports"));
            Employed employedRequest = TestFactory.GetEmployedRequest();
            DefaultHttpRequest request = TestFactory.CreateHttpRequest(employedRequest);

            //Act procedimiento
            IActionResult response = await EmployedFuntion.CreateEmployed(request, mockTarea, logger);

            //Asert si la prueba si hace lo que necesita
            OkObjectResult result = (OkObjectResult)response;
            Assert.Equal(StatusCodes.Status200OK, result.StatusCode);
        }

        [Fact]
        public async void UpdateEmployed_Should_Return_200()
        {
            //Arrenge inicio de las pruebas
            MockCloudTableTarea mockTarea = new MockCloudTableTarea(new Uri("http://127.0.0.1:10002/devstoreaccount1/reports"));
            Employed tareaRequest = TestFactory.GetEmployedRequest();
            Guid tareaId = Guid.NewGuid();
            DefaultHttpRequest request = TestFactory.CreateHttpRequest(tareaId, tareaRequest);

            //Act procedimiento
            IActionResult response = await EmployedFuntion.EmployedUpdate(request, mockTarea, tareaId.ToString(),logger);

            //Asert si la prueba si hace lo que necesita
            OkObjectResult result = (OkObjectResult)response;
            Assert.Equal(StatusCodes.Status200OK, result.StatusCode);
        }

       [Fact]
        public async void DeleteEmployed_Should_Return_200()
        {
            //Arrenge inicio de las pruebas
            MockCloudTableTarea mockTarea = new MockCloudTableTarea(new Uri("http://127.0.0.1:10002/devstoreaccount1/reports"));
            Guid tareaId = Guid.NewGuid();
            DefaultHttpRequest request = TestFactory.CreateHttpRequest(tareaId);

            //Act procedimiento
            IActionResult response = await EmployedFuntion.DeleteEmployedd(request, mockTarea, tareaId.ToString(), logger);

            //Asert si la prueba si hace lo que necesita
            OkObjectResult result = (OkObjectResult)response;
            Assert.Equal(StatusCodes.Status200OK, result.StatusCode);
       
        }

        /*[Fact]
        public async void GetDateEmployed_Should_Return_200()
        {
            //Arrenge inicio de las pruebas
            MockCloudTableConsolidate mockTarea = new MockCloudTableConsolidate(new Uri("http://127.0.0.1:10002/devstoreaccount1/reports"));
            DateTime date = DateTime.UtcNow;
            DefaultHttpRequest request = TestFactory.CreateHttpRequest(date);

            //Act procedimiento
            IActionResult response = await EmployedFuntion.GetEmployedByDate(request,mockTarea, date, logger);

            //Asert si la prueba si hace lo que necesita
            OkObjectResult result = (OkObjectResult)response;
            Assert.Equal(StatusCodes.Status200OK, result.StatusCode);

        }
        */
    }
}
