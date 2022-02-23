// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System;
using Microsoft.Azure.Functions.Worker;

namespace FunctionApp
{
    public class MyBlob
    {
        public string Id { get; set; }
        public string Name { get; set; }
    }

    public static class Function2
    {
        [Function("Function2")]
        public static Book Run(
            [QueueTrigger("functionstesting2", Connection = "MyStorageConnStr")] Book myQueueItem,
            [BlobInput("test-samples/base.json", Connection = "MyStorageConnStr")] MyBlob baseBlob,
            [BlobInput("test-samples/specific.json", Connection = "MyStorageConnStr")] MyBlob sampleBlob)
        {
            Console.WriteLine(sampleBlob);
            return myQueueItem;
        }
    }

    public class Book
    {
        public string name { get; set; }
        public string id { get; set; }
    }

}
