using System;
using System.Threading.Tasks;
using DFC.App.EventStore.Data.Models;
using DFC.EventStore.Function;
using FakeItEasy;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.EventGrid.Models;
using Xunit;
using ILogger = Microsoft.Extensions.Logging.ILogger;

namespace DFC.ServiceTaxonomy.ApiFunction.Tests
{
    public class ExecuteEventGridTriggerTests
    {
        private readonly Execute _executeFunction;
        private readonly ILogger _log;
        private readonly HttpRequest _request;

        public ExecuteEventGridTriggerTests()
        {
            _log = A.Fake<ILogger>();
        }

        [Fact]
        public void ExecuteEventGridTrigger_WhenPassedEvent_StoresEventSuccessfully()
        {
            //Arrange

            var eventStoreModel = new EventStoreModel()
            {

            };

            //Act

            //Assert
        }

        private async Task<IActionResult> RunFunction(EventStoreModel eventStoreModel)
        {
            return await _executeFunction.Run(eventStoreModel, _log).ConfigureAwait(false);
        }
    }
}