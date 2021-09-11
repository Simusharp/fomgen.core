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
    public class UpdateRatesSection : FomSection, IList<UpdateRate>
    {
        private readonly IList<UpdateRate> _rates = new List<UpdateRate>();

        public override string SectionName { get; } = "updateRates";

        internal override IEnumerable<ValidationFailure> Validate(IValidator<string> validator)
        {
            var list = new List<ValidationFailure>();
            foreach (var rate in _rates)
            {
                var result = validator.Validate(rate.Name, opts => opts.IncludeAllRuleSets());
                foreach (var failure in result.Errors)
                {
                    list.Add(new ValidationFailure($"Update Rate: {rate.Name}", failure.ErrorMessage));
                }
            }

            return list;
        }

        public IEnumerator<UpdateRate> GetEnumerator()
        {
            return this._rates.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        public void Add(UpdateRate item)
        {
            this._rates.Add(item);
        }

        public void Clear()
        {
            this._rates.Clear();
        }

        public bool Contains(UpdateRate item)
        {
            return this._rates.Contains(item);
        }

        public void CopyTo(UpdateRate[] array, int arrayIndex)
        {
            this._rates.CopyTo(array, arrayIndex);
        }

        public bool Remove(UpdateRate item)
        {
            return this._rates.Remove(item);
        }

        public int Count => this._rates.Count;

        public bool IsReadOnly => this._rates.IsReadOnly;

        public int IndexOf(UpdateRate item)
        {
            return this._rates.IndexOf(item);
        }

        public void Insert(int index, UpdateRate item)
        {
            this._rates.Insert(index, item);
        }

        public void RemoveAt(int index)
        {
            this._rates.RemoveAt(index);
        }

        public UpdateRate this[int index]
        {
            get => this._rates[index];
            set => this._rates[index] = value;
        }
    }
}
