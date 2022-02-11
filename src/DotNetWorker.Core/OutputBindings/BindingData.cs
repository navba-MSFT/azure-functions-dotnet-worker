// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

namespace Microsoft.Azure.Functions.Worker
{
    public class BindingData<T>
    {
        internal BindingData(FunctionContext functionContext, string name, T value, string? type)
        {
            _functionContext = functionContext;
            Name = name;
            _value = value;
            Type = type;
        }
        private T _value;
        private readonly FunctionContext _functionContext;

        /// <summary>
        /// Gets the name of the binding entry.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Gets the value of the binding entry.
        /// </summary>
        public T Value
        {
            get
            {
                return _value;
            } 
            set
            {
                _value=value;
                _functionContext.GetBindings().OutputBindingData[Name] = value;                
            }
        }

        /// <summary>
        /// Gets the type of the binding entry.
        /// Ex: "http","queue" etc.
        /// </summary>
        public string? Type { get; }
    }
}
