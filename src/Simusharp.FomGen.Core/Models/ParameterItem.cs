/*
 *   Copyright 2021 Simusharp
 *   Don't remove this header
 *   Distributed under the MIT License.
 */

namespace Simusharp.FomGen.Core.Models
{
    public record ParameterItem
    {
        public string Name { get; init; }

        public string DataType { get; init; }

        public string Semantics { get; init; }
    }
}