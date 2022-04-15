// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System;

namespace Microsoft.Azure.Functions.Worker
{
    /// <summary>
    /// Trigger type enum.
    /// </summary>
     [Flags]
    public enum TriggerType
    {
        /// <summary>
        /// All trigger
        /// </summary>
        All = 0,
        /// <summary>
        /// Http trigger
        /// </summary>
        HttpTrigger = 1,

        /// <summary>
        /// Queue triggger
        /// </summary>
        QueueTrigger =2 ,

        /// <summary>
        /// Blob triggger
        /// </summary>
        BlobTrigger = 4,
    }

    /// <summary>
    ///  Trigger type names constant.
    ///  Similar to https://github.com/dotnet/aspnetcore/blob/main/src/Http/Headers/src/HeaderNames.cs
    /// </summary>
    public static class TriggerTypeNames
    {
        /// <summary>
        /// Gets the HTTP trigger name.
        /// </summary>
        public static readonly string Http = "httpTrigger";

        /// <summary>
        /// Gets the Queue trigger name.
        /// </summary>
        public static readonly string Queue = "queueTrigger";

        /// <summary>
        /// Gets the Blob trigger name.
        /// </summary>
        public static readonly string Blob = "blobTrigger";
    }
}
