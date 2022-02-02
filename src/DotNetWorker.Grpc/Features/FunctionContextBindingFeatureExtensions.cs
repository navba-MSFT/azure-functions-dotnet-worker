// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using Microsoft.Azure.Functions.Worker.Context.Features;

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
        /// <param name="context"></param>
        /// <returns></returns>
        public static IReadOnlyDictionary<string, object?> GetInputData(this FunctionContext context)
        {
            return context.GetBindings().InputData;
        }

        /// <summary>
        /// Gets the trigger meta data for the current function invocation.
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public static IReadOnlyDictionary<string, object?> GetTriggerMetadata(this FunctionContext context)
        {
            return context.GetBindings().TriggerMetadata;
        }

        /// <summary>
        /// Gets the invocation result of the current function invocation.
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public static object? GetInvocationResult(this FunctionContext context)
        {
            return context.GetBindings().InvocationResult;
        }

        /// <summary>
        /// Sets the invocation result of the current function invocation.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="value">The invocation result value.</param>
        public static void SetInvocationResult(this FunctionContext context, object? value)
        {
            context.GetBindings().InvocationResult = value;
        }

        /// <summary>
        /// Gets the output binding entries for the current function invocation.
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public static IEnumerable<OutputBindingData> GetOutputBindings(this FunctionContext context)
        {
            IFunctionBindingsFeature? bindingsFeature = context.GetBindings();

            if (bindingsFeature.OutputBindingData != null)
            {
                foreach (var data in bindingsFeature.OutputBindingData)
                {
                    // Gets binding type (http,queue etc) from function definition.
                    string? bindingType = null;
                    if (context.FunctionDefinition.OutputBindings.TryGetValue(data.Key, out var bindingData))
                    {
                        bindingType = bindingData.Type;
                    }

                    yield return new OutputBindingData(data.Key, data.Value, bindingType);
                }
            }
        }

        /// <summary>
        /// Sets the output binding for the current function invocation.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="name">The name of the output binding entry to set the value for.</param>
        /// <param name="value">The output binding value.</param>
        /// <exception cref="InvalidOperationException"></exception>
        public static void SetOutputBinding(this FunctionContext context, string name, object? value)
        {
            var feature = context.GetBindings();

            if (feature.OutputBindingData.ContainsKey(name))
            {
                feature.OutputBindingData[name] = value;
            }
            else
            {
                throw new InvalidOperationException($"Output binding entry not present for {name}");
            }
        }
    }
}
