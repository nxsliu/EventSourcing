﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RabbitMQ.Client;
using RabbitMQ.Client.Framing;
using MessageQueue;

namespace ApplyAPI.Controllers
{
    public class ApplyController : ApiController
    {
        private Publisher _publisher;

        public IEnumerable<string> Get()
        {
            return new[] {"Poop"};
        }

        public ApplyController()
        {
            _publisher = new Publisher();
        }

        public HttpResponseMessage Post([FromBody]JToken apply)
        {                                    
            try
            {
                var application = JsonConvert.DeserializeObject<dynamic>(apply.ToString());
                var headers = new Dictionary<string, object>() {{"ApplicationType", (string) application.ApplicationType}};                

                _publisher.PublishEvent("ApplicationSubmission", apply.ToString(), Guid.NewGuid().ToString(), headers);

                return Request.CreateResponse(HttpStatusCode.Created);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex);
            }            
        }
    }
}
