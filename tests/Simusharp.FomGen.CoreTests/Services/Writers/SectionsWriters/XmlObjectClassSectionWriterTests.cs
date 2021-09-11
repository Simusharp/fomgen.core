/*
 *   Copyright 2021 Simusharp
 *   Don't remove this header
 *   Distributed under the MIT License.
 */

using System;
using System.Linq;
using System.Xml.Linq;
using NUnit.Framework;
using Simusharp.FomGen.Core.Models;
using Simusharp.FomGen.Core.Services.Readers.SectionsReaders;
using Simusharp.FomGen.Core.Services.Writers.SectionsWriters;

namespace Simusharp.FomGen.CoreTests.Services.Writers.SectionsWriters
{
    [TestFixture]
    public class XmlObjectClassSectionWriterTests
    {
        private XDocument _xDoc;
        private XmlObjectClassSectionWriter _writer;
        private XmlObjectClassSectionReader _reader;
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
            _writer = new XmlObjectClassSectionWriter();
            _reader = new XmlObjectClassSectionReader();
        }

        [Test]
        public void WriteObjectClassNormal()
        {
            // Arrange
            var section = (ObjectClassSection)_reader.ReadFomSection(_xDocSource);

            // Act
            var updatedDoc = _writer.WriteFomSection(section, _xDoc);
            var updatedSection = (ObjectClassSection)_reader.ReadFomSection(updatedDoc);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.AreEqual(39, updatedSection.Root.Count);
                var o1 = updatedSection.Root.Find(x => "Dishwasher".Equals(x.Value?.Name));
                Assert.NotNull(o1);
                Assert.AreEqual(0, o1.Value.Attributes.Count);
                var o2 = updatedSection.Root.Find(x => "Waiter".Equals(x.Value?.Name));
                Assert.NotNull(o2);
                Assert.AreEqual(3, o2.Value.Attributes.Count);
                var a1 = o2.Value.Attributes.FirstOrDefault(x => "Cheerfulness".Equals(x.Name));
                Assert.AreEqual("WaiterValue", a1?.DataType);
                Assert.AreEqual("DivestAcquire", a1?.Ownership);
                var o3 = updatedSection.Root.Find(x => "Waiter1".Equals(x.Value?.Name));
                Assert.Null(o3);
            });
        }
    }
}