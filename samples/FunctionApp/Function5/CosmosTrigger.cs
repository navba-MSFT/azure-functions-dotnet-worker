// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System.Collections.Generic;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace FunctionApp
{
    public class CosmosTrigger
    {
        private readonly ILogger _logger;

        public CosmosTrigger(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<CosmosTrigger>();
        }

        [Function("CosmosTrigger")]
        public void Run([CosmosDBTrigger(
            databaseName: "items-db",
            collectionName: "products-container",
            ConnectionStringSetting = "MyCosmosDbConnStr",
            LeaseCollectionName = "leases")] IReadOnlyList<MyDocument> myDocuments)
        {
            if (myDocuments != null && myDocuments.Count > 0)
            {
                _logger.LogInformation("Documents modified: " + myDocuments.Count);
                _logger.LogInformation("First document Id: " + myDocuments[0].Id + ", name:" + myDocuments[0].Name);
            }
        }
    }
}
