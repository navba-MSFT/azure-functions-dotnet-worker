// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using Microsoft.Azure.Functions.Worker.Converters;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Microsoft.Azure.Functions.Worker.Context.Features
{
    public interface IBindingConverterFactory
    {
        IEnumerable<IConverter> CreateConverters();
    }

    internal sealed class BindingConverterFactory : IBindingConverterFactory
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly WorkerOptions _workerOptions;
        
        public BindingConverterFactory(IOptions<WorkerOptions> workerOptions, IServiceProvider serviceProvider)
        {
            _workerOptions = workerOptions.Value ?? throw new ArgumentNullException(nameof(workerOptions));
            _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
        }

        public IEnumerable<IConverter> CreateConverters()
        {
            if (_workerOptions.BindingConverters == null || _workerOptions.BindingConverters.Count == 0)
            {
                throw new InvalidOperationException("No binding converters found in worker options!");
            }

            var converterList = new List<IConverter>(_workerOptions.BindingConverters.Count);

            foreach (Type converterType in _workerOptions.BindingConverters)
            {
                if (typeof(IConverter).IsAssignableFrom(converterType) == false)
                {
                    throw new InvalidOperationException($"{converterType.Name} should implement Microsoft.Azure.Functions.Worker.Converters.IConverter to be used as a binding converter");
                }

                converterList.Add((IConverter)ActivatorUtilities.CreateInstance(_serviceProvider, converterType));
            }

            return converterList;
        }
    }
}
