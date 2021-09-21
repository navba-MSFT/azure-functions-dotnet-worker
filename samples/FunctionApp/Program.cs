// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace FunctionApp
{
    class Program
    {
        static async Task Main(string[] args)
        {
// #if DEBUG
//          Debugger.Launch();
// #endif
            //<docsnippet_startup>
            var host = new HostBuilder()
                //<docsnippet_configure_defaults>
                .ConfigureFunctionsWorkerDefaults((workerApplication) =>
                {
                    workerApplication.UseMiddleware<MyMiddleware>();
                })
                //</docsnippet_configure_defaults>
                //.ConfigureFunctionsWorkerDefaults((appBuilder)=>{ }, (options) =>
                //{
                //    options.BindingConverters.Add(typeof(MyCustomerConverter));
                //})
                //<docsnippet_dependency_injection>
                .ConfigureServices(s =>
                {
                    s.AddHttpClient();
                    s.AddSingleton<IHttpResponderService, DefaultHttpResponderService>();
                })
                //</docsnippet_dependency_injection>
                .Build();
            //</docsnippet_startup>

            //<docsnippet_host_run>
            await host.RunAsync();
            //</docsnippet_host_run>
        }
    }
}
