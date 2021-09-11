/*
 *   Copyright 2021 Simusharp
 *   Don't remove this header
 *   Distributed under the MIT License.
 */

using NUnit.Framework;
using Simusharp.FomGen.Core.Models;
using Simusharp.FomGen.Core.Services.Readers.SectionsReaders;
using Simusharp.FomGen.Core.Services.Writers.SectionsWriters;
using System;
using System.Linq;
using System.Xml.Linq;

namespace Simusharp.FomGen.CoreTests.Services.Writers.SectionsWriters
{
    [TestFixture]
    public class XmlInteractionClassSectionWriterTests
    {
        private XDocument _xDoc;
        private XmlInteractionClassSectionWriter _writer;
        private XmlInteractionClassSectionReader _reader;
        private XDocument _xDocSource;

        [SetUp]
        public void Init()
        {
            // Arrange
            const string data = "<?xml version=\"1.0\" encoding=\"utf-8\"?>"
                                + "<objectModel xmlns=\"http://standards.ieee.org/IEEE1516-2010\" "
                                + "xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" "
                                + "xsi:schemaLocation=\"http://standards.ieee.org/IEEE1516-2010 "
                                + "http://standards.ieee.org/downloads/1516/1516.2-2010/IEEE1516-DIF-2010.xsd\">"
                                + "</objectModel>";
            _xDoc = XDocument.Parse(data);
            _xDocSource = XDocument.Load(AppDomain.CurrentDomain.BaseDirectory + "/SuppFiles/RestaurantFOMmodule.xml");
            _writer = new XmlInteractionClassSectionWriter();
            _reader = new XmlInteractionClassSectionReader();
        }

        [Test]
        public void WriteInteractionClassNormal()
        {
            // Arrange
            var section = (InteractionClassSection)_reader.ReadFomSection(_xDocSource);

            // Act
            var updatedDoc = _writer.WriteFomSection(section, _xDoc);
            var updatedSection = (InteractionClassSection)_reader.ReadFomSection(updatedDoc);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.AreEqual(15, updatedSection.Root.Count);
                var o1 = updatedSection.Root.Find(x => "FromKidsMenu".Equals(x.Value?.Name));
                Assert.NotNull(o1);
                Assert.AreEqual(0, o1.Value.Parameters.Count);
                var o2 = updatedSection.Root.Find(x => "MainCourseServed".Equals(x.Value?.Name));
                Assert.NotNull(o2);
                Assert.AreEqual(3, o2.Value.Parameters.Count);
                var a1 = o2.Value.Parameters.FirstOrDefault(x => "TemperatureOk".Equals(x.Name));
                Assert.AreEqual("ServiceStat", a1?.DataType);
                var o3 = updatedSection.Root.Find(x => "MainCourseServed1".Equals(x.Value?.Name));
                Assert.Null(o3);
            });
        }
    }
}