using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using EventStore.ClientAPI;

namespace EventStore
{
    public class Writer
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
}
