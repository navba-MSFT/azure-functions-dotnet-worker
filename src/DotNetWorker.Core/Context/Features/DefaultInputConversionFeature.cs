// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Azure.Functions.Worker.Converters;
using Microsoft.Azure.Functions.Worker.Core.Converters;
using Microsoft.Azure.Functions.Worker.Core.Converters.Converter;

namespace Microsoft.Azure.Functions.Worker.Context.Features
{
    internal sealed class DefaultInputConversionFeature : IFunctionInputConversionFeature
    {
        private readonly IEnumerable<IFunctionInputConverter> _converters;
        private readonly IFunctionInputConverterProvider _converterProvider;

        public DefaultInputConversionFeature(IFunctionInputConverterProvider converterProvider)
        {
            _converters = converterProvider.DefaultConverters;
            _converterProvider = converterProvider ?? throw new ArgumentNullException(nameof(converterProvider));
        }

        public async ValueTask<ConversionResult> TryConvertAsync(ConverterContext context)
        {
            // Check a converter is explicitly passed via the converter context.
            IFunctionInputConverter? converterFromContext = GetConverterFromContext(context);

            if (converterFromContext != null)
            {
                var bindingResult = await converterFromContext.ConvertAsync(context);
                return bindingResult;
            }

            // Use the globally registered converters
            // The first converter to successfully convert wins.
            // For example, this allows a converter that parses JSON strings to return false if the
            // string is not valid JSON. This manager will then continue with the next matching provider.
            foreach (var converter in _converters)
            {
                var bindingResult = await converter.ConvertAsync(context);
                if (bindingResult.IsSuccess)
                {
                    return bindingResult;
                }
            }

            return default;
        }

        private IFunctionInputConverter? GetConverterFromContext(ConverterContext context)
        {
            Type? converterType = default;
                        
            // Check a converter is specified on the conversionContext.Properties. If yes,use that.
            if (context.Properties!=null && context.Properties.TryGetValue(PropertyBagKeys.ConverterType, out var converterTypeObj))
            {
                converterType = (Type)converterTypeObj;
            }
            else
            {
                // check the class used as TargetType has a BindingConverter attribute decoration.
                var binderType = typeof(InputConverterAttribute);
                var binderAttr = context.TargetType.GetCustomAttributes(binderType, inherit: true).FirstOrDefault();

                if (binderAttr != null)
                {
                    converterType = ((InputConverterAttribute)binderAttr).ConverterType;
                }
            }

            if (converterType != null)
            {
                return this._converterProvider.GetConverterInstance(converterType);
            }

            return null;
        }
    }
}
