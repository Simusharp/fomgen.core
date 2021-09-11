/*
 *   Copyright 2021 Simusharp
 *   Don't remove this header
 *   Distributed under the MIT License.
 */

using FluentValidation.Results;
using Simusharp.FomGen.Core.Validation;
using System.Collections.Generic;

namespace Simusharp.FomGen.Core.Models
{
    public class FomModule : IFomModule
    {
        public string Name { get; set; }

        public ModelIdentificationSection ModelIdentificationSection { get; set; }

        public DimensionSection DimensionSection { get; set; }

        public SwitchSection SwitchSection { get; set; }

        public ObjectClassSection ObjectClassSection { get; set; }

        public InteractionClassSection InteractionClassSection { get; set; }

        public TransportationSection TransportationSection { get; set; }

        public DataTypeSection DataTypeSection { get; set; }

        public SynchronizationSection SynchronizationSection { get; set; }

        public TagSection TagSection { get; set; }

        public TimeSection TimeSection { get; set; }

        public UpdateRatesSection UpdateRatesSection { get; set; }

        public IList<ValidationFailure> Validate()
        {
            var errors = new List<ValidationFailure>();
            var validator = new NameValidation();
            if (ModelIdentificationSection != null)
            {
                errors.AddRange(ModelIdentificationSection.Validate(validator));
            }

            if (DimensionSection != null)
            {
                errors.AddRange(DimensionSection.Validate(validator));
            }

            if (SwitchSection != null)
            {
                errors.AddRange(SwitchSection.Validate(validator));
            }

            if (ObjectClassSection != null)
            {
                errors.AddRange(ObjectClassSection.Validate(validator));
            }

            if (InteractionClassSection != null)
            {
                errors.AddRange(InteractionClassSection.Validate(validator));
            }

            if (TransportationSection != null)
            {
                errors.AddRange(TransportationSection.Validate(validator));
            }

            if (DataTypeSection != null)
            {
                errors.AddRange(DataTypeSection.Validate(validator));
            }

            if (SynchronizationSection != null)
            {
                errors.AddRange(SynchronizationSection.Validate(validator));
            }

            if (TagSection != null)
            {
                errors.AddRange(TagSection.Validate(validator));
            }

            if (TimeSection != null)
            {
                errors.AddRange(TimeSection.Validate(validator));
            }

            if (UpdateRatesSection != null)
            {
                errors.AddRange(UpdateRatesSection.Validate(validator));
            }

            return errors;
        }
    }
}
