using System;
using System.Collections.Generic;
using System.Linq;
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

    public class ApplyStream<T> where T : AggregateRoot
    {
        public void Save(AggregateRoot aggregate, string type, EventMetaData metaData)
        {
            using (var connection = EventStoreConnection.Create(new Uri("tcp://admin:changeit@localhost:1113")))
            {
                connection.ConnectAsync().Wait();

                foreach (var @Event in aggregate.GetUncommitedChanges())
                {
                    var myEvent = new EventData(Guid.NewGuid(), type, true,
                        Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(@Event)),
                        Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(metaData)));

                    connection.AppendToStreamAsync("test-apply",
                                                   ExpectedVersion.Any, myEvent).Wait();
                }                
            }
        }
    }
}
