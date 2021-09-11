/*
 *   Copyright 2021 Simusharp
 *   Don't remove this header
 *   Distributed under the MIT License.
 */

using System;
using System.Linq;
using NUnit.Framework;
using Simusharp.FomGen.Core.Mergers;
using Simusharp.FomGen.Core.Models;

namespace Simusharp.FomGen.CoreTests.Mergers
{
    [TestFixture]
    public class TransportationMergerTest
    {
        [Test]
        public void Merge_Normal_Valid()
        {
            // Arrange
            var s1 = new TransportationSection
            {
                new Transportation{ Name = "T1", Reliability = TransportationType.Reliable, Semantics = "D1"},
                new Transportation{ Name = "T2", Reliability = TransportationType.Reliable, Semantics = "D2"},
                new Transportation{ Name = "T3", Reliability = TransportationType.BestEffort, Semantics = "D3"},
                new Transportation{ Name = "T4", Reliability = TransportationType.BestEffort, Semantics = "D4"}
            };

            var s2 = new TransportationSection
            {
                new Transportation{ Name = "T2", Reliability = TransportationType.Reliable, Semantics = "D2"},
                new Transportation{ Name = "T3", Reliability = TransportationType.BestEffort, Semantics = "D3"},
                new Transportation{ Name = "T5", Reliability = TransportationType.BestEffort, Semantics = "D5"}
            };

            var merger = new TransportationMerger();

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
            var merger = new TransportationMerger();

            // Act
            // Assert
            Assert.Catch<ArgumentNullException>(() => merger.Merge(null));
        }

        [Test]
        public void Merge_Empty_ThrowsException()
        {
            // Arrange
            var merger = new TransportationMerger();

            // Act
            // Assert
            Assert.Null(merger.Merge(new TransportationSection[0]));
        }
    }
}
