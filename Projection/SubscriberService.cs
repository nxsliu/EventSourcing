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

namespace Projection
{
    public class SubscriberService
    {
        private const string EventStoreHost = "http://127.0.0.1:2113";
        private const string AtomFeedJson = "application/vnd.eventstore.atom+json";

        private readonly NetworkCredential _eventStoreCredential;

        public SubscriberService()
        {
            _eventStoreCredential = new NetworkCredential("admin", "changeit");
        }

        public void Start()
        {
            ReadSteam().Wait();
        }

        public void Stop()
        {
        }

        private async Task ReadSteam()
        {
            using (var handler = new HttpClientHandler())
            {
                handler.Credentials = _eventStoreCredential;
                using (var client = new HttpClient(handler))
                {
                    client.BaseAddress = new Uri(EventStoreHost);
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(AtomFeedJson));

                    HttpResponseMessage response = await client.GetAsync("/streams/applications/0/forward/10");

                    if (response.IsSuccessStatusCode)
                    {
                        var syndication = await response.Content.ReadAsStringAsync();

                        var feed = JsonConvert.DeserializeObject<SyndicationFeed>(syndication);

                        foreach (var entry in feed.Entries.Reverse())
                        {
                            LoadEvent(entry).Wait();
                        }
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

                    HttpResponseMessage response =
                        await client.GetAsync(entry.Links.Single(l => l.Relation == "alternate").Uri);

                    if (response.IsSuccessStatusCode)
                    {
                        var entryEvent = await response.Content.ReadAsStringAsync();

                        var applicationEvent = JsonConvert.DeserializeObject<ApplicationEvent>(entryEvent);

                        
                    }
                }
            }
        }
    }
}