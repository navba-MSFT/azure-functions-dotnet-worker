// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using Microsoft.Azure.Functions.Worker.Pipeline;

namespace Microsoft.Azure.Functions.Worker.Converters
{
    public abstract class ConverterContext
    {
        /// <summary>
        /// The target type to which conversion should happen.
        /// </summary>
        public abstract Type TargetType { get; set; }
        
        /// <summary>
        /// A dictionary holding additional properties used for conversion.
        /// </summary>
        public abstract IReadOnlyDictionary<string, object> Properties { get; set;}

        public abstract object? Source { get; set; }

        public abstract FunctionContext FunctionContext { get; set; }
    }
}
