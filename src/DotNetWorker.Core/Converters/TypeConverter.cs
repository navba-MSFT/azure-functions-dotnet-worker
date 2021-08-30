// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System;
using System.Threading.Tasks;

namespace Microsoft.Azure.Functions.Worker.Converters
{
    internal class TypeConverter : IConverter
    {
        public ValueTask<BindingResult> ConvertAsync(ConverterContext context)
        {    
            Type? sourceType = context.Source?.GetType();

            if (sourceType is not null &&
                context.Parameter.Type.IsAssignableFrom(sourceType))
            {
                var bindingResult = BindingResult.Success(context.Source);
                return new ValueTask<BindingResult>(bindingResult);
            }
                        
            return new ValueTask<BindingResult>(BindingResult.Failed());
        }
    }
}
