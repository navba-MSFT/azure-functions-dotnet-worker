// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System.Net;
using Microsoft.Azure.Functions.Worker.Context.Features;
using Microsoft.Azure.Functions.Worker.Http;

namespace Microsoft.Azure.Functions.Worker.Grpc.Features
{
    public static class FunctionContextExtensions
    {
        public static HttpRequestData? GetHttpRequestData(this FunctionContext context)
        {
            var feature = context.Features.Get<IFunctionBindingsFeature>();

            HttpRequestData? httpRequestData = null;
            foreach (var input in feature.InputData)
            {
                if (input.Value is HttpRequestData grpcHttpRequestData)
                {
                    httpRequestData = grpcHttpRequestData;
                    break;
                }
            }

            return httpRequestData;
        }

        public static HttpResponseData CreateHttpResponse(this FunctionContext context, HttpStatusCode httpStatusCode)
        {
            return new GrpcHttpResponseData(context, httpStatusCode);
        }
    }
}
