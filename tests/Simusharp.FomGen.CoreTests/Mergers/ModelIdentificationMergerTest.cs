/*
 *   Copyright 2021 Simusharp
 *   Don't remove this header
 *   Distributed under the MIT License.
 */

using System.Linq;
using NUnit.Framework;
using Simusharp.FomGen.Core.Mergers;
using Simusharp.FomGen.Core.Models;

namespace Simusharp.FomGen.CoreTests.Mergers
{
    [TestFixture]
    public class ModelIdentificationMergerTest
    {
        [Test]
        public void Merge_Normal_Valid()
        {
            // Arrange
            var s1 = new ModelIdentificationSection
            {
                Name = "S1"
            };

            var s2 = new ModelIdentificationSection
            {
                Name = "S2"
            };

            var merger = new ModelIdentificationMerger();

            // Act
            var result = merger.Merge(new[] { s1, s2 });

            // Assert
            Assert.Multiple(() =>
            {
                Assert.True(result.References.Any(x => x.Identification == "S1"));
                Assert.True(result.References.Any(x => x.Identification == "S2"));
                Assert.True(result.References.All(x => x.Type == "Composed_From"));
            });
        }
    }
}
