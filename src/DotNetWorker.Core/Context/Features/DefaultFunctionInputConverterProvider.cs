using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using Microsoft.Azure.Functions.Worker.Converters;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Microsoft.Azure.Functions.Worker.Context.Features
{
    /// <summary>
    /// An abstraction to get IConverter instances.
    ///  - Provides IConverter instances from what is defined in WorkerOptions.BindingConverters
    ///  - Provides IConverter instances when requested for a specific type explicitly.
    ///  - Internally caches the instances created.
    /// </summary>
    internal sealed class DefaultFunctionInputConverterProvider : IFunctionInputConverterProvider
    {
        private readonly ConcurrentDictionary<Type, IFunctionInputConverter> _converterCache = new();
        private readonly IServiceProvider _serviceProvider;
        private readonly WorkerOptions _workerOptions;
        
        public DefaultFunctionInputConverterProvider(IOptions<WorkerOptions> workerOptions, IServiceProvider serviceProvider)
        {
            _workerOptions = workerOptions.Value ?? throw new ArgumentNullException(nameof(workerOptions));
            _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
            DefaultConverters = CreateDefaultConverters();
        }
                
        /// <summary>
        /// Gets the built-in default converters.
        /// </summary>
        public IEnumerable<IFunctionInputConverter> DefaultConverters { get; }
                
        /// <summary>
        /// Gets an instance of the converter for the type requested.
        /// </summary>
        /// <param name="converterType">The type of IConverter implementation to return.</param>
        /// <returns>IConverter instance of the requested type.</returns>
        public IFunctionInputConverter GetConverterInstance(Type converterType)
        {
            if (converterType == null)
            {
                throw new ArgumentNullException((nameof(converterType)));
            }

            IFunctionInputConverter converterInstance;
            
            // Get the IConverter instance for converterType from cache if present.
            if (_converterCache.TryGetValue(converterType, out var converterFromCache))
            {
                converterInstance = converterFromCache;
            }
            else
            {
                // Create and cache.
                converterInstance = (IFunctionInputConverter)ActivatorUtilities.CreateInstance(_serviceProvider, converterType);
                _converterCache[converterType] = converterInstance;
            }

            return converterInstance;
        }
        
        private IEnumerable<IFunctionInputConverter> CreateDefaultConverters()
        {
            if (_workerOptions.BindingConverters == null || _workerOptions.BindingConverters.Count == 0)
            {
                throw new InvalidOperationException("No binding converters found in worker options!");
            }

            var converterList = new List<IFunctionInputConverter>(_workerOptions.BindingConverters.Count);

            foreach (Type converterType in _workerOptions.BindingConverters)
            {
                if (typeof(IFunctionInputConverter).IsAssignableFrom(converterType) == false)
                {
                    throw new InvalidOperationException($"{converterType.Name} should implement Microsoft.Azure.Functions.Worker.Converters.IConverter to be used as a binding converter");
                }

                converterList.Add((IFunctionInputConverter)ActivatorUtilities.CreateInstance(_serviceProvider, converterType));
            }

            return converterList;
        }
    }
}
