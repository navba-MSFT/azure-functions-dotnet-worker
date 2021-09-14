// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Azure.Functions.Worker.Converters
{
    internal class MemoryConverter : IConverter
    {
        public ValueTask<ParameterBindingResult> ConvertAsync(ConverterContext context)
        {
            if (context.Source is not ReadOnlyMemory<byte> sourceMemory)
            {
                return new ValueTask<ParameterBindingResult>(ParameterBindingResult.Failed());
            }

            if (context.Parameter.Type.IsAssignableFrom(typeof(string)))
            {
                var target = Encoding.UTF8.GetString(sourceMemory.Span);
                return new ValueTask<ParameterBindingResult>(ParameterBindingResult.Success(target));
            }

            if (context.Parameter.Type.IsAssignableFrom(typeof(byte[])))
            {
                var target = sourceMemory.ToArray();
                return new ValueTask<ParameterBindingResult>(ParameterBindingResult.Success(target));
            }

            return new ValueTask<ParameterBindingResult>(ParameterBindingResult.Failed());
        }
    }
}
