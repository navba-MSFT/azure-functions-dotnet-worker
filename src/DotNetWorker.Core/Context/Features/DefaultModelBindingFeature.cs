// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Azure.Functions.Worker.Converters;
using Microsoft.Azure.Functions.Worker.Core.Converters.Converter;
using Microsoft.Azure.Functions.Worker.Diagnostics.Exceptions;
using Microsoft.Extensions.DependencyInjection;

namespace Microsoft.Azure.Functions.Worker.Context.Features
{
    internal class DefaultModelBindingFeature : IModelBindingFeature
    {
        private readonly IEnumerable<IConverter> _converters;
        private bool _inputBound;
        private object?[]? _parameterValues;

        public DefaultModelBindingFeature(IEnumerable<IConverter> converters)
        {
            _converters = converters ?? throw new ArgumentNullException(nameof(converters));
        }

        public object?[]? InputArguments => _parameterValues;

        public async ValueTask<object?[]> BindFunctionInputAsync(FunctionContext context)
        {
            if (_inputBound)
            {
                throw new InvalidOperationException("Duplicate binding call detected. " +
                    $"Input parameters can only be bound to arguments once. Use the {nameof(InputArguments)} property to inspect values.");
            }

            _parameterValues = new object?[context.FunctionDefinition.Parameters.Length];
            _inputBound = true;

            List<string>? errors = null;
            for (int i = 0; i < _parameterValues.Length; i++)
            {
                FunctionParameter param = context.FunctionDefinition.Parameters[i];

                IFunctionBindingsFeature functionBindings = context.GetBindings();

                // Check InputData first, then TriggerMetadata
                if (!functionBindings.InputData.TryGetValue(param.Name, out object? source))
                {
                    functionBindings.TriggerMetadata.TryGetValue(param.Name, out source);
                }

                var converterContext = new DefaultConverterContext(param, source, context);

                var bindingResult = await TryConvertAsync(converterContext);

                if (bindingResult.IsSuccess)
                {
                    _parameterValues[i] = bindingResult.Model;
                }
                else if (source is not null)
                {
                    // Don't initialize this list unless we have to
                    if (errors is null)
                    {
                        errors = new List<string>();
                    }

                    errors.Add($"Cannot convert input parameter '{param.Name}' to type '{param.Type.FullName}' from type '{source.GetType().FullName}'.");
                }
            }

            // found errors
            if (errors is not null)
            {
                throw new FunctionInputConverterException($"Error converting {errors.Count} input parameters for Function '{context.FunctionDefinition.Name}': {string.Join(Environment.NewLine, errors)}");
            }

            return _parameterValues;
        }

        // to do: DI Inject this via another class (BinderCacheProvider ?)
        static readonly ConcurrentDictionary<Type, IConverter> binderTypeToConverterCache = new ConcurrentDictionary<Type, IConverter>();

        internal async ValueTask<ParameterBindingResult> TryConvertAsync(ConverterContext context)
        {
            IConverter? parameterSpecificConverter = GetConverterSpecificToParameter(context);

            if (parameterSpecificConverter!= null)
            {
                var bindingResult = await parameterSpecificConverter.ConvertAsync(context);
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

        private static IConverter? GetConverterSpecificToParameter(ConverterContext context)
        {
            // Check a converter is specified on the method parameter. If yes,use that.
            // ex: [BindingConverter(typeof(MyComplexCustomerConverter))] CustomerViewModel customerViewModel

            Type? converterType = default;
            IConverter? parameterSpecificConverter = default;

            var bindingConverterTypeFromParam = context.Parameter.BindingConverterType;
            if (bindingConverterTypeFromParam != null)
            {
                converterType = bindingConverterTypeFromParam;
            }
            else
            {
                // check the class used as method parameter has a BindingConverter attribute decoration.
                var binderType = typeof(ParameterBinderAttribute);
                var binderAttr = context.Parameter.Type.GetCustomAttributes(binderType, inherit: true).FirstOrDefault();

                if (binderAttr != null)
                {
                    converterType = ((ParameterBinderAttribute)binderAttr).ConverterType;
                }
            }
          
            if (converterType != null)
            {
                if (binderTypeToConverterCache.TryGetValue(converterType, out var converterFromCache))
                {
                    parameterSpecificConverter = converterFromCache;
                }
                else
                {
                    parameterSpecificConverter = (IConverter)ActivatorUtilities.CreateInstance(context.FunctionContext.InstanceServices, converterType);
                    binderTypeToConverterCache[converterType] = parameterSpecificConverter;
                }

            }

            return parameterSpecificConverter;
        }
    }
}
