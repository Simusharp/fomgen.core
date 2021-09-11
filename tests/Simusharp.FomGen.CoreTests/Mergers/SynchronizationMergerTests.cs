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
    public class SynchronizationMergerTests
    {
        [Test]
        public void Merge_Normal_Valid()
        {
            // Arrange
            var s1 = new SynchronizationSection
            {
                new Synchronization{ Label = "T1", Capability = CapabilityType.Achieve, TagDataType = "NA", Semantics = "D1"},
                new Synchronization{ Label = "T2", Capability = CapabilityType.Achieve, TagDataType = "NA", Semantics = "D1"},
                new Synchronization{ Label = "T3", Capability = CapabilityType.Achieve, TagDataType = "NA", Semantics = "D1"},
                new Synchronization{ Label = "T4", Capability = CapabilityType.Achieve, TagDataType = "NA", Semantics = "D1"}
            };

            var s2 = new SynchronizationSection
            {
                new Synchronization{ Label = "T2", Capability = CapabilityType.Achieve, TagDataType = "NA", Semantics = "D1"},
                new Synchronization{ Label = "T3", Capability = CapabilityType.Achieve, TagDataType = "NA", Semantics = "D1"},
                new Synchronization{ Label = "T5", Capability = CapabilityType.Achieve, TagDataType = "NA", Semantics = "D1"}
            };

            var merger = new SynchronizationMerger();

            // Act
            var result = merger.Merge(new[] { s1, s2 });

            // Assert
            Assert.Multiple(() =>
            {
                Assert.AreEqual(5, result.Count);
                Assert.True(result.Any(x => x.Label == "T1"));
                Assert.True(result.Any(x => x.Label == "T5"));
            });
        }

        [Test]
        public void Merge_Null_ThrowsException()
        {
            // Arrange
            var merger = new SynchronizationMerger();

            // Act
            // Assert
            Assert.Catch<ArgumentNullException>(() => merger.Merge(null));
        }

        [Test]
        public void Merge_Empty_ThrowsException()
        {
            // Arrange
            var merger = new SynchronizationMerger();

            // Act
            // Assert
            Assert.Null(merger.Merge(new SynchronizationSection[0]));
        }
    }
}
