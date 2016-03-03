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
using Projection.Events;
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

            var currentUri = $"/streams/test-apply/{(lastEventRead == null ? 0 : lastEventRead + 1)}/forward/5";

            while (true)
            {
                var previousUri = ReadBatch(currentUri).Result;

                if (previousUri == currentUri)
                {
                    Task.Delay(1000).Wait();
                }

                currentUri = previousUri;
            }
        }

        private async Task<string> ReadBatch(string uri)
        {
            using (var handler = new HttpClientHandler())
            {
                handler.Credentials = _eventStoreCredential;
                using (var client = new HttpClient(handler))
                {
                    client.BaseAddress = new Uri(EventStoreHost);
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(AtomFeedJson));

                    var response = await client.GetAsync(uri);

                    if (response.IsSuccessStatusCode)
                    {
                        var syndication = await response.Content.ReadAsStringAsync();

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

                        var eventWrapper = JsonConvert.DeserializeObject<StreamEvent>(entryEvent);

                        // strip the NameSpace from the eventType if it exists 
                        var className =
                            eventWrapper.Content.EventType.Substring(eventWrapper.Content.EventType.LastIndexOf('.') + 1);

                        var eventTypeClass = Type.GetType($"Projection.Events.{className}");

                        if (eventTypeClass != null)
                        {
                            var applyEvent = JsonConvert.DeserializeObject(eventWrapper.Content.Data.ToString(),
                                eventTypeClass) as IEvent;

                            applyEvent?.Execute(_applicationStatusRepository, eventWrapper.Content.EventNumber);
                        }
                    }
                    else
                    {
                        throw new Exception("Could not connect to Event Store");
                    }
                }
            }
        }
    }
}