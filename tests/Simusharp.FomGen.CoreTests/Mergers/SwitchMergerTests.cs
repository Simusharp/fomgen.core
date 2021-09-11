/*
 *   Copyright 2021 Simusharp
 *   Don't remove this header
 *   Distributed under the MIT License.
 */

using System;
using Moq;
using NUnit.Framework;
using Simusharp.FomGen.Core.Mergers;
using Simusharp.FomGen.Core.Models;

namespace Simusharp.FomGen.CoreTests.Mergers
{
    [TestFixture]
    public class SwitchMergerTests
    {
        [Test]
        public void MergeTestFailDifferentValues()
        {
            // Arrange
            var mockSwitch1 = new Mock<ISwitch>();
            mockSwitch1.Setup(sw => sw.NodeName).Returns("Name");
            mockSwitch1.Setup(sw => sw.AttributeValue).Returns("Value1");
            mockSwitch1.Setup(sw => sw.AttributeName).Returns("isEnabled");

            //var s1 = new SwitchSection { mockSwitch1.Object };

            var mockSwitch2 = new Mock<ISwitch>();
            mockSwitch2.Setup(sw => sw.NodeName).Returns("Name");
            mockSwitch2.Setup(sw => sw.AttributeValue).Returns("Value2");
            mockSwitch2.Setup(sw => sw.AttributeName).Returns("isEnabled");

            //var s2 = new SwitchSection { mockSwitch2.Object };

            var merger = new SwitchMerger();

            // Act
            // Assert
            //Assert.Throws<InvalidOperationException>(() => { merger.Merge(new []{s1, s2}); });
        }

        [Test]
        public void MergeTestFailDifferentCount()
        {
            // Arrange
            var mockSwitch1 = new Mock<ISwitch>();
            mockSwitch1.Setup(sw => sw.NodeName).Returns("Name");
            mockSwitch1.Setup(sw => sw.AttributeValue).Returns("Value1");
            mockSwitch1.Setup(sw => sw.AttributeName).Returns("isEnabled");

            //var s1 = new SwitchSection { mockSwitch1.Object };

            var mockSwitch2 = new Mock<ISwitch>();
            mockSwitch2.Setup(sw => sw.NodeName).Returns("Name");
            mockSwitch2.Setup(sw => sw.AttributeValue).Returns("Value2");
            mockSwitch2.Setup(sw => sw.AttributeName).Returns("isEnabled");

            //var s2 = new SwitchSection { mockSwitch1.Object, mockSwitch2.Object };

            var merger = new SwitchMerger();

            // Act
            // Assert
            //Assert.Throws<InvalidOperationException>(() => { merger.Merge(new[] { s1, s2 }); });
        }

        [Test]
        public void MergeTestSucceed()
        {
            // Arrange
            var s1 = new SwitchSection();
            var s2 = new SwitchSection();

            for (var i = 0; i < 3; i++)
            {
                var mockSwitch = new Mock<ISwitch>();
                mockSwitch.Setup(sw => sw.NodeName).Returns($"Name{i}");
                mockSwitch.Setup(sw => sw.AttributeValue).Returns($"Value{i}");
                if (i < 2)
                {
                    mockSwitch.Setup(sw => sw.AttributeName).Returns("isEnabled");
                }
                else
                {
                    mockSwitch.Setup(sw => sw.AttributeName).Returns("resignAction");
                }

                //s1.Add(mockSwitch.Object);
                //s2.Add(mockSwitch.Object);
            }

            // Act
            var merger = new SwitchMerger();
            var s = merger.Merge(new[] { s1, s2 });

            // Assert
            for (var i = 0; i < 3; i++)
            {
                //Assert.AreEqual($"Name{i}", s[i].NodeName);
                //Assert.AreEqual($"Value{i}", s[i].AttributeValue);
                //Assert.AreEqual(i < 2 ? "isEnabled" : "resignAction", s[i].AttributeName);
            }
        }
    }
}