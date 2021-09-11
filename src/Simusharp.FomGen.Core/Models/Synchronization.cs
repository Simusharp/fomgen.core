/*
 *   Copyright 2021 Simusharp
 *   Don't remove this header
 *   Distributed under the MIT License.
 */

namespace Simusharp.FomGen.Core.Models
{
    public class Synchronization
    {
        public string Label { get; set; }

        public string TagDataType { get; set; }

        public CapabilityType? Capability { get; set; }

        public string Semantics { get; set; }
    }
}
