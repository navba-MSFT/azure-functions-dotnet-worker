namespace Microsoft.Azure.Functions.Worker.Converters
{
    /// <summary>
    /// A type representing the result of parameter binding operation.
    /// </summary>
    public readonly struct ParameterBindingResult
    {
        /// <summary>
        /// Indicates the binding operation was successful or not.
        /// </summary>
        public bool IsSuccess { get; }

        /// <summary>
        /// The Model (Parameter) to which we were trying to bind.
        /// </summary>
        public object? Model {get;}

        /// <summary>
        /// Creates a new BindingResult
        /// </summary>
        /// <param name="isSuccess"></param>
        /// <param name="model"></param>
        public ParameterBindingResult(bool isSuccess, object? model)
        {
            this.IsSuccess = isSuccess;
            this.Model = model;
        }

        /// <summary>
        /// Creates a new BindingResult
        /// </summary>
        /// <param name="model"></param>
        public ParameterBindingResult(object model)
        {
            this.IsSuccess = true;
            this.Model = model;
        }
        
        public static ParameterBindingResult Success(object? model) => new ParameterBindingResult(isSuccess: true, model);
        
        public static ParameterBindingResult Failed() => new ParameterBindingResult(isSuccess:false, model: null);
    }
}
