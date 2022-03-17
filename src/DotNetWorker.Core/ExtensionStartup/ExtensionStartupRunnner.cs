﻿// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System;
using System.Linq;
using System.Reflection;

namespace Microsoft.Azure.Functions.Worker.Core
{
    internal class ExtensionStartupRunnner
    {
        private const string StartupAttributeName = nameof(WorkerExtensionStartupAttribute);

        internal static void RunExtensionStartupCode(IFunctionsWorkerApplicationBuilder builder)
        {
            // Find the auto(source) generated class(WorkerExtensionStartupRunner)
            var generatedStartupRunnerType = (Assembly.GetEntryAssembly())!
                                            .GetTypes()
                                            .FirstOrDefault(type => type.GetCustomAttributes()
                                                                        .Any(attr => attr.GetType().Name == StartupAttributeName));

            // Our source generator will not create the file when no extension startup hooks are found.
            if (generatedStartupRunnerType == null)
            {
                return;
            }


            var method = generatedStartupRunnerType.GetMethod("RunStartupForExtensions");
            if (method == null)
            {
                throw new InvalidOperationException($"Types decorated with {StartupAttributeName} must have a RunStartupForExtensions method.");
            }

            var startupRunnerInstance = Activator.CreateInstance(generatedStartupRunnerType);
            method.Invoke(startupRunnerInstance, parameters: new[] { builder });
        }
    }
}