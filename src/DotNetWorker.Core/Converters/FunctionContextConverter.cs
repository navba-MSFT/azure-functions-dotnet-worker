// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System.Threading.Tasks;

namespace Microsoft.Azure.Functions.Worker.Converters
{
    internal class FunctionContextConverter : IConverter
    {
        public ValueTask<BindingResult> ConvertAsync(ConverterContext context)
        {
            // Special handling for the context.
            if (context.Parameter.Type == typeof(FunctionContext))
            {
                return new ValueTask<BindingResult>(BindingResult.Success(context.FunctionContext));
            }

            return new ValueTask<BindingResult>(BindingResult.Failed());
        }
    }
}
