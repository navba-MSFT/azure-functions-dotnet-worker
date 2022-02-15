using System;
using System.Collections.Generic;
using System.Net;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;

namespace FunctionApp
{
    public class MyOutgoingQueueMsg
    {
        public string Name { get; set; }
    }
    public static class HttpWriteToQueue
    {
        [Function("HttpWriteToQueue")]
        [QueueOutput("myqueue-items-b", Connection = "MyStorageConnStr")]

        public static MyOutgoingQueueMsg Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post")] HttpRequestData req,
            FunctionContext executionContext)
        {

            var response = "foo-"+DateTime.Now;

            return new MyOutgoingQueueMsg{ Name =response };
        }
    }
}
