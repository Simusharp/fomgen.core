/*
 *   Copyright 2021 Simusharp
 *   Don't remove this header
 *   Distributed under the MIT License.
 */

using System;
using FluentValidation;

namespace Simusharp.FomGen.Core.Validation
{
    internal class NameValidation : AbstractValidator<string>
    {
        public NameValidation()
        {
            RuleSet("Basic", () =>
            {
                RuleFor(x => x).NotEmpty();
                RuleFor(x => x).Matches("^[a-zA-Z_]([a-zA-Z0-9_])*$")
                    .WithMessage("Name must start with letter or underscore and can only contains letters, digits, and underscore");
            });
            
            RuleSet("HLA", () =>
            {
                RuleFor(x => x).Must(x => !x.StartsWith("HLA", StringComparison.CurrentCultureIgnoreCase))
                    .WithMessage("Name can't start with 'HLA'");
            });

            RuleSet("NA", () =>
            {
                RuleFor(x => x).Must(x => !x.Equals("NA", StringComparison.CurrentCultureIgnoreCase))
                    .WithMessage("Name can't be 'NA'");
            });
        }
    }
}
