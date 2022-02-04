// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

namespace Microsoft.Azure.Functions.Worker
{
    /// <summary>
    /// A type representing an output binding entry.
    /// </summary>
    public class BindingData
    {
        internal BindingData(string name, object? value, string? type)
        {
            Name = name;
            Value = value;
            Type = type;
        }

        /// <summary>
        /// Gets the name of the output binding entry.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Gets the value of the output binding entry.
        /// </summary>
        public object? Value { get; }

        /// <summary>
        /// Gets the type of the output binding entry.
        /// Ex: "http","queue" etc.
        /// </summary>
        public string? Type { get; }
    }
}
