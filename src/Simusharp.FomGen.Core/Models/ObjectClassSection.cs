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
    public class ObjectClassSection : FomSection
    {
        public override string SectionName { get; } = "objects";

        internal override IEnumerable<ValidationFailure> Validate(IValidator<string> validator)
        {
            return ValidateTree(Root, validator);
        }

        public TreeNode<ObjectClass> Root { get; set; }

        internal static IEnumerable<ValidationFailure> ValidateTree<T>(TreeNode<T> node, IValidator<string> validator) where T : IName
        {
            var result = validator.Validate(node.Value.Name, options => options.IncludeRuleSets("Basic", "NA"));

            switch (node.Value)
            {
                case ObjectClass oc:
                    {
                        if (!oc.IsScaffold)
                        {
                            foreach (var failure in validator.Validate(node.Value.Name, options => options.IncludeRuleSets("HLA")).Errors)
                            {
                                result.Errors.Add(failure);
                            }
                        }

                        foreach (var attribute in oc.Attributes)
                        {
                            foreach (var failure in validator.Validate(attribute.Name, opts => opts.IncludeAllRuleSets()).Errors)
                            {
                                result.Errors.Add(failure);
                            }
                        }

                        break;
                    }
                case InteractionClass ic:
                    {
                        if (!ic.IsScaffold)
                        {
                            foreach (var failure in validator.Validate(node.Value.Name, options => options.IncludeRuleSets("HLA")).Errors)
                            {
                                result.Errors.Add(failure);
                            }
                        }

                        foreach (var parameter in ic.Parameters)
                        {
                            foreach (var failure in validator.Validate(parameter.Name, opts => opts.IncludeAllRuleSets()).Errors)
                            {
                                result.Errors.Add(failure);
                            }
                        }

                        break;
                    }
            }

            foreach (var child in node.Children)
            {
                foreach (var failure in ValidateTree(child, validator))
                {
                    result.Errors.Add(failure);
                }
            }

            return result.Errors;
        }
    }
}
