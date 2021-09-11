/*
 *   Copyright 2021 Simusharp
 *   Don't remove this header
 *   Distributed under the MIT License.
 */

using System.Collections.Generic;

namespace Simusharp.FomGen.Core.Models
{
    public class AttributeItem
    {
        private readonly List<string> _dimensions = new();

        public string Name { get; set; }

        public string DataType { get; set; }

        public string UpdateType { get; set; }

        public string UpdateCondition { get; set; }

        public string Ownership { get; set; }

        public string Sharing { get; set; }

        public string Transportation { get; set; }

        public string Order { get; set; }

        public string Semantics { get; set; }

        public IReadOnlyList<string> Dimensions => this._dimensions;

        public void AddDimension(string dimension)
        {
            this._dimensions.Add(dimension);
        }

        public void RemoveDimension(string dimension)
        {
            this._dimensions.Remove(dimension);
        }

        protected bool Equals(AttributeItem other)
        {
            var set = new HashSet<string>(this._dimensions);
            var isEqual = set.SetEquals(other.Dimensions);
            return isEqual && Name == other.Name && DataType == other.DataType && UpdateType == other.UpdateType && UpdateCondition == other.UpdateCondition && Ownership == other.Ownership && Sharing == other.Sharing && Transportation == other.Transportation && Order == other.Order && Semantics == other.Semantics;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((AttributeItem)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = (_dimensions != null ? _dimensions.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (Name != null ? Name.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (DataType != null ? DataType.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (UpdateType != null ? UpdateType.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (UpdateCondition != null ? UpdateCondition.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (Ownership != null ? Ownership.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (Sharing != null ? Sharing.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (Transportation != null ? Transportation.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (Order != null ? Order.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (Semantics != null ? Semantics.GetHashCode() : 0);
                return hashCode;
            }
        }

        public static bool operator ==(AttributeItem left, AttributeItem right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(AttributeItem left, AttributeItem right)
        {
            return !Equals(left, right);
        }
    }
}