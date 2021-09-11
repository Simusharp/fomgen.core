/*
 *   Copyright 2021 Simusharp
 *   Don't remove this header
 *   Distributed under the MIT License.
 */

using System;
using System.Linq;
using System.Xml.Linq;
using NUnit.Framework;
using Simusharp.FomGen.Core.Mergers;
using Simusharp.FomGen.Core.Models;
using Simusharp.FomGen.Core.Services.Readers.SectionsReaders;

namespace Simusharp.FomGen.CoreTests.Mergers
{
    [TestFixture]
    public class InteractionClassMergerTests
    {
        [Test]
        public void Merge_Same_Succeed()
        {
            // Arrange
            var xDoc = XDocument.Load(AppDomain.CurrentDomain.BaseDirectory + "/SuppFiles/RestaurantFOMmodule.xml");
            var reader = new XmlInteractionClassSectionReader();
            var s1 = (InteractionClassSection)reader.ReadFomSection(xDoc);
            var s2 = (InteractionClassSection)reader.ReadFomSection(xDoc);
            var merger = new InteractionClassMerger();

            // Act
            var section = merger.Merge(new[] { s1, s2 });

            // Assert
            Assert.Multiple(() =>
            {
                Assert.AreEqual(15, section.Root.Count);
                var o1 = section.Root.Find(x => "FromKidsMenu".Equals(x.Value?.Name));
                Assert.NotNull(o1);
                Assert.AreEqual(0, o1.Value.Parameters.Count);
                var o2 = section.Root.Find(x => "MainCourseServed".Equals(x.Value?.Name));
                Assert.NotNull(o2);
                Assert.AreEqual(3, o2.Value.Parameters.Count);
                var a1 = o2.Value.Parameters.FirstOrDefault(x => "TemperatureOk".Equals(x.Name));
                Assert.AreEqual("ServiceStat", a1?.DataType);
                var o3 = section.Root.Find(x => "MainCourseServed1".Equals(x.Value?.Name));
                Assert.Null(o3);
            });
        }
    }
}