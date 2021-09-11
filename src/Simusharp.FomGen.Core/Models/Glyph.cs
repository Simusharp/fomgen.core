/*
 *   Copyright 2021 Simusharp
 *   Don't remove this header
 *   Distributed under the MIT License.
 */

namespace Simusharp.FomGen.Core.Models
{
    public class Glyph
    {
        public string Type { get; set; }

        public string Alt { get; set; } = null;

        public double? Height { get; set; }

        public double? Width { get; set; }

        public string DecodedImage { get; set; }
    }
}
