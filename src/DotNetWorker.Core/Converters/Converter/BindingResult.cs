namespace Microsoft.Azure.Functions.Worker.Converters
{
    /// <summary>
    /// A type representing the result of binding operation.
    /// </summary>
    public readonly struct BindingResult
    {
        /// <summary>
        /// Indicates the binding operation was successful or not.
        /// </summary>
        public bool IsSuccess { get; }

        public object? Model {get;}

        /// <summary>
        /// Creates a new BindingResult
        /// </summary>
        /// <param name="isSuccess"></param>
        /// <param name="model"></param>
        public BindingResult(bool isSuccess, object? model)
        {
            this.IsSuccess = isSuccess;
            this.Model = model;
        }

        /// <summary>
        /// Creates a new BindingResult
        /// </summary>
        /// <param name="model"></param>
        public BindingResult(object model)
        {
            this.IsSuccess = true;
            this.Model = model;
        }
        
        public static BindingResult Success(object? model) => new BindingResult(isSuccess: true, model);
        
        public static BindingResult Failed() => new BindingResult(isSuccess:false, model: null);
    }
}
