// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System;
using System.Threading.Tasks;

namespace Microsoft.Azure.Functions.Worker.Converters
{
    internal class TypeConverter : IConverter
    {
        public ValueTask<ParameterBindingResult> ConvertAsync(ConverterContext context)
        {    
            Type? sourceType = context.Source?.GetType();

            if (sourceType is not null &&
                context.Parameter.Type.IsAssignableFrom(sourceType))
            {
                var bindingResult = ParameterBindingResult.Success(context.Source);
                return new ValueTask<ParameterBindingResult>(bindingResult);
            }
                        
            return new ValueTask<ParameterBindingResult>(ParameterBindingResult.Failed());
        }
    }
}
