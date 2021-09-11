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
    /// <summary>
    /// The base class for each section in FOM file or module
    /// </summary>
    public abstract class FomSection
    {
        /// <summary>
        /// Gets the section name.
        /// </summary>
        public abstract string SectionName { get; }

        /// <summary>
        /// Validate the section against HLA naming rules
        /// </summary>
        /// <param name="validator">Name validator rules</param>
        /// <returns>A list of validation failures</returns>
        internal abstract IEnumerable<ValidationFailure> Validate(IValidator<string> validator);
    }
}
