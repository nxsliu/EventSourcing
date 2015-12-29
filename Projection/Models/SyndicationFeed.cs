using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projection.Models
{
    public class SyndicationFeed
    {
        public IEnumerable<Link> Links { get; set; }

        public IEnumerable<Entry> Entries { get; set; }
    }

    public class Link
    {
        public string Uri { get; set; }

        public string Relation { get; set; }
    }

    public class Entry
    {
        public IEnumerable<Link> Links { get; set; }
    }
}
