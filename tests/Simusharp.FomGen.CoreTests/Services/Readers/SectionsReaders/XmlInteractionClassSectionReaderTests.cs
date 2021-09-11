/*
 *   Copyright 2021 Simusharp
 *   Don't remove this header
 *   Distributed under the MIT License.
 */

using NUnit.Framework;
using Simusharp.FomGen.Core.Models;
using Simusharp.FomGen.Core.Services.Readers.SectionsReaders;
using System;
using System.Linq;
using System.Xml.Linq;

namespace Simusharp.FomGen.CoreTests.Services.Readers.SectionsReaders
{
    [TestFixture]
    public class XmlInteractionClassSectionReaderTests
    {
        private XDocument _xDoc;
        private XmlInteractionClassSectionReader _reader;

        [SetUp]
        public void Init()
        {
            _xDoc = XDocument.Load(AppDomain.CurrentDomain.BaseDirectory + "/SuppFiles/RestaurantFOMmodule.xml");
            _reader = new XmlInteractionClassSectionReader();
        }

        [Test]
        public void Read_Normal_Succeed()
        {
            // Arrange
            // Act
            var section = (InteractionClassSection)_reader.ReadFomSection(_xDoc);

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