// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System.Threading.Tasks;
using Microsoft.Azure.Functions.Worker.Converters;

namespace Microsoft.Azure.Functions.Worker.Context.Features
{
    /// <summary>
    /// A feature which allow us to do a single conversion from a source to target type.
    /// </summary>
    public interface IConversionFeature
    {
        ValueTask<ConversionResult> TryConvertAsync(ConverterContext context);
    }
}
