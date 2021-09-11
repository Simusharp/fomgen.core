/*
 *   Copyright 2021 Simusharp
 *   Don't remove this header
 *   Distributed under the MIT License.
 */

using FluentValidation;
using FluentValidation.Results;
using System.Collections;
using System.Collections.Generic;

namespace Simusharp.FomGen.Core.Models
{
    public class TransportationSection : FomSection, IList<Transportation>
    {
        private readonly IList<Transportation> _transportations = new List<Transportation>();

        public override string SectionName { get; } = "transportations";

        internal override IEnumerable<ValidationFailure> Validate(IValidator<string> validator)
        {
            var list = new List<ValidationFailure>();
            foreach (var transportation in _transportations)
            {
                var result = validator.Validate(transportation.Name, opts => opts.IncludeAllRuleSets());
                foreach (var failure in result.Errors)
                {
                    list.Add(new ValidationFailure($"Transportation: {transportation.Name}", failure.ErrorMessage));
                }
            }

            return list;
        }

        public IEnumerator<Transportation> GetEnumerator()
        {
            return this._transportations.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        public void Add(Transportation item)
        {
            this._transportations.Add(item);
        }

        public void Clear()
        {
            this._transportations.Clear();
        }

        public bool Contains(Transportation item)
        {
            return this._transportations.Contains(item);
        }

        public void CopyTo(Transportation[] array, int arrayIndex)
        {
            this._transportations.CopyTo(array, arrayIndex);
        }

        public bool Remove(Transportation item)
        {
            return this._transportations.Remove(item);
        }

        public int Count => this._transportations.Count;

        public bool IsReadOnly => this._transportations.IsReadOnly;

        public int IndexOf(Transportation item)
        {
            return this._transportations.IndexOf(item);
        }

        public void Insert(int index, Transportation item)
        {
            this._transportations.Insert(index, item);
        }

        public void RemoveAt(int index)
        {
            this._transportations.RemoveAt(index);
        }

        public Transportation this[int index]
        {
            get => this._transportations[index];
            set => this._transportations[index] = value;
        }
    }
}
