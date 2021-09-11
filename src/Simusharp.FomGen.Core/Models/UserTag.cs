/*
 *   Copyright 2021 Simusharp
 *   Don't remove this header
 *   Distributed under the MIT License.
 */

using System;

namespace Simusharp.FomGen.Core.Models
{
    public class UserTag: IEquatable<UserTag>
    {
        public UserTagType Category { get; set; }

        public string DataType { get; set; }

        public string Semantics { get; set; }

        public bool Equals(UserTag other)
        {
            return other != null && this.Category == other.Category && this.DataType == other.DataType
                   && this.Semantics == other.Semantics;
        }
    }
}
