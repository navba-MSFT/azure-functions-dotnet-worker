// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System.Net;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;

namespace FunctionApp
{
    public static class Function4
    {
        [Function("Function4")]
        public static HttpResponseData Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequestData req, 
            FunctionContext executionContext,
            //[MyBinder]
            ProductViewModel productViewModel)
        {
            var response = req.CreateResponse(HttpStatusCode.OK);

            response.Headers.Add("Content-Type", "text/html; charset=utf-8");
            response.WriteString($"Product name:{productViewModel?.Name}, Price:{productViewModel?.Price}");

            return response;
        }
    }
}
