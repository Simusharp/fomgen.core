/*
 *   Copyright 2021 Simusharp
 *   Don't remove this header
 *   Distributed under the MIT License.
 */

using FluentValidation;
using FluentValidation.Results;
using Simusharp.FomGen.Core.Util;
using System.Collections.Generic;

namespace Simusharp.FomGen.Core.Models
{
    public class InteractionClassSection : FomSection
    {
        public override string SectionName { get; } = "interactions";

        internal override IEnumerable<ValidationFailure> Validate(IValidator<string> validator)
        {
            return ObjectClassSection.ValidateTree(Root, validator);
        }

        public TreeNode<InteractionClass> Root { get; set; }
    }
}
