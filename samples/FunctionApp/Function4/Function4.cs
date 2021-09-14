// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System.Net;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Core.Converters.Converter;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;

namespace FunctionApp
{
    public static class Function4
    {
        //[Function("Function4")]
        //public static HttpResponseData Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequestData req, 
        //    FunctionContext executionContext,
        //    [BindingConverter(typeof(MyCustomerConverter))] CustomerViewModel customerViewModel)
        //{
        //    var response = req.CreateResponse(HttpStatusCode.OK);

        //    response.Headers.Add("Content-Type", "text/html; charset=utf-8");
        //    response.WriteString($"4 Product name:{customerViewModel?.Name}, Price:{customerViewModel?.Price}");

        //    return response;
        //}

                
        [Function("Function7")]
        public static HttpResponseData RunComplex([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequestData req, 
            FunctionContext executionContext,
            [ParameterBinder(typeof(MyComplexCustomerConverter))] CustomerViewModel customerViewModel)
        {
            var response = req.CreateResponse(HttpStatusCode.OK);

            response.Headers.Add("Content-Type", "text/html; charset=utf-8");
            response.WriteString($"4 Product name:{customerViewModel?.Name}, Price:{customerViewModel?.Price}");

            return response;
        }

                
        //[Function("Function6")]
        //public static HttpResponseData Run6([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequestData req, 
        //    FunctionContext executionContext,
        //    CustomerViewModel customerViewModel)
        //{
        //    var response = req.CreateResponse(HttpStatusCode.OK);

        //    response.Headers.Add("Content-Type", "text/html; charset=utf-8");
        //    response.WriteString($"6 Product name:{customerViewModel?.Name}, Price:{customerViewModel?.Price}");

        //    return response;
        //}
    }
}
