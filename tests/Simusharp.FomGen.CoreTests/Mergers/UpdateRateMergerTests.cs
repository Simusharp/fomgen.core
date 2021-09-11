/*
 *   Copyright 2021 Simusharp
 *   Don't remove this header
 *   Distributed under the MIT License.
 */

using System;
using System.Linq;
using NUnit.Framework;
using Simusharp.FomGen.Core;
using Simusharp.FomGen.Core.Mergers;
using Simusharp.FomGen.Core.Models;

namespace Simusharp.FomGen.CoreTests.Mergers
{
    [TestFixture()]
    public class UpdateRateMergerTests
    {
        [Test()]
        public void Merge_Normal_Valid()
        {
            // Arrange
            var updateRateSec1 = new UpdateRatesSection
            {
                new UpdateRate{ Name = "Hi", Rate = (decimal) 50.45},
                new UpdateRate{ Name = "Low", Rate = (decimal) 10.42}
            };

            var updateRateSec2 = new UpdateRatesSection
            {
                new UpdateRate{ Name = "Medium", Rate = (decimal) 33.85},
                new UpdateRate{ Name = "Low", Rate = (decimal) 10.42}
            };

            var merger = new UpdateRateMerger();

            // Act
            var result = merger.Merge(new []{updateRateSec1, updateRateSec2});

            // Assert
            Assert.Multiple(() =>
            {
                Assert.AreEqual(3, result.Count);
                Assert.True(result.Any(u => u.Name == "Hi"));
                Assert.True(result.Any(u => u.Name == "Medium"));
                Assert.True(result.Any(u => u.Name == "Low"));
                Assert.True(result.Any(u => u.Rate == (decimal)50.45));
                Assert.True(result.Any(u => u.Rate == (decimal)33.85));
                Assert.True(result.Any(u => u.Rate == (decimal)10.42));
            });
        }

        [Test()]
        public void Merge_Contradiction_ThrowsException()
        {
            // Arrange
            var updateRateSec1 = new UpdateRatesSection
            {
                new UpdateRate{ Name = "Hi", Rate = (decimal) 50.45},
                new UpdateRate{ Name = "Low", Rate = (decimal) 10.42}
            };

            var updateRateSec2 = new UpdateRatesSection
            {
                new UpdateRate{ Name = "Medium", Rate = (decimal) 33.85},
                new UpdateRate{ Name = "Low", Rate = (decimal) 8.16}
            };

            var merger = new UpdateRateMerger();

            // Act
            // Assert
            Assert.Throws<FomMergerException>(() => merger.Merge(new[] { updateRateSec1, updateRateSec2 }));
        }
    }
}