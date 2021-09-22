// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System;

namespace Microsoft.Azure.Functions.Worker.Context.Features
{
    /// <summary>
    /// Provider abstraction to get IConversionFeature instance.
    /// </summary>
    public interface IConversionFeatureProvider
    {
        bool TryCreate(Type type, out IFunctionInputConversionFeature? feature);
    }
}
