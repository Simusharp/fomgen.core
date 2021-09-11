/*
 *   Copyright 2021 Simusharp
 *   Don't remove this header
 *   Distributed under the MIT License.
 */

namespace Simusharp.FomGen.Core.Models
{
    public class BasicData
    {
        public string Name { get; set; }

        public int? Size { get; set; }

        public string Interpretation { get; set; }

        public string Endian { get; set; }

        public string Encoding { get; set; }
    }
}