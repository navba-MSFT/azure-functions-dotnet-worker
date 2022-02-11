// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Azure.Functions.Worker.Context.Features;
using Microsoft.Azure.Functions.Worker.Converters;
using Microsoft.Extensions.DependencyInjection;

namespace Microsoft.Azure.Functions.Worker
{
    /// <summary>
    /// FunctionContext extension methods for binding data.
    /// </summary>
    public static class FunctionContextBindingFeatureExtensions
    {
        /// <summary>
        /// Gets the input binding data for the current function invocation.
        /// </summary>
        /// <param name="context">The function context instance.</param>
        /// <returns>The input binding data as a read only dictionary.</returns>
        public static IReadOnlyDictionary<string, object?> GetInputData(this FunctionContext context)
        {
            return context.GetBindings().InputData;
        }

        /// <summary>
        /// Gets the trigger meta data for the current function invocation.
        /// </summary>
        /// <param name="context">The function context instance.</param>
        /// <returns>The invocation trigger meta data as a read only dictionary.</returns>
        public static IReadOnlyDictionary<string, object?> GetTriggerMetadata(this FunctionContext context)
        {
            return context.GetBindings().TriggerMetadata;
        }

        public async static Task<object> BindInput(this FunctionContext context, BindingMetadata bindingMetadata)
        {
            // find the parameter from function definition for the bindingMetadata requested.
            // Use that parameter definition(which has Type) to get converted value.

            Type paramType;
            FunctionParameter parameter = null;
            foreach (var param in context.FunctionDefinition.Parameters)
            {
                if (param.Name==bindingMetadata.Name)
                {                    
                    parameter = param;
                    paramType= param.Type;
                    break;
                }
            }
            if (parameter != null)
            {
                var ts = await GetConvertedValueFromFeature(context, parameter);
                var result = new BindingData<object>(context, bindingMetadata.Name, ts, bindingMetadata.Type);

                return result;
            }
            return default;

        }
        public async static Task<BindingData<T>> BindInput<T>(this FunctionContext context)
        {
            var inputType = typeof(T);

            // find the parameter from function definition for the Type requested.
            // Use that parameter definition(which has Type) to get converted value.


            FunctionParameter parameter = null;
            foreach (var param in context.FunctionDefinition.Parameters)
            {
                if (param.Type == inputType)
                {
                    if (parameter != null)
                    {
                        // means more than one parameter found with the type requested.
                        // customer should use the other API.
                        throw new InvalidOperationException("!@#$ Use the other BindInput overload");
                    }
                    parameter = param;
                }
            }


            if (parameter != null)
            {

                var ipbindsin= context.FunctionDefinition.InputBindings;
                var inputBinding = ipbindsin.First(a=>a.Key == parameter.Name);

                var ts = await GetConvertedValueFromFeature(context, parameter);

                T tts= (T)ts;
                var result = new BindingData<T>(context, inputBinding.Value.Name, tts, inputBinding.Value.Type);

                return result;
            }

            return default;
        }

        private static async Task<object> GetConvertedValueFromFeature(FunctionContext context, FunctionParameter parameter)
        {
            IFunctionBindingsFeature functionBindings = context.GetBindings();

            var converterContextFactory = context.InstanceServices.GetService<IConverterContextFactory>();

            var inputConversionFeature = context.Features.Get<IInputConversionFeature>();

            // Check InputData first, then TriggerMetadata
            if (!functionBindings.InputData.TryGetValue(parameter.Name, out object? source))
            {
                functionBindings.TriggerMetadata.TryGetValue(parameter.Name, out source);
            }

            var converterContext = converterContextFactory!.Create(parameter.Type, source, context);

            var bindingResult = await inputConversionFeature!.ConvertAsync(converterContext);
            object ts = bindingResult.Value;
            return ts;
        }


        /// <summary>
        /// Gets the invocation result of the current function invocation.
        /// </summary>
        /// <param name="context">The function context instance.</param>
        /// <returns>The invocation result value.</returns>
        public static object? GetInvocationResult(this FunctionContext context)
        {
            return context.GetBindings().InvocationResult;
        }

        /// <summary>
        /// Sets the invocation result of the current function invocation.
        /// </summary>
        /// <param name="context">The function context instance.</param>
        /// <param name="value">The invocation result value.</param>
        public static void SetInvocationResult(this FunctionContext context, object? value)
        {
            context.GetBindings().InvocationResult = value;
        }

        /// <summary>
        /// Gets the output binding entries for the current function invocation.
        /// </summary>
        /// <param name="context">The function context instance.</param>
        /// <returns>Collection of <see cref="BindingData1"/></returns>
        public static IEnumerable<BindingData<object>> GetOutputBindings(this FunctionContext context)
        {
            var bindingsFeature = context.GetBindings();

            foreach (var data in bindingsFeature.OutputBindingData)
            {
                // Gets binding type (http,queue etc) from function definition.
                string? bindingType = null;
                if (context.FunctionDefinition.OutputBindings.TryGetValue(data.Key, out var bindingData))
                {
                    bindingType = bindingData.Type;
                }
                //yield return new BindingData1(data.Key, data.Value, bindingType);

                yield return new BindingData<object>(context, data.Key, data.Value, bindingType);
            }
        }

        /// <summary>
        /// Sets the value of an output binding entry for the current function invocation.
        /// </summary>
        /// <param name="context">The function context instance.</param>
        /// <param name="name">The name of the output binding entry to set the value for.</param>
        /// <param name="value">The value of the output binding entry.</param>
        /// <exception cref="InvalidOperationException">Throws if no output binding entry present for the name passed in.</exception>
        public static void SetOutputBinding(this FunctionContext context, string name, object? value)
        {
            var bindingFeature = context.GetBindings();

            if (bindingFeature.OutputBindingData.ContainsKey(name))
            {
                bindingFeature.OutputBindingData[name] = value;
            }
            else
            {
                throw new InvalidOperationException($"Output binding entry not present for {name}");
            }
        }
    }
}
