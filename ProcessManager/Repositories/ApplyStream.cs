using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using EventStore.ClientAPI;
using Newtonsoft.Json;
using ProcessManager.Events;
using ProcessManager.Products;

namespace ProcessManager.Repositories
{
    public class ApplyStream
    {
        public void Write(string type, string data, string metadata)
        {
            using (var connection = EventStoreConnection.Create(new Uri("tcp://admin:changeit@localhost:1113")))
            {
                connection.ConnectAsync().Wait();

                var myEvent = new EventData(Guid.NewGuid(), type, true,
                            Encoding.UTF8.GetBytes(data), Encoding.UTF8.GetBytes(metadata));

                connection.AppendToStreamAsync("test-apply",
                                               ExpectedVersion.Any, myEvent).Wait();
            }
        }
    }

    public interface IStreamRepository<out T>
    {
        void Save(AggregateRoot aggregate, EventMetaData metaData);
        T GetById(Guid id);
    }

    public class ApplyStream<T> : IStreamRepository<T> where T : AggregateRoot, new()
    {
        public void Save(AggregateRoot aggregate, EventMetaData metaData)
        {
            using (var connection = EventStoreConnection.Create(new Uri("tcp://admin:changeit@localhost:1113")))
            {
                connection.ConnectAsync().Wait();

                foreach (var @Event in aggregate.GetUncommitedChanges())
                {
                    var myEvent = new EventData(Guid.NewGuid(), @Event.GetType().ToString(), true,
                        Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(@Event)),
                        Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(metaData)));

                    connection.AppendToStreamAsync("test-apply",
                                                   ExpectedVersion.Any, myEvent).Wait();
                }

                aggregate.MarkChangesAsCommitted();
            }            
        }

        public T GetById(Guid id)
        {
            var streamEvents = new List<Event>();

            using (var connection = EventStoreConnection.Create(new Uri("tcp://admin:changeit@localhost:1113")))
            {
                connection.ConnectAsync().Wait();
               
                StreamEventsSlice currentSlice;
                var nextSliceStart = StreamPosition.Start;
                do
                {
                    currentSlice = connection.ReadStreamEventsForwardAsync("test-apply", nextSliceStart,
                                                                  200, false)
                                                                  .Result;

                    nextSliceStart = currentSlice.NextEventNumber;                    

                    streamEvents.AddRange(currentSlice.Events.Where(e => FilterById(e, id.ToString())).Select(e => ConvertToEvent(e.Event)).ToList());
                    
                } while (!currentSlice.IsEndOfStream);                
            }

            var aggregateRoot = new T();
            aggregateRoot.BuildFromHistory(streamEvents);
            return aggregateRoot;
        }

        private bool FilterById(ResolvedEvent recordedEvent, string id)
        {
            var data = JsonConvert.DeserializeObject<dynamic>(Encoding.UTF8.GetString(recordedEvent.Event.Data));

            return data.Id == id;
        }

        private Event ConvertToEvent(RecordedEvent @event)
        {
            return JsonConvert.DeserializeObject(Encoding.UTF8.GetString(@event.Data), Type.GetType(@event.EventType)) as Event;
        }
    }
}
