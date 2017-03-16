using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EventStore.ClientAPI;
using Lax.Domain.Common;
using Lax.Domain.Exceptions;
using Newtonsoft.Json;

namespace Lax.Domain.Events.EventStore {

    public class EventStoreDomainRepository : DomainRepositoryBase {
        
        private IEventStoreConnection _connection;

        private readonly IEventStoreConnectionProvider _connectionProvider;

        public EventStoreDomainRepository(IEventStoreConnectionProvider connectionProvider) {
            _connectionProvider = connectionProvider;
        }

        private string AggregateToStreamName(Type type, Guid id) => 
            string.Format("{0}-{1}", type.Name, id);

        private async Task<IEventStoreConnection> GetConnection() => 
            _connection ?? (_connection = await _connectionProvider.ProvideConnection());

        public override async Task<IEnumerable<IEvent<TAggregateState>>> Save<TAggregate, TAggregateState>(TAggregate aggregate) {
            var events = aggregate.UncommitedEvents().ToList();
            var expectedVersion = CalculateExpectedVersion(aggregate, events);
            var eventData = events.Select(CreateEventData);
            var streamName = AggregateToStreamName(aggregate.GetType(), aggregate.Id);
            var connection = await GetConnection();
            await connection.AppendToStreamAsync(streamName, expectedVersion, eventData);
            return events;
        }

        public override async Task<TAggregate> GetById<TAggregate, TAggregateState>(Guid id) {
            var streamName = AggregateToStreamName(typeof(TAggregate), id);
            var streamEvents = new List<ResolvedEvent>();
            StreamEventsSlice currentSlice;
            var nextSliceStart = StreamPosition.Start;
            var connection = await GetConnection();
            do {
                currentSlice = await connection.ReadStreamEventsForwardAsync(streamName, nextSliceStart, 4096, false);
                if (currentSlice.Status == SliceReadStatus.StreamNotFound) {
                    throw new AggregateNotFoundException("Could not find aggregate of type " + typeof(TAggregate) +
                                                         " and id " + id);
                }
                nextSliceStart = currentSlice.NextEventNumber;
                streamEvents.AddRange(currentSlice.Events);
            } while (!currentSlice.IsEndOfStream);

            var deserializedEvents = streamEvents.Select(e => {
                var metadata = DeserializeObject<Dictionary<string, string>>(e.OriginalEvent.Metadata);
                var eventData = DeserializeObject(e.OriginalEvent.Data, metadata[EventClrTypeHeader]);
                return eventData as IEvent<TAggregateState>;
            });
            return BuildAggregate<TAggregate, TAggregateState>(deserializedEvents);
        }

        private T DeserializeObject<T>(byte[] data) => (T) (DeserializeObject(data, typeof(T).AssemblyQualifiedName));

        private object DeserializeObject(byte[] data, string typeName) {
            var jsonString = Encoding.UTF8.GetString(data);
            return JsonConvert.DeserializeObject(jsonString, Type.GetType(typeName));
        }

        public EventData CreateEventData(object @event) {
            var eventHeaders = new Dictionary<string, string> {
                {
                    EventClrTypeHeader, @event.GetType().AssemblyQualifiedName
                }
            };
            var eventDataHeaders = SerializeObject(eventHeaders);
            var data = SerializeObject(@event);
            var eventData = new EventData(Guid.NewGuid(), @event.GetType().Name, true, data, eventDataHeaders);
            return eventData;
        }

        private byte[] SerializeObject(object obj) {
            var jsonObj = JsonConvert.SerializeObject(obj);
            var data = Encoding.UTF8.GetBytes(jsonObj);
            return data;
        }

        public string EventClrTypeHeader = "EventClrTypeName";

    }

}