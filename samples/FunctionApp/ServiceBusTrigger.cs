using System;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace FunctionApp
{
    public class ServiceBusTrigger
    {
        private readonly ILogger _logger;

        public ServiceBusTrigger(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<ServiceBusTrigger>();
        }

        [Function("ServiceBusTrigger")]
        public void Run([ServiceBusTrigger("myqueue", Connection = "MyServiceBusConnStr")] string myQueueItem)
        {
            _logger.LogInformation($"C# ServiceBus queue trigger function processed message: {myQueueItem}");
        }
    }
}
