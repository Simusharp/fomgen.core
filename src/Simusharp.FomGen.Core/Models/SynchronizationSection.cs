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
    public class SynchronizationSection : FomSection, IList<Synchronization>
    {
        private readonly IList<Synchronization> _syncPoints = new List<Synchronization>();

        public override string SectionName { get; } = "synchronizations";

        internal override IEnumerable<ValidationFailure> Validate(IValidator<string> validator)
        {
            var list = new List<ValidationFailure>();
            foreach (var point in _syncPoints)
            {
                var result = validator.Validate(point.Label, opts => opts.IncludeAllRuleSets());
                foreach (var failure in result.Errors)
                {
                    list.Add(new ValidationFailure($"Synchronization Point: {point.Label}", failure.ErrorMessage));
                }
            }

            return list;
        }

        public IEnumerator<Synchronization> GetEnumerator()
        {
            return this._syncPoints.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        public void Add(Synchronization item)
        {
            this._syncPoints.Add(item);
        }

        public void Clear()
        {
            this._syncPoints.Clear();
        }

        public bool Contains(Synchronization item)
        {
            return this._syncPoints.Contains(item);
        }

        public void CopyTo(Synchronization[] array, int arrayIndex)
        {
            this._syncPoints.CopyTo(array, arrayIndex);
        }

        public bool Remove(Synchronization item)
        {
            return this._syncPoints.Remove(item);
        }

        public int Count => this._syncPoints.Count;

        public bool IsReadOnly => this._syncPoints.IsReadOnly;

        public int IndexOf(Synchronization item)
        {
            return this._syncPoints.IndexOf(item);
        }

        public void Insert(int index, Synchronization item)
        {
            this._syncPoints.Insert(index, item);
        }

        public void RemoveAt(int index)
        {
            this._syncPoints.RemoveAt(index);
        }

        public Synchronization this[int index]
        {
            get => this._syncPoints[index];
            set => this._syncPoints[index] = value;
        }
    }
}