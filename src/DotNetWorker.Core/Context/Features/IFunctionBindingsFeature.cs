// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System.Collections.Generic;
using Microsoft.Azure.Functions.Worker.OutputBindings;

namespace Microsoft.Azure.Functions.Worker.Context.Features
{
    /// <summary>
    /// Provides the input and output data for a function invocation.
    /// </summary>
    public interface IFunctionBindingsFeature
    {
        /// <summary>
        /// Gets the trigger meta data for the function invocation.
        /// </summary>
        public IReadOnlyDictionary<string, object?> TriggerMetadata { get; }

        /// <summary>
        /// Gets the input data for the function invocation.
        /// </summary>
        public IReadOnlyDictionary<string, object?> InputData { get; }

        /// <summary>
        /// Gets the output binding data for the function invocation.
        /// </summary>
        public IDictionary<string, object?> OutputBindingData { get;}

        /// <summary>
        /// Gets the output binding info for the function.
        /// </summary>
        public OutputBindingsInfo OutputBindingsInfo { get; }

        /// <summary>
        /// Gets or sets the invocation result.
        /// </summary>
        public object? InvocationResult { get; set; }

        /// <summary>
        /// Sets the output binding data.
        /// </summary>
        /// <param name="name">Name of the output binding.</param>
        /// <param name="value">Value of the output binding.</param>
        void SetOutputBindingData(string name, object? value);
    }
}
