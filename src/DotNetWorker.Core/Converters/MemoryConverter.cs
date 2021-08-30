// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Azure.Functions.Worker.Converters
{
    internal class MemoryConverter : IConverter
    {
        public ValueTask<BindingResult> ConvertAsync(ConverterContext context)
        {
            if (context.Source is not ReadOnlyMemory<byte> sourceMemory)
            {
                return new ValueTask<BindingResult>(BindingResult.Failed());
            }

            if (context.Parameter.Type.IsAssignableFrom(typeof(string)))
            {
                var target = Encoding.UTF8.GetString(sourceMemory.Span);
                return new ValueTask<BindingResult>(BindingResult.Success(target));
            }

            if (context.Parameter.Type.IsAssignableFrom(typeof(byte[])))
            {
                var target = sourceMemory.ToArray();
                return new ValueTask<BindingResult>(BindingResult.Success(target));
            }

            return new ValueTask<BindingResult>(BindingResult.Failed());
        }
    }
}
