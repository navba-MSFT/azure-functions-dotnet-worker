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
        private static readonly Type _featureType = typeof(DefaultConversionFeature);
        private readonly IConverterProvider _converterProvider;

        public ConversionFeatureProvider(IConverterProvider converterProvider)
        {
            _converterProvider = converterProvider;
        }

        public bool TryCreate(Type type, out IConversionFeature? feature)
        {
            feature = type == _featureType 
                ? new DefaultConversionFeature(_converterProvider) 
                : null;

            return feature is not null;
        }
    }
}
