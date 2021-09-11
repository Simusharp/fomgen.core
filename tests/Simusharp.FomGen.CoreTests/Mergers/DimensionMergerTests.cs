/*
 *   Copyright 2021 Simusharp
 *   Don't remove this header
 *   Distributed under the MIT License.
 */

using NUnit.Framework;
using Simusharp.FomGen.Core.Mergers;
using Simusharp.FomGen.Core.Models;
using System;
using System.Linq;

namespace Simusharp.FomGen.CoreTests.Mergers
{
    [TestFixture]
    public class DimensionMergerTests
    {
        [Test]
        public void Merge_Normal_Valid()
        {
            // Arrange
            var s1 = new DimensionSection
            {
                new Dimension{ Name = "T1", DataType = "C", NormalizationFunction = "NA", ValueWhenUnspecified = "D1", UpperBound = 10},
                new Dimension{ Name = "T2", DataType = "C", NormalizationFunction = "NA", ValueWhenUnspecified = "D1", UpperBound = 10},
                new Dimension{ Name = "T3", DataType = "C", NormalizationFunction = "NA", ValueWhenUnspecified = "D1", UpperBound = 10},
                new Dimension{ Name = "T4", DataType = "C", NormalizationFunction = "NA", ValueWhenUnspecified = "D1", UpperBound = 10}
            };

            var s2 = new DimensionSection
            {
                new Dimension{ Name = "T2", DataType = "C", NormalizationFunction = "NA", ValueWhenUnspecified = "D1", UpperBound = 10},
                new Dimension{ Name = "T3", DataType = "C", NormalizationFunction = "NA", ValueWhenUnspecified = "D1", UpperBound = 10},
                new Dimension{ Name = "T5", DataType = "C", NormalizationFunction = "NA", ValueWhenUnspecified = "D1", UpperBound = 10}
            };


            var merger = new DimensionMerger();

            // Act
            var result = merger.Merge(new[] { s1, s2 });

            // Assert
            Assert.Multiple(() =>
            {
                Assert.AreEqual(5, result.Count);
                Assert.True(result.Any(x => x.Name == "T1"));
                Assert.True(result.Any(x => x.Name == "T5"));
            });
        }

        [Test]
        public void Merge_Null_ThrowsException()
        {
            // Arrange
            var merger = new DimensionMerger();

            // Act
            // Assert
            Assert.Catch<ArgumentNullException>(() => merger.Merge(null));
        }

        [Test]
        public void Merge_Empty_ThrowsException()
        {
            // Arrange
            var merger = new DimensionMerger();

            // Act
            // Assert
            Assert.Null(merger.Merge(new DimensionSection[0]));
        }
    }
}
