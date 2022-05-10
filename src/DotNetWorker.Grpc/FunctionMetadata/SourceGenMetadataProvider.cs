// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Threading.Tasks;
using Microsoft.Azure.Functions.Worker.Grpc.Messages;

namespace Microsoft.Azure.Functions.Worker
{
    internal class SourceGenMetadataProvider : IFunctionMetadataProvider
    {
        public virtual async Task<ImmutableArray<RpcFunctionMetadata>> GetFunctionMetadataAsync(string directory)
        {
            var metadataList = new List<RpcFunctionMetadata>();

            var functionMetadata = new RpcFunctionMetadata();

            // hard-coded values that are checked for when the host validates functions
            functionMetadata.IsProxy = false;
            functionMetadata.Language = "dotnet-isolated";
            functionMetadata.FunctionId = Guid.NewGuid().ToString();
            functionMetadata.Name = "HttpTriggerSimple";
            functionMetadata.ScriptFile = "FunctionApp.dll";
            functionMetadata.EntryPoint = "FunctionApp.HttpTriggerSimple.Run";

            // create raw bindings
            var binding1 =
            @"{
                ""name"": ""req"",
                ""type"": ""httpTrigger"",
                ""direction"": ""In"",
                ""authLevel"": ""Anonymous"",
                ""methods"": [ ""get"",""post""]
            }";
            functionMetadata.RawBindings.Add(binding1);
            var bindingInfo1 = new BindingInfo();
            Enum.TryParse("In", out BindingInfo.Types.Direction direction);
            bindingInfo1.Direction = direction;
            bindingInfo1.Type = "httpTrigger";
            functionMetadata.Bindings.Add("req", bindingInfo1);

            var binding2 =
            @"{
                ""name"": ""$return"",
                ""type"": ""http"",
                ""direction"": ""Out""
            }";
            functionMetadata.RawBindings.Add(binding2);
            var bindingInfo2 = new BindingInfo();
            Enum.TryParse("Out", out BindingInfo.Types.Direction direction2);
            bindingInfo2.Direction = direction2;
            bindingInfo2.Type = "http";
            functionMetadata.Bindings.Add("$return", bindingInfo2);

            metadataList.Add(functionMetadata);
            var metadataTask = Task.FromResult(metadataList.ToImmutableArray());
            return await metadataTask;
        }
    }
}
