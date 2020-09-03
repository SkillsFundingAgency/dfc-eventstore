using System;
using System.Threading.Tasks;
using DFC.App.EventStore.Data.Models;
using DFC.Compui.Cosmos.Contracts;
using DFC.EventStore.Function;
using FakeItEasy;
using Microsoft.Azure.EventGrid;
using Xunit;
using ILogger = Microsoft.Extensions.Logging.ILogger;

namespace DFC.ServiceTaxonomy.ApiFunction.Tests
{
    public class ExecuteEventGridTriggerTests
    {
        private readonly Execute _executeFunction;
        private readonly ILogger _log;
        private readonly IDocumentService<EventStoreModel> _eventStoreRepository;

        public ExecuteEventGridTriggerTests()
        {
            _log = A.Fake<ILogger>();
            _eventStoreRepository = A.Fake<IDocumentService<EventStoreModel>>();
            _executeFunction = new Execute(_eventStoreRepository);
        }

        [Fact]
        public async Task ExecuteEventGridTrigger_WhenPassedEvent_StoresEventSuccessfully()
        {
            //Arrange
            var eventStoreModel = new EventStoreModel()
            {
                Data = "Some data...",
                DataVersion = "1.0.0",
                EventTime = DateTime.UtcNow,
                Id = Guid.NewGuid(),
                EventType = EventTypes.StorageBlobCreatedEvent,
                Subject = "My Test Subject",
                Topic = "My/Topic/Test"
            };

            //Act
            await RunFunction(eventStoreModel);

            //Assert
            A.CallTo(() => _eventStoreRepository.UpsertAsync(A<EventStoreModel>.Ignored)).MustHaveHappenedOnceExactly();
        }

        [Fact]
        public async Task ExecuteEventGridTrigger_ThrowsException_ExceptionCaught()
        {
            //Arrange
            var eventStoreModel = new EventStoreModel()
            {
                Data = "Some data...",
                DataVersion = "1.0.0",
                EventTime = DateTime.UtcNow,
                Id = Guid.NewGuid(),
                EventType = EventTypes.StorageBlobCreatedEvent,
                Subject = "My Test Subject",
                Topic = "My/Topic/Test"
            };

            A.CallTo(() => _eventStoreRepository.UpsertAsync(A<EventStoreModel>.Ignored)).Throws(new InvalidOperationException("Something went wrong"));

            //Act

            //Assert
            await Assert.ThrowsAsync<InvalidOperationException>(async () => await RunFunction(eventStoreModel));
        }

        [Fact]
        public async Task ExecuteEventGridTrigger_WhenPassedNullEvent_DoesNotStoreEvent()
        {
            //Arrange
           
            //Act
            await RunFunction(null);

            //Assert
            A.CallTo(() => _eventStoreRepository.UpsertAsync(A<EventStoreModel>.Ignored)).MustNotHaveHappened();
        }

        private async Task RunFunction(EventStoreModel eventStoreModel)
        {
           await _executeFunction.Run(eventStoreModel, _log).ConfigureAwait(false);
        }
    }
}