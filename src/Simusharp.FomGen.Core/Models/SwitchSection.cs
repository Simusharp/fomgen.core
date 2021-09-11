/*
 *   Copyright 2021 Simusharp
 *   Don't remove this header
 *   Distributed under the MIT License.
 */

using FluentValidation;
using FluentValidation.Results;
using System;
using System.Collections.Generic;

namespace Simusharp.FomGen.Core.Models
{
    /// <summary>
    /// A base class for switches section
    /// </summary>
    public class SwitchSection : FomSection
    {
        /// <inheritdoc cref="SectionName"/>
        public sealed override string SectionName => "switches";

        internal override IEnumerable<ValidationFailure> Validate(IValidator<string> validator)
        {
            return Array.Empty<ValidationFailure>();
        }

        public ResignSwitchType? AutomaticResignSwitch { get; set; }

        public bool AutoProvide { get; set; }

        public bool ConveyRegionDesignatorSets { get; set; }

        public bool ConveyProducingFederate { get; set; }

        public bool AttributeScopeAdvisory { get; set; }

        public bool AttributeRelevanceAdvisory { get; set; }

        public bool ObjectClassRelevanceAdvisory { get; set; }

        public bool InteractionRelevanceAdvisory { get; set; }

        public bool ServiceReporting { get; set; }

        public bool ExceptionReporting { get; set; }

        public bool DelaySubscriptionEvaluation { get; set; }
    }
}