/*
 *   Copyright 2021 Simusharp
 *   Don't remove this header
 *   Distributed under the MIT License.
 */

using System.Collections.Generic;
using System.Linq;

namespace Simusharp.FomGen.Core.Models
{
    public class ObjectClass : IName
    {
        private readonly List<AttributeItem> _attributes = new List<AttributeItem>();

        public string Name { get; set; }

        public string Sharing { get; set; }

        public string Semantics { get; set; }

        public IReadOnlyList<AttributeItem> Attributes => this._attributes;

        public void AddAttribute(AttributeItem attribute)
        {
            this._attributes.Add(attribute);
        }

        public void RemoveAttribute(AttributeItem attribute)
        {
            this._attributes.Remove(attribute);
        }

        public bool IsScaffold => string.IsNullOrWhiteSpace(Sharing) && string.IsNullOrWhiteSpace(Semantics) &&
                                  _attributes.Count == 0;

        public bool IsMatch(ObjectClass other)
        {
            var isMatch = string.Equals(Semantics, other.Semantics) && string.Equals(Sharing, other.Sharing) && _attributes.Count == other.Attributes.Count;
            if (isMatch)
            {
                foreach (var attribute in _attributes)
                {
                    var otherAttribute = other.Attributes.FirstOrDefault(x => x.Name.Equals(attribute.Name));
                    isMatch = attribute == otherAttribute;
                    if (!isMatch)
                    {
                        break;
                    }
                }
            }

            return isMatch;
        }
    }
}
