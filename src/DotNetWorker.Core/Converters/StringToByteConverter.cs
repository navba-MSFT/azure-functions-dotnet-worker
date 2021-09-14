// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Azure.Functions.Worker.Converters
{
    internal class StringToByteConverter : IConverter
    {
        public ValueTask<ParameterBindingResult> ConvertAsync(ConverterContext context)
        {
            if (!(context.Parameter.Type.IsAssignableFrom(typeof(byte[])) &&
                  context.Source is string sourceString))
            {
                return new ValueTask<ParameterBindingResult>(ParameterBindingResult.Failed());
            }

            var byteArray = Encoding.UTF8.GetBytes(sourceString);
            var bindingResult = ParameterBindingResult.Success(byteArray);

            return new ValueTask<ParameterBindingResult>(bindingResult);
        }
    }
}
