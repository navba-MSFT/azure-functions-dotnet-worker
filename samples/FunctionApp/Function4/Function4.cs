// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System.Net;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Core.Converters.Converter;
using Microsoft.Azure.Functions.Worker.Http;

namespace FunctionApp
{
    public static class Function4
    {
        [Function("Function4")]
        public static HttpResponseData RunComplex([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequestData req,
            FunctionContext executionContext,
            ProductViewModel productVm)
        {
            var response = req.CreateResponse(HttpStatusCode.OK);

            response.Headers.Add("Content-Type", "text/html; charset=utf-8");
            response.WriteString($"Product name:{productVm?.Name}, Price:{productVm?.Price}");

            return response;
        }
    }
}
