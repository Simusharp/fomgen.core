/*
 *   Copyright 2021 Simusharp
 *   Don't remove this header
 *   Distributed under the MIT License.
 */

using NUnit.Framework;
using Simusharp.FomGen.Core;
using Simusharp.FomGen.Core.Mergers;
using Simusharp.FomGen.Core.Models;
using System;

namespace Simusharp.FomGen.CoreTests.Mergers
{
    [TestFixture]
    public class TimeMergerTests
    {
        [Test]
        public void Merge_Normal_Valid()
        {
            // Arrange
            var s1 = new TimeSection
            {
                LookAhead = new() { DataType = "D1", Semantics = "S1"},
                TimeStamp = new() { DataType = "D2", Semantics = "S2"}
            };

            var s2 = new TimeSection
            {
                LookAhead = new() { DataType = "D1", Semantics = "S1" },
                TimeStamp = new() { DataType = "D2", Semantics = "S2" }
            };

            var merger = new TimeMerger();

            // Act
            var result = merger.Merge(new[] { s1, s2 });

            // Assert
            Assert.Multiple(() =>
            {
                Assert.NotNull(result.LookAhead);
                Assert.NotNull(result.TimeStamp);
                Assert.AreEqual("D1", result.LookAhead.DataType);
                Assert.AreEqual("S1", result.LookAhead.Semantics);
                Assert.AreEqual("D2", result.TimeStamp.DataType);
                Assert.AreEqual("S2", result.TimeStamp.Semantics);
            });
        }

        [Test]
        public void Merge_Normal_Null_Valid()
        {
            // Arrange
            var s1 = new TimeSection
            {
                LookAhead = new() { DataType = "D1", Semantics = "S1" }
            };

            var s2 = new TimeSection
            {
                LookAhead = new() { DataType = "D1", Semantics = "S1" }
            };

            var merger = new TimeMerger();

            // Act
            var result = merger.Merge(new[] { s1, s2 });

            // Assert
            Assert.Multiple(() =>
            {
                Assert.NotNull(result.LookAhead);
                Assert.Null(result.TimeStamp);
                Assert.AreEqual("D1", result.LookAhead.DataType);
                Assert.AreEqual("S1", result.LookAhead.Semantics);
            });
        }

        [Test]
        public void Merge_NullLookahead_ThrowsException()
        {
            // Arrange
            var s1 = new TimeSection
            {
                TimeStamp = new() { DataType = "D2", Semantics = "S2" }
            };

            var s2 = new TimeSection
            {
                LookAhead = new() { DataType = "D1", Semantics = "S1" },
                TimeStamp = new() { DataType = "D2", Semantics = "S2" }
            };

            var merger = new TimeMerger();

            // Act
            // Assert
            Assert.Catch<FomMergerException>(() => merger.Merge(new[] { s1, s2 }));
        }

        [Test]
        public void Merge_NotIdentical_ThrowsException()
        {
            // Arrange
            var s1 = new TimeSection
            {
                LookAhead = new() { DataType = "D1", Semantics = "S1" },
                TimeStamp = new() { DataType = "D2", Semantics = "S2" }
            };

            var s2 = new TimeSection
            {
                LookAhead = new() { DataType = "D1", Semantics = "S1" },
                TimeStamp = new() { DataType = "D2", Semantics = "S3" }
            };

            var merger = new TimeMerger();

            // Act
            // Assert
            Assert.Catch<FomMergerException>(() => merger.Merge(new[] { s1, s2 }));
        }

        [Test]
        public void Merge_Null_ThrowsException()
        {
            // Arrange
            var merger = new TimeMerger();

            // Act
            // Assert
            Assert.Catch<ArgumentNullException>(() => merger.Merge(null));
        }

        [Test]
        public void Merge_Empty_ThrowsException()
        {
            // Arrange
            var merger = new TimeMerger();

            // Act
            // Assert
            Assert.Null(merger.Merge(new TimeSection[0]));
        }
    }
}
