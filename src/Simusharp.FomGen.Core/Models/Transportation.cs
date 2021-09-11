/*
 *   Copyright 2021 Simusharp
 *   Don't remove this header
 *   Distributed under the MIT License.
 */

namespace Simusharp.FomGen.Core.Models
{
    public class Transportation
    {
        public string Name { get; set; }

        public TransportationType Reliability { get; set; }

        public string Semantics { get; set; }
    }
}
