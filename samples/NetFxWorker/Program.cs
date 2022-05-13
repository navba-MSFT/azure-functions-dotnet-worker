using System;
using System.Diagnostics;
using Microsoft.Extensions.Hosting;
using Microsoft.Azure.Functions.Worker;
using NetFxWorker;

namespace ConsoleApp8
{
    internal class Program
    {
        static void Main(string[] args)
        {
            FunctionsDebugger.Enable();

            var host = new HostBuilder()
                .ConfigureFunctionsWorkerDefaults()
                .Build();

            host.Run();
        }
    }
}