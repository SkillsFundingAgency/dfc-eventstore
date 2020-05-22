using System;
using System.Threading.Tasks;
using DFC.EventStore.Function;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ILogger = Microsoft.Extensions.Logging.ILogger;

namespace DFC.ServiceTaxonomy.ApiFunction.Tests
{
    public class ExecuteHttpTriggerTests
    {
        private readonly Execute _executeFunction;
        private readonly ILogger _log;
        private readonly HttpRequest _request;

        public ExecuteHttpTriggerTests()
        {
           
        }

        private async Task<IActionResult> RunFunction(string contentType, Guid? id)
        {
            return await _executeFunction.Run(_request, contentType, id, _log).ConfigureAwait(false);
        }
    }
}