using System;
using System.Collections.Generic;
using System.Text;
using Tarea.Test.Helpers;
using Tarea.Function.Funtion;
using Xunit;

namespace Tarea.Test.Tests
{
    public class ScheduledFunctionTest
    {

        [Fact]
        public void ScheduledFunction_Should_Log_Message()
        {
            //Arrage
            MockCloudTableTarea mockTarea = new MockCloudTableTarea(new Uri("http://127.0.0.1:10002/devstoreaccount1/reports"));
            MockCloudTableConsolidate mockTarea1 = new MockCloudTableConsolidate(new Uri("http://127.0.0.1:10002/devstoreaccount1/reports"));
            ListLogger logger = (ListLogger)TestFactory.CreateLogger(LoggerTypes.List);

            //Act
            Consolidated.Run(null, mockTarea, mockTarea1, logger);
            string message = logger.Logs[0];

            //Asert
            Assert.Contains("Consolidate completed", message);
        }
    }
}
