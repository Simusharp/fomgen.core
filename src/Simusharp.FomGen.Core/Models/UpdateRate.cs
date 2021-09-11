/*
 *   Copyright 2021 Simusharp
 *   Don't remove this header
 *   Distributed under the MIT License.
 */

namespace Simusharp.FomGen.Core.Models
{
    public class UpdateRate
    {
        /// <summary>
        /// Update rate name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Maximum update rate in Hz
        /// </summary>
        public decimal Rate { get; set; }

        public string Semantics { get; set; }
    }
}
