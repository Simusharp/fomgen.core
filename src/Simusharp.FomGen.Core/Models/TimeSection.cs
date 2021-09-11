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
    public class TimeSection : FomSection
    {
        public override string SectionName { get; } = "time";

        internal override IEnumerable<ValidationFailure> Validate(IValidator<string> validator)
        {
            return Array.Empty<ValidationFailure>();
        }

        public Time TimeStamp { get; set; }

        public Time LookAhead { get; set; }
    }
}
