// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;

namespace Microsoft.Azure.Functions.Worker.Converters
{
    internal class DefaultConverterContext : ConverterContext
    {
        public DefaultConverterContext(Type targetType, object? source, FunctionContext context)
        {
            FunctionContext = context ?? throw new ArgumentNullException(nameof(context));
            Source = source;                        
            TargetType = targetType;
        }

        public override object? Source { get; set; }

        public override FunctionContext FunctionContext { get; set; }
                
        public override IReadOnlyDictionary<string, object> Properties { get; set;}

        public override Type TargetType { get; set; }
    }
}
