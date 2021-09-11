/*
 *   Copyright 2021 Simusharp
 *   Don't remove this header
 *   Distributed under the MIT License.
 */

namespace Simusharp.FomGen.Core.Models
{
    public class Dimension
    {
        public string Name { get; set; }

        public string DataType { get; set; }

        public int? UpperBound { get; set; }

        public string NormalizationFunction { get; set; }

        public string ValueWhenUnspecified { get; set; }
    }
}
