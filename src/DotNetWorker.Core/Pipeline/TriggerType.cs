// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

namespace Microsoft.Azure.Functions.Worker
{
    /// <summary>
    ///  Trigger type names constant.
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
