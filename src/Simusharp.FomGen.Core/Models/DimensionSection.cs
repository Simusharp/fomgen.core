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
    public class DimensionSection : FomSection, IList<Dimension>
    {
        private readonly IList<Dimension> _dimensions = new List<Dimension>();

        public override string SectionName { get; } = "dimensions";

        internal override IEnumerable<ValidationFailure> Validate(IValidator<string> validator)
        {
            var list = new List<ValidationFailure>();
            foreach (var dimension in _dimensions)
            {
                var result = validator.Validate(dimension.Name, opts => opts.IncludeAllRuleSets());
                foreach (var failure in result.Errors)
                {
                    list.Add(new ValidationFailure($"Dimension: {dimension.Name}", failure.ErrorMessage));
                }
            }

            return list;
        }

        public IEnumerator<Dimension> GetEnumerator()
        {
            return this._dimensions.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        public void Add(Dimension item)
        {
            this._dimensions.Add(item);
        }

        public void Clear()
        {
            this._dimensions.Clear();
        }

        public bool Contains(Dimension item)
        {
            return this._dimensions.Contains(item);
        }

        public void CopyTo(Dimension[] array, int arrayIndex)
        {
            this._dimensions.CopyTo(array, arrayIndex);
        }

        public bool Remove(Dimension item)
        {
            return this._dimensions.Remove(item);
        }

        public int Count => this._dimensions.Count;

        public bool IsReadOnly => this._dimensions.IsReadOnly;

        public int IndexOf(Dimension item)
        {
            return this._dimensions.IndexOf(item);
        }

        public void Insert(int index, Dimension item)
        {
            this._dimensions.Insert(index, item);
        }

        public void RemoveAt(int index)
        {
            this._dimensions.RemoveAt(index);
        }

        public Dimension this[int index]
        {
            get => this._dimensions[index];
            set => this._dimensions[index] = value;
        }
    }
}
