using System;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace FunctionApp
{
    public class MyServiceBusMessage
    {
        public int Id { get; set; }
        public string Name { set; get; }
        public string Location { set; get; }
    }
    public static class ServiceBusMsgTriggerFunction
    {
        [Function("ServiceBusMsgTriggerFunction")]
        public static void Run([ServiceBusTrigger("myqueue", Connection = "MyServiceBusConnStr")] MyServiceBusMessage myQueueItem, FunctionContext context)
        {
            var logger = context.GetLogger("ServiceBusMsgTriggerFunction");
            logger.LogInformation($"C# ServiceBus queue trigger function processed message: {myQueueItem}");
        }
    }
}
