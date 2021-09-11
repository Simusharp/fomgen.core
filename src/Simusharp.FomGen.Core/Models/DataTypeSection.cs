/*
 *   Copyright 2021 Simusharp
 *   Don't remove this header
 *   Distributed under the MIT License.
 */

using FluentValidation;
using FluentValidation.Results;
using System.Collections.Generic;

namespace Simusharp.FomGen.Core.Models
{
    public class DataTypeSection : FomSection
    {
        private readonly List<BasicData> _basicData = new();
        private readonly List<SimpleData> _simpleData = new();
        private readonly List<EnumeratedData> _enumeratedData = new();
        private readonly List<ArrayData> _arrayData = new();
        private readonly List<FixedRecordData> _fixedRecordData = new();
        private readonly List<VariantRecordData> _variantRecordData = new();

        public override string SectionName { get; } = "dataTypes";

        internal override IEnumerable<ValidationFailure> Validate(IValidator<string> validator)
        {
            var list = new List<ValidationFailure>();
            foreach (var basicData in _basicData)
            {
                var result = validator.Validate(basicData.Name, opts => opts.IncludeAllRuleSets());
                foreach (var failure in result.Errors)
                {
                    list.Add(new ValidationFailure($"Basic Data: {basicData.Name}", failure.ErrorMessage));
                }
            }

            foreach (var simpleData in _simpleData)
            {
                var result = validator.Validate(simpleData.Name, opts => opts.IncludeAllRuleSets());
                foreach (var failure in result.Errors)
                {
                    list.Add(new ValidationFailure($"Simple Data: {simpleData.Name}", failure.ErrorMessage));
                }
            }

            foreach (var enumeratedData in _enumeratedData)
            {
                var result = validator.Validate(enumeratedData.Name, opts => opts.IncludeAllRuleSets());
                foreach (var failure in result.Errors)
                {
                    list.Add(new ValidationFailure($"Enumerated Data: {enumeratedData.Name}", failure.ErrorMessage));
                }

                foreach (var enumeratedItem in enumeratedData.Enumerator)
                {
                    result = validator.Validate(enumeratedItem.Name, opts => opts.IncludeAllRuleSets());
                    foreach (var failure in result.Errors)
                    {
                        list.Add(new ValidationFailure($"Enumerated Data Enumerator: {enumeratedItem.Name}", failure.ErrorMessage));
                    }
                }
            }

            foreach (var arrayData in _arrayData)
            {
                var result = validator.Validate(arrayData.Name, opts => opts.IncludeAllRuleSets());
                foreach (var failure in result.Errors)
                {
                    list.Add(new ValidationFailure($"Array Data: {arrayData.Name}", failure.ErrorMessage));
                }
            }

            foreach (var fixedRecord in _fixedRecordData)
            {
                var result = validator.Validate(fixedRecord.Name, opts => opts.IncludeAllRuleSets());
                foreach (var failure in result.Errors)
                {
                    list.Add(new ValidationFailure($"Fixed Record Data: {fixedRecord.Name}", failure.ErrorMessage));
                }

                foreach (var field in fixedRecord.Fields)
                {
                    result = validator.Validate(field.Name, opts => opts.IncludeAllRuleSets());
                    foreach (var failure in result.Errors)
                    {
                        list.Add(new ValidationFailure($"Fixed Record Field: {field.Name}", failure.ErrorMessage));
                    }
                }
            }

            foreach (var variantRecordData in _variantRecordData)
            {
                var result = validator.Validate(variantRecordData.Name, opts => opts.IncludeAllRuleSets());
                foreach (var failure in result.Errors)
                {
                    list.Add(new ValidationFailure($"Variant Record Data: {variantRecordData.Name}", failure.ErrorMessage));
                }

                foreach (var alternativeItem in variantRecordData.Alternatives)
                {
                    result = validator.Validate(alternativeItem.Name, options => options.IncludeRuleSets("Basic", "HLA"));
                    foreach (var failure in result.Errors)
                    {
                        list.Add(new ValidationFailure($"Variant Record Alternative: {alternativeItem.Name}", failure.ErrorMessage));
                    }
                }
            }

            return list;
        }

        public IReadOnlyList<BasicData> BasicData => this._basicData;

        public IReadOnlyList<SimpleData> SimpleData => this._simpleData;

        public IReadOnlyList<EnumeratedData> EnumeratedData => this._enumeratedData;

        public IReadOnlyList<ArrayData> ArrayData => this._arrayData;

        public IReadOnlyList<FixedRecordData> FixedRecordData => this._fixedRecordData;

        public IReadOnlyList<VariantRecordData> VariantRecordData => this._variantRecordData;

        public void AddBasicData(BasicData basicData)
        {
            this._basicData.Add(basicData);
        }

        public void RemoveBasicData(BasicData basicData)
        {
            this._basicData.Remove(basicData);
        }

        public void AddSimpleData(SimpleData simpleData)
        {
            this._simpleData.Add(simpleData);
        }

        public void RemoveSimpleData(SimpleData simpleData)
        {
            this._simpleData.Remove(simpleData);
        }

        public void AddEnumeratedData(EnumeratedData enumeratedData)
        {
            this._enumeratedData.Add(enumeratedData);
        }

        public void RemoveEnumeratedData(EnumeratedData enumeratedData)
        {
            this._enumeratedData.Remove(enumeratedData);
        }

        public void AddArrayData(ArrayData arrayData)
        {
            this._arrayData.Add(arrayData);
        }

        public void RemoveArrayData(ArrayData arrayData)
        {
            this._arrayData.Remove(arrayData);
        }

        public void AddFixedRecordData(FixedRecordData fixedRecordData)
        {
            this._fixedRecordData.Add(fixedRecordData);
        }

        public void RemoveFixedRecordData(FixedRecordData fixedRecordData)
        {
            this._fixedRecordData.Remove(fixedRecordData);
        }

        public void AddVariantRecordData(VariantRecordData variantRecordData)
        {
            this._variantRecordData.Add(variantRecordData);
        }

        public void RemoveVariantRecordData(VariantRecordData variantRecordData)
        {
            this._variantRecordData.Remove(variantRecordData);
        }
    }
}
