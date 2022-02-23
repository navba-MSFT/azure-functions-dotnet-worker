// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

namespace Microsoft.Azure.Functions.Worker
{
    public class InvocationResult : InvocationResult<object>
    {
        internal InvocationResult(FunctionContext functionContext, object? value) : base(functionContext, value)
        {
        }
    }
    public class InvocationResult<T> 
    {
        internal InvocationResult(FunctionContext functionContext, T? value)
        {
            _functionContext = functionContext;
            _value = value;
        }

        private T? _value;
        private readonly FunctionContext _functionContext;
        
        /// <summary>
        /// Gets or sets the invocation result value.
        /// </summary>
        public T? Value
        {
            get => _value;
            set
            {
                _value = value;
                _functionContext.GetBindings().InvocationResult = value;
            }
        }
    }
}
