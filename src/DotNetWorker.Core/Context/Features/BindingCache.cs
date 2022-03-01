// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System;
using System.Collections.Concurrent;
using Microsoft.Azure.Functions.Worker.Converters;

namespace Microsoft.Azure.Functions.Worker.Context.Features
{
    public class BindingCache<T> : IBindingCache<T>
    {
        private string _msg;

        public BindingCache()
        {
            _msg=DateTime.Now.ToString();
        }

        // This cache should be per invocation, not static
        private ConcurrentDictionary<string, T> _cache
            = new ConcurrentDictionary<string, T>();

        public string GetMsg() => _msg;
        public bool TryGetValue(string key, out T? value)
        {
            if (_cache.TryGetValue(key, out var conversionResult))
            {
                value = conversionResult;
                return true;
            }

            value = default;
            return false;
        }

        public bool TryAdd(string key, T value)
        {
            return _cache.TryAdd(key, value);
        }
    }
}
