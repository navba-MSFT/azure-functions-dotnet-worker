using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.Functions.Worker.Converters;

namespace Microsoft.Azure.Functions.Worker.Core.Converters.Converter
{
    [AttributeUsage(

        // Support method parameters in actions.
        AttributeTargets.Parameter |
        // Support model types.
        AttributeTargets.Class |
        AttributeTargets.Enum |
        AttributeTargets.Struct,

        AllowMultiple = false,
        Inherited = true)]
    public class BindingConverterAttribute : Attribute
    {
        public Type ConverterType { get; }

        public BindingConverterAttribute(Type converterType)
        {
            if (converterType == null)
            {
                throw new ArgumentNullException(nameof(converterType));
            }

            if (typeof(IConverter).IsAssignableFrom(converterType) == false)
            {
                throw new InvalidOperationException($"{converterType.Name} should implement Microsoft.Azure.Functions.Worker.Converters.IConverter to be used as a binding converter");
            }

            this.ConverterType = converterType;
        }
    }
}
