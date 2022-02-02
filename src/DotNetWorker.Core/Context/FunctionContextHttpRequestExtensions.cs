// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Microsoft.Azure.Functions.Worker.Http;

namespace Microsoft.Azure.Functions.Worker
{
    /// <summary>
    /// FunctionContext extensions for http trigger function invocations.
    /// </summary>
    public static class FunctionContextHttpRequestExtensions
    {
        /// <summary>
        /// Gets the <see cref="HttpRequestData"/> instance if the invocation is for an http trigger.
        /// </summary>
        /// <param name="context">The FunctionContext instance.</param>
        /// <returns>HttpRequestData instance if the invocation is http, else null</returns>
        public static HttpRequestData? GetHttpRequestData(this FunctionContext context)
        {
            var bindingsFeature = context.GetBindings();

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
