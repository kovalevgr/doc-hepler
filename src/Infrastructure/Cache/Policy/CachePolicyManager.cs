﻿#nullable enable
using System;
using DocHelper.Domain.Cache.Policy;
using DocHelper.Infrastructure.Cache.Configuration;
using DocHelper.Infrastructure.Cache.Utilities;
using Microsoft.Extensions.Options;

namespace DocHelper.Infrastructure.Cache.Policy
{
    public class CachePolicyManager
    {
        private readonly CacheOptions _options;

        public CachePolicyManager(IOptions<CacheOptions> options)
        {
            _options = options.Value;
        }

        /// <summary>
        /// Converts the `commandText` to `CachePolicy`
        /// </summary>
        public CachePolicy? GetCachePolicy(string commandText)
        {
            if (commandText is null)
            {
                throw new ArgumentNullException(nameof(commandText));
            }

            if (
                !_options.UseCache
                || FunctionsUtilities.ContainsNonDeterministicFunction(commandText)
                || !StringUtilities.IsStringExist(commandText, new[] {"insert ", "update ", "delete ", "create "})
            )
            {
                return null;
            }

            return GetGlobalPolicy();
        }

        private CachePolicy GetGlobalPolicy() => new CachePolicy()
            .ExpirationMode(CacheExpirationMode.Absolute)
            .Timeout(_options.Timeout);
    }
}