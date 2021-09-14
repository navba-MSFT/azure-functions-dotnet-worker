// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System;
using System.Threading.Tasks;

namespace Microsoft.Azure.Functions.Worker.Converters
{
    /// <summary>
    /// Converter to bind Guid/Guid? type parameters.
    /// </summary>
    internal class GuidConverter : IConverter
    {
        public ValueTask<ParameterBindingResult> ConvertAsync(ConverterContext context)
        {
            if (context.Parameter.Type == typeof(Guid) || context.Parameter.Type == typeof(Guid?))
            {
                if (context.Source is string sourceString && Guid.TryParse(sourceString, out Guid parsedGuid))
                {
                    return new ValueTask<ParameterBindingResult>(ParameterBindingResult.Success(parsedGuid));
                }
            }

            return new ValueTask<ParameterBindingResult>(ParameterBindingResult.Failed());
        }
    }
}
