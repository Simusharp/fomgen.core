/*
 *   Copyright 2021 Simusharp
 *   Don't remove this header
 *   Distributed under the MIT License.
 */

using System.Collections.Generic;

namespace Simusharp.FomGen.Core.Models
{
    public class VariantRecordData
    {
        private readonly List<AlternativeItem> _alternatives = new List<AlternativeItem>();

        public string Name { get; set; }

        public string Discriminant { get; set; }

        public string DataType { get; set; }

        public string Encoding { get; set; }

        public string Semantics { get; set; }

        public IReadOnlyList<AlternativeItem> Alternatives => this._alternatives;

        public void AddAlternative(AlternativeItem alternative)
        {
            this._alternatives.Add(alternative);
        }

        public void RemoveAlternative(AlternativeItem alternative)
        {
            this._alternatives.Remove(alternative);
        }
    }
}
