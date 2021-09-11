/*
 *   Copyright 2021 Simusharp
 *   Don't remove this header
 *   Distributed under the MIT License.
 */

using System.Collections.Generic;

namespace Simusharp.FomGen.Core.Models
{
    public class EnumeratedData
    {
        private readonly List<EnumeratedItem> _enumerator = new List<EnumeratedItem>();

        public string Name { get; set; }

        public string Representation { get; set; }

        public string Semantics { get; set; }

        public IReadOnlyList<EnumeratedItem> Enumerator => this._enumerator;

        public void AddEnumerator(EnumeratedItem enumerator)
        {
            this._enumerator.Add(enumerator);
        }

        public void RemoveEnumerator(EnumeratedItem enumerator)
        {
            this._enumerator.Remove(enumerator);
        }
    }
}
