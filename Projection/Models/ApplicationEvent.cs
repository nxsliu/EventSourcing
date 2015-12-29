using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Projection.Repositories;

namespace Projection.Models
{
    public class ApplicationEvent
    {
        public ApplicationContent Content { get; set; }

        public void ProjectEvent(IApplicationStatusRepository repo)
        {
            if (repo.ApplicationExists(Content.Data.ApplicationId))
            {
                repo.UpdateApplication(Content);
            }
            else
            {
                repo.CreateApplication(Content);
            }
        }
    }

    public class ApplicationContent
    {
        public string EventId { get; set; }

        public string EventType { get; set; }

        public Application Data { get; set; }
    }

    public class Application
    {
        public string ApplicationId { get; set; }

        public string Name { get; set; }
    }
}
