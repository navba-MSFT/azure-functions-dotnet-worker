// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System;
using Microsoft.Azure.Functions.Worker.Context.Features;
using Microsoft.Azure.Functions.Worker.Http;

namespace Microsoft.Azure.Functions.Worker
{
    /// <summary>
    /// FunctionContext extensions for http invocations.
    /// </summary>
    public static class FunctionContextHttpRequestExtensions
    {
        /// <summary>
        /// Gets the HttpRequestData instance if the invocation is http trigger.
        /// </summary>
        /// <param name="context">The FunctionContext instance.</param>
        /// <returns>HttpRequestData instance if the invocation is http, else null</returns>
        public static HttpRequestData? GetHttpRequestData(this FunctionContext context)
        {
            var bindingsFeature = context.GetBindings();

            if (bindingsFeature == null)
            {
                throw new InvalidOperationException($"{nameof(IFunctionBindingsFeature)} is missing.");
            }

            HttpRequestData? httpRequestData = null;
            foreach (var input in bindingsFeature.InputData)
            {
                if (input.Value is HttpRequestData httpRequestDataFromInput)
                {
                    httpRequestData = httpRequestDataFromInput;
                    break;
                }
            }

            return httpRequestData;
        }
    }
}
