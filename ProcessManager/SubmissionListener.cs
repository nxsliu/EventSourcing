using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ProcessManager
{
    public class SubmissionListener
    {
        public void Start()
        {
            ListenToSubmissionEvents();
        }

        public void Stop()
        {

        }

        private void ListenToSubmissionEvents()
        {
            while (true)
            {
                // connect to Mq;
                // get event from message;
                // save event to event store
                // send event to process manager
            }
        }
    }
}
