// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System;

namespace Microsoft.Azure.Functions.Worker
{
    /// <summary>
    /// Function invocation trigger types.
    /// </summary>
    [Flags]
    public enum TriggerType
    {
        /// <summary>
        /// All triggers.
        /// </summary>
        All = 0,

        /// <summary>
        /// Http trigger.
        /// </summary>
        HttpTrigger = 1,

        /// <summary>
        /// Queue triggger.
        /// </summary>
        QueueTrigger = 2,

        /// <summary>
        /// Blob triggger.
        /// </summary>
        BlobTrigger = 4,
    }
}
