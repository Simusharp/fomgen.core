/*
 *   Copyright 2021 Simusharp
 *   Don't remove this header
 *   Distributed under the MIT License.
 */

using System.Collections.Generic;

namespace Simusharp.FomGen.Core.Models
{
    public class FixedRecordData
    {
        private readonly List<FixedRecordField> _fields = new List<FixedRecordField>();

        public string Name { get; set; }

        public string Encoding { get; set; }

        public string Semantics { get; set; }

        public IReadOnlyList<FixedRecordField> Fields => this._fields;

        public void AddField(FixedRecordField field)
        {
            this._fields.Add(field);
        }

        public void RemoveField(FixedRecordField field)
        {
            this._fields.Remove(field);
        }
    }
}
