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
using System.Linq;

namespace Simusharp.FomGen.CoreTests.Mergers
{
    [TestFixture]
    public class TagMergerTests
    {
        [Test]
        public void Merge_Normal_Valid()
        {
            // Arrange
            var s1 = new TagSection
            {
                new UserTag{ Category = UserTagType.UpdateReflectTag, DataType = "D1", Semantics = "S1"},
                new UserTag{ Category = UserTagType.AcquisitionRequestTag, DataType = "D2", Semantics = "S1"},
                new UserTag{ Category = UserTagType.DeleteRemoveTag, DataType = "D1", Semantics = "S2"}
            };

            var s2 = new TagSection
            {
                new UserTag{ Category = UserTagType.UpdateReflectTag, DataType = "D1", Semantics = "S1"},
                new UserTag{ Category = UserTagType.AcquisitionRequestTag, DataType = "D2", Semantics = "S1"},
                new UserTag{ Category = UserTagType.DeleteRemoveTag, DataType = "D1", Semantics = "S2"}
            };

            var merger = new TagMerger();

            // Act
            var result = merger.Merge(new[] { s1, s2 });

            // Assert
            Assert.Multiple(() =>
            {
                Assert.AreEqual(3, result.Count);
                Assert.True(result.Any(x => x.Category == UserTagType.UpdateReflectTag));
                Assert.True(result.Any(x => x.Category == UserTagType.DeleteRemoveTag));
            });
        }

        [Test]
        public void Merge_NotIdentical_ThrowsException()
        {
            // Arrange
            var s1 = new TagSection
            {
                new UserTag{ Category = UserTagType.UpdateReflectTag, DataType = "D1", Semantics = "S1"},
                new UserTag{ Category = UserTagType.AcquisitionRequestTag, DataType = "D2", Semantics = "S1"},
                new UserTag{ Category = UserTagType.DeleteRemoveTag, DataType = "D1", Semantics = "S2"}
            };

            var s2 = new TagSection
            {
                new UserTag{ Category = UserTagType.UpdateReflectTag, DataType = "D1", Semantics = "S1"},
                new UserTag{ Category = UserTagType.AcquisitionRequestTag, DataType = "D2", Semantics = "S1"},
                new UserTag{ Category = UserTagType.SendReceiveTag, DataType = "D1", Semantics = "S2"}
            };

            var merger = new TagMerger();

            // Act
            // Assert
            Assert.Catch<FomMergerException>(() => merger.Merge(new[] { s1, s2 }));
        }

        [Test]
        public void Merge_Null_ThrowsException()
        {
            // Arrange
            var merger = new TagMerger();

            // Act
            // Assert
            Assert.Catch<ArgumentNullException>(() => merger.Merge(null));
        }

        [Test]
        public void Merge_Empty_ThrowsException()
        {
            // Arrange
            var merger = new TagMerger();

            // Act
            // Assert
            Assert.Null(merger.Merge(new TagSection[0]));
        }
    }
}
