/*
 *   Copyright 2021 Simusharp
 *   Don't remove this header
 *   Distributed under the MIT License.
 */

using FluentValidation;
using FluentValidation.Results;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Simusharp.FomGen.Core.Models
{
    public class TagSection : FomSection, IList<UserTag>
    {
        private readonly IList<UserTag> _userTags = new List<UserTag>();

        public override string SectionName { get; } = "tags";

        internal override IEnumerable<ValidationFailure> Validate(IValidator<string> validator)
        {
            return Array.Empty<ValidationFailure>();
        }

        public IEnumerator<UserTag> GetEnumerator()
        {
            return this._userTags.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        public void Add(UserTag item)
        {
            this._userTags.Add(item);
        }

        public void Clear()
        {
            this._userTags.Clear();
        }

        public bool Contains(UserTag item)
        {
            return this._userTags.Contains(item);
        }

        public void CopyTo(UserTag[] array, int arrayIndex)
        {
            this._userTags.CopyTo(array, arrayIndex);
        }

        public bool Remove(UserTag item)
        {
            return this._userTags.Remove(item);
        }

        public int Count => this._userTags.Count;

        public bool IsReadOnly => this._userTags.IsReadOnly;

        public int IndexOf(UserTag item)
        {
            return this._userTags.IndexOf(item);
        }

        public void Insert(int index, UserTag item)
        {
            this._userTags.Insert(index, item);
        }

        public void RemoveAt(int index)
        {
            this._userTags.RemoveAt(index);
        }

        public UserTag this[int index]
        {
            get => this._userTags[index];
            set => this._userTags[index] = value;
        }
    }
}