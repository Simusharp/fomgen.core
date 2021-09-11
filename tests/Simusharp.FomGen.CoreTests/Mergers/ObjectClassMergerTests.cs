/*
 *   Copyright 2021 Simusharp
 *   Don't remove this header
 *   Distributed under the MIT License.
 */

using NUnit.Framework;
using Simusharp.FomGen.Core.Services.Readers.SectionsReaders;
using System;
using System.Linq;
using System.Xml.Linq;
using AutoFixture;
using Simusharp.FomGen.Core.Mergers;
using Simusharp.FomGen.Core.Models;
using Simusharp.FomGen.Core.Util;

namespace Simusharp.FomGen.CoreTests.Mergers
{
    [TestFixture]
    public class ObjectClassMergerTests
    {
        [Test]
        public void Merge_Same_Succeed()
        {
            // Arrange
            var xDoc = XDocument.Load(AppDomain.CurrentDomain.BaseDirectory + "/SuppFiles/RestaurantFOMmodule.xml");
            var reader = new XmlObjectClassSectionReader();
            var s1 = (ObjectClassSection)reader.ReadFomSection(xDoc);
            var s2 = (ObjectClassSection)reader.ReadFomSection(xDoc);
            var merger = new ObjectClassMerger();

            // Act
            var section = merger.Merge(new[] { s1, s2 });

            // Assert
            Assert.Multiple(() =>
            {
                Assert.AreEqual(39, section.Root.Count);
                var o1 = section.Root.Find(x => "Dishwasher".Equals(x.Value?.Name));
                Assert.NotNull(o1);
                Assert.AreEqual(0, o1.Value.Attributes.Count);
                var o2 = section.Root.Find(x => "Waiter".Equals(x.Value?.Name));
                Assert.NotNull(o2);
                Assert.AreEqual(3, o2.Value.Attributes.Count);
                var a1 = o2.Value.Attributes.FirstOrDefault(x => "Cheerfulness".Equals(x.Name));
                Assert.AreEqual("WaiterValue", a1?.DataType);
                Assert.AreEqual("DivestAcquire", a1?.Ownership);
                var o3 = section.Root.Find(x => "Waiter1".Equals(x.Value?.Name));
                Assert.Null(o3);
            });
        }

        [Test]
        public void Merge_Disjoint_Succeed()
        {
            // Arrange
            var fixture = new Fixture();
            var root1 = new ObjectClass {Name = "HLAobjectRoot"};
            var s1 = new ObjectClassSection {Root = new TreeNode<ObjectClass>(root1)};
            
            var root2 = new ObjectClass {Name = "HLAobjectRoot"};
            var s2 = new ObjectClassSection {Root = new TreeNode<ObjectClass>(root2)};
            var children = fixture.CreateMany<ObjectClass>(6).ToArray();
            
            s1.Root.Add(new TreeNode<ObjectClass>(children[0]));
            s1.Root.Children[0].Add(new TreeNode<ObjectClass>(children[1]));
            s1.Root.Children[0].Add(new TreeNode<ObjectClass>(children[2]));

            s2.Root.Add(new TreeNode<ObjectClass>(children[3]));
            s2.Root.Children[0].Add(new TreeNode<ObjectClass>(children[4]));
            s2.Root.Children[0].Add(new TreeNode<ObjectClass>(children[5]));

            var merger = new ObjectClassMerger();

            // Act
            var section = merger.Merge(new[] { s1, s2 });

            // Assert
            Assert.Multiple(() =>
            {
                Assert.AreEqual(7, section.Root.Count);
                foreach (var child in children)
                {
                    Assert.NotNull(section.Root.Find(x => x.Value.Name.Equals(child.Name)));
                }
            });
        }

        [Test]
        public void Merge_Scaffold_Succeed()
        {
            // Arrange
            var fixture = new Fixture();
            var root1 = new ObjectClass { Name = "HLAobjectRoot" };
            var s1 = new ObjectClassSection { Root = new TreeNode<ObjectClass>(root1) };

            var root2 = new ObjectClass { Name = "HLAobjectRoot" };
            var s2 = new ObjectClassSection { Root = new TreeNode<ObjectClass>(root2) };
            var children = fixture.CreateMany<ObjectClass>(6).ToArray();

            s1.Root.Add(new TreeNode<ObjectClass>(children[0]));
            s1.Root.Children[0].Add(new TreeNode<ObjectClass>(children[1]));
            s1.Root.Children[0].Add(new TreeNode<ObjectClass>(children[2]));

            s2.Root.Add(new TreeNode<ObjectClass>(new ObjectClass{Name = children[0].Name}));
            s2.Root.Children[0].Add(new TreeNode<ObjectClass>(children[3]));
            s2.Root.Children[0].Add(new TreeNode<ObjectClass>(children[4]));
            s2.Root.Children[0].Add(new TreeNode<ObjectClass>(children[5]));

            var merger = new ObjectClassMerger();

            // Act
            var section = merger.Merge(new[] { s1, s2 });

            // Assert
            Assert.Multiple(() =>
            {
                Assert.AreEqual(7, section.Root.Count);
                Assert.AreEqual(5, section.Root.Children[0].Children.Count);
                foreach (var child in children)
                {
                    Assert.NotNull(section.Root.Find(x => x.Value.Name.Equals(child.Name)));
                }
            });
        }

        [Test]
        public void Merge_DifferentRoots_ThrowsException()
        {
            // Arrange
            var fixture = new Fixture();
            var root1 = new ObjectClass { Name = "HLAobjectRoot" };
            var s1 = new ObjectClassSection { Root = new TreeNode<ObjectClass>(root1) };

            var root2 = new ObjectClass { Name = "HLAobjectRoot1" };
            var s2 = new ObjectClassSection { Root = new TreeNode<ObjectClass>(root2) };
            var children = fixture.CreateMany<ObjectClass>(6).ToArray();

            s1.Root.Add(new TreeNode<ObjectClass>(children[0]));
            s1.Root.Children[0].Add(new TreeNode<ObjectClass>(children[1]));
            s1.Root.Children[0].Add(new TreeNode<ObjectClass>(children[2]));

            s2.Root.Add(new TreeNode<ObjectClass>(new ObjectClass { Name = children[0].Name }));
            s2.Root.Children[0].Add(new TreeNode<ObjectClass>(children[3]));
            s2.Root.Children[0].Add(new TreeNode<ObjectClass>(children[4]));
            s2.Root.Children[0].Add(new TreeNode<ObjectClass>(children[5]));

            var merger = new ObjectClassMerger();

            // Act
            // Assert
            Assert.Catch<Exception>(() => merger.Merge(new []{s1, s2}));
        }

        [Test]
        public void Merge_DifferentProperties_ThrowsException()
        {
            // Arrange
            var fixture = new Fixture();
            var root1 = new ObjectClass { Name = "HLAobjectRoot" };
            var s1 = new ObjectClassSection { Root = new TreeNode<ObjectClass>(root1) };

            var root2 = new ObjectClass { Name = "HLAobjectRoot" };
            var s2 = new ObjectClassSection { Root = new TreeNode<ObjectClass>(root2) };
            var children = fixture.CreateMany<ObjectClass>(6).ToArray();

            s1.Root.Add(new TreeNode<ObjectClass>(children[0]));
            s1.Root.Children[0].Add(new TreeNode<ObjectClass>(children[1]));
            s1.Root.Children[0].Add(new TreeNode<ObjectClass>(children[2]));

            s2.Root.Add(new TreeNode<ObjectClass>(new ObjectClass { Name = children[0].Name, Sharing = "Sharing"}));
            s2.Root.Children[0].Add(new TreeNode<ObjectClass>(children[3]));
            s2.Root.Children[0].Add(new TreeNode<ObjectClass>(children[4]));
            s2.Root.Children[0].Add(new TreeNode<ObjectClass>(children[5]));

            var merger = new ObjectClassMerger();

            // Act
            // Assert
            Assert.Catch<Exception>(() => merger.Merge(new[] { s1, s2 }));
        }

        [Test]
        public void Merge_DifferentAttributes_ThrowsException()
        {
            // Arrange
            var fixture = new Fixture();
            var root1 = new ObjectClass { Name = "HLAobjectRoot" };
            var s1 = new ObjectClassSection { Root = new TreeNode<ObjectClass>(root1) };

            var root2 = new ObjectClass { Name = "HLAobjectRoot" };
            var s2 = new ObjectClassSection { Root = new TreeNode<ObjectClass>(root2) };
            var children = fixture.CreateMany<ObjectClass>(6).ToArray();

            s1.Root.Add(new TreeNode<ObjectClass>(children[0]));
            s1.Root.Children[0].Add(new TreeNode<ObjectClass>(children[1]));
            s1.Root.Children[0].Add(new TreeNode<ObjectClass>(children[2]));

            s2.Root.Add(new TreeNode<ObjectClass>(new ObjectClass
            {
                Name = children[0].Name,
                Sharing = children[0].Sharing,
                Semantics = children[0].Semantics
            }));
            s2.Root.Children[0].Value.AddAttribute(fixture.Create<AttributeItem>());
            s2.Root.Children[0].Add(new TreeNode<ObjectClass>(children[3]));
            s2.Root.Children[0].Add(new TreeNode<ObjectClass>(children[4]));
            s2.Root.Children[0].Add(new TreeNode<ObjectClass>(children[5]));

            var merger = new ObjectClassMerger();

            // Act
            // Assert
            Assert.Catch<Exception>(() => merger.Merge(new[] { s1, s2 }));
        }
    }
}
