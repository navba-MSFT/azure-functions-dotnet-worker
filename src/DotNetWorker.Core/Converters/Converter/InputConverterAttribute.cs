using System;
using Microsoft.Azure.Functions.Worker.Converters;

namespace Microsoft.Azure.Functions.Worker.Core.Converters.Converter
{
    [AttributeUsage(
        AttributeTargets.Parameter |
        AttributeTargets.Class |
        AttributeTargets.Enum |
        AttributeTargets.Struct,
        AllowMultiple = false,
        Inherited = true)]
    public sealed class InputConverterAttribute : Attribute
    {
        public Type ConverterType { get; }

        public InputConverterAttribute(Type converterType)
        {
            if (converterType == null)
            {
                throw new ArgumentNullException(nameof(converterType));
            }

            if (typeof(IFunctionInputConverter).IsAssignableFrom(converterType) == false)
            {
                throw new InvalidOperationException($"{converterType.Name} should implement Microsoft.Azure.Functions.Worker.Converters.IConverter to be used as a binding converter");
            }

            ConverterType = converterType;
        }
    }
}
