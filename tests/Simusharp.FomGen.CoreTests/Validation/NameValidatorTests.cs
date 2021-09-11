/*
 *   Copyright 2021 Simusharp
 *   Don't remove this header
 *   Distributed under the MIT License.
 */

using FluentValidation.TestHelper;
using NUnit.Framework;
using Simusharp.FomGen.Core.Validation;

namespace Simusharp.FomGen.CoreTests.Validation
{
    public class NameValidatorTests
    {
        private NameValidation _validator;

        [SetUp]
        public void Setup()
        {
            _validator = new NameValidation();
        }

        [Test]
        public void Name_Normal_HaveNoError()
        {
            _validator.ShouldNotHaveValidationErrorFor(x => x, "section", "Basic,HLA,NA");
            _validator.ShouldNotHaveValidationErrorFor(x => x, "Normal", "Basic,HLA,NA");
            _validator.ShouldNotHaveValidationErrorFor(x => x, "_valid34", "Basic,HLA,NA");

            _validator.ShouldHaveValidationErrorFor(x => x, "3valid", "Basic,HLA,NA");
            _validator.ShouldHaveValidationErrorFor(x => x, "name.name", "Basic,HLA,NA");
            _validator.ShouldHaveValidationErrorFor(x => x, "hlaName", "Basic,HLA,NA");
            _validator.ShouldHaveValidationErrorFor(x => x, "hla", "Basic,HLA,NA");
            _validator.ShouldHaveValidationErrorFor(x => x, "na", "Basic,HLA,NA");

            _validator.ShouldNotHaveValidationErrorFor(x => x, "hla", "Basic");
            _validator.ShouldNotHaveValidationErrorFor(x => x, "na", "Basic");
        }
    }
}
