﻿// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System.Threading.Tasks;
using Microsoft.Azure.Functions.Worker.Sdk.Generators;
using Xunit;
namespace Sdk.Generator.Tests
{
    public class ExtensionStartupRunnerTests
    {
        const string expectedGeneratedFileName = $"ExtensionStartupRunner.g.cs";
        [Fact]
        public async Task StartupCodeGetsGenerated()
        {
            string inputCode = @"
public class Foo
{
}";

            string expectedOutput = @"// <auto-generated/>
using System;
using Microsoft.Azure.Functions.Worker;
namespace Microsoft.Azure.Functions.Worker
{
    [AttributeUsage(AttributeTargets.Class)]
    public class WorkerExtensionStartupAttribute : Attribute
    {
    }
    [WorkerExtensionStartup]
    internal class WorkerExtensionStartupRunner
    {
        public void RunStartupForExtensions(IFunctionsWorkerApplicationBuilder builder)
        {
            try
            {
                new Worker.Extensions.Sample.MySampleExtensionStartup().Configure(builder);
            }
            catch(Exception ex)
            {
                Console.WriteLine('Error calling Configure on Worker.Extensions.Sample.MySampleExtensionStartup instance.'+ex.ToString());
            }
        }
    }
}
".Replace("'","\"");
            var extensionAssemblies = new[]
            {
                    typeof(Worker.Extensions.Sample.MySampleExtensionStartup).Assembly,
            };

            await TestHelpers.RunTestAsync<ExtensionStartupRunnerGenerator>(
                extensionAssemblies,
                inputCode,
                expectedGeneratedFileName,
                expectedOutput);
        }
    }
}
