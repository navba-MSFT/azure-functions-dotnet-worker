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

        /// <summary>
        /// Binds the 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="bindingMetadata"></param>
        /// <returns></returns>
        public static async Task<object?> BindInput(this FunctionContext context, BindingMetadata bindingMetadata)
        {
            // find the parameter from function definition for the bindingMetadata requested.
            // Use that parameter definition(which has Type) to get converted value.

            FunctionParameter? parameter = null;
            foreach (var param in context.FunctionDefinition.Parameters)
            {
                if (param.Name == bindingMetadata.Name)
                {                    
                    parameter = param;
                    break;
                }
            }
            if (parameter != null)
            {
                var ts = await GetConvertedValueFromFeature(context, parameter);
                return ts;
            }

            return null;
        }

        /// <summary>
        /// Binds an input item for the requested type.
        /// </summary>
        /// <typeparam name="T">The type of input item to bind to.</typeparam>
        /// <param name="context">The function context.</param>
        /// <returns>An instance of <see cref="OutputBindingData{T}"/> if binding was successful, else null</returns>
        /// <exception cref="InvalidOperationException"></exception>
        public static async Task<T?> BindInputAsync<T>(this FunctionContext context)
        {
            var inputType = typeof(T);

            // find the parameter from function definition for the Type requested.
            // Use that parameter definition(which has Type) to get converted value.

            FunctionParameter? parameter = null;
            foreach (var param in context.FunctionDefinition.Parameters)
            {
                if (param.Type == inputType)
                {
                    if (parameter != null)
                    {
                        // means more than one parameter found with the type requested.
                        // customer should use the other overload of this method with an explicit BindingMetadata instance.
                        throw new InvalidOperationException("More than one binding item found for the requested Type. Use the BindInput overload which takes an instance of BindingMetadata.");
                    }
                    parameter = param;
                }
            }

            if (parameter != null)
            {
                var inputBinding = context.FunctionDefinition.InputBindings.First(a => a.Key == parameter.Name);

                var ts = await GetConvertedValueFromFeature(context, parameter);

                T tts= (T)ts;
                //var result = new BindingData<T>(context, inputBinding.Value.Name, tts, inputBinding.Value.Type);

                return tts;
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
        public static IEnumerable<OutputBindingData> GetOutputBindings(this FunctionContext context)
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

                yield return new OutputBindingData(context, data.Key, data.Value, bindingType);
            }
        }        
    }
}
