// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System;

namespace Microsoft.Azure.Functions.Worker.Context.Features
{
    /// <summary>
    /// Provider to get IConversionFeature instance.
    /// </summary>
    internal sealed class ConversionFeatureProvider : IConversionFeatureProvider
    {
        private static readonly Type _featureType = typeof(DefaultInputConversionFeature);
        private readonly IFunctionInputConverterProvider _converterProvider;

        public ConversionFeatureProvider(IFunctionInputConverterProvider converterProvider)
        {
            _converterProvider = converterProvider;
        }

        public bool TryCreate(Type type, out IFunctionInputConversionFeature? feature)
        {
            feature = type == _featureType 
                ? new DefaultInputConversionFeature(_converterProvider) 
                : null;

            return feature is not null;
        }
    }
}
