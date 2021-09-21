using System;
using System.Collections.Generic;
using Microsoft.Azure.Functions.Worker.Converters;

namespace Microsoft.Azure.Functions.Worker.Context.Features
{
    /// <summary>
    /// An abstraction to get IConverter instances.
    /// </summary>
    public interface IConverterProvider
    {
        /// <summary>
        /// Gets the built-in default converters.
        /// </summary>
        IEnumerable<IConverter> DefaultConverters { get; }
        
        /// <summary>
        /// Gets an instance of the converter for the type requested.
        /// </summary>
        /// <param name="converterType">The type of IConverter implementation to return.</param>
        /// <returns>IConverter instance of the requested type.</returns>
        IConverter GetConverterInstance(Type converterType);
    }
}
