using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.Remoting;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Projection.Models;
using Projection.Repositories;

namespace Projection
{
    public class SubscriberService
    {
        private const string EventStoreHost = "http://127.0.0.1:2113";
        private const string AtomFeedJson = "application/vnd.eventstore.atom+json";

        private readonly NetworkCredential _eventStoreCredential;
        private readonly IApplicationStatusRepository _applicationStatusRepository;

        public SubscriberService()
        {
            _eventStoreCredential = new NetworkCredential("admin", "changeit");
            _applicationStatusRepository = new ApplicationStatusRepository();
        }

        public void Start()
        {
            ReadSteam();
        }

        public void Stop()
        {
        }

        private void ReadSteam()
        {
            var lastEventRead = _applicationStatusRepository.GetLastSuccessfulEvent();

            var currentUri = string.Format("/streams/applications/{0}/forward/2",
                lastEventRead == null ? 0 : lastEventRead + 1);

            while (true)
            {
                var previousUri = ReadBatch(currentUri);

                if (previousUri == currentUri)
                {
                    Task.Delay(1000).Wait();
                }

                currentUri = previousUri;
            }
        }

        private string ReadBatch(string uri)
        {
            using (var handler = new HttpClientHandler())
            {
                handler.Credentials = _eventStoreCredential;
                using (var client = new HttpClient(handler))
                {
                    client.BaseAddress = new Uri(EventStoreHost);
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(AtomFeedJson));

                    var response = client.GetAsync(uri).Result;

                    if (response.IsSuccessStatusCode)
                    {
                        var syndication = response.Content.ReadAsStringAsync().Result;

                        var feed = JsonConvert.DeserializeObject<SyndicationFeed>(syndication);

                        foreach (var entry in feed.Entries.Reverse())
                        {
                            LoadEvent(entry).Wait();
                        }

                        var previousLink = feed.Links.FirstOrDefault(l => l.Relation == "previous");

                        return previousLink != null ? previousLink.Uri : uri;
                    }
                    else
                    {
                        throw new Exception("Could not connect to Event Store");
                    }
                }
            }
        }

        private async Task LoadEvent(Entry entry)
        {
            using (var handler = new HttpClientHandler())
            {
                handler.Credentials = _eventStoreCredential;
                using (var client = new HttpClient(handler))
                {
                    client.BaseAddress = new Uri(EventStoreHost);
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(AtomFeedJson));

                    var response = await client.GetAsync(entry.Links.Single(l => l.Relation == "alternate").Uri);

                    if (response.IsSuccessStatusCode)
                    {
                        var entryEvent = await response.Content.ReadAsStringAsync();

                        var applicationEvent = JsonConvert.DeserializeObject<ApplicationEvent>(entryEvent);

                        applicationEvent.ProjectEvent(_applicationStatusRepository);
                    }
                }
            }
        }
    }
}