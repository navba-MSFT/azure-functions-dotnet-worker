// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System;
using Microsoft.Azure.Functions.Worker.Converters;

namespace Microsoft.Azure.Functions.Worker.Context.Features
{
    public interface IBindingCache<T>
    {
        string GetMsg();
        bool TryGetValue(string key, out T? value);

        bool TryAdd(string key, T value);
    }
}
