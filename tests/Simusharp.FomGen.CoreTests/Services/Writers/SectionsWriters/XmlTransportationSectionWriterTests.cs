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
using Simusharp.FomGen.Core.Services.Writers.SectionsWriters;

namespace Simusharp.FomGen.CoreTests.Services.Writers.SectionsWriters
{
    [TestFixture]
    public class XmlTransportationSectionWriterTests
    {
        private XDocument _xDoc;
        private XmlTransportationSectionWriter _writer;
        private TransportationSection _section;

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
            _writer = new XmlTransportationSectionWriter();
            _section = new TransportationSection
            {
                new Transportation {Name = "T1", Reliability = TransportationType.Reliable, Semantics = "S1"},
                new Transportation {Name = "T2", Reliability = TransportationType.BestEffort, Semantics = "S2"}
            };
        }

        [Test]
        public void Write_Normal_Succeed()
        {
            // Act
            var updatedDoc = _writer.WriteFomSection(_section, _xDoc);

            // Assert
            var ns = updatedDoc.Root?.Name.Namespace;
            var sections = updatedDoc.Descendants(ns + "objectModel").ToArray();
            Assert.AreEqual(1, sections.Length);

            var transportationSection = sections[0].Elements(ns + "transportations").ToArray();
            Assert.AreEqual(1, transportationSection.Length);
            var transportationElements = transportationSection[0].Elements(ns + "transportation").ToArray();
            Assert.AreEqual(2, transportationElements.Length);
            Assert.True(transportationElements.Any(x => x.Element(ns + "name")?.Value == "T1"));
            Assert.True(transportationElements.Any(x => x.Element(ns + "name")?.Value == "T2"));
            Assert.True(transportationElements.Any(x => x.Element(ns + "reliable")?.Value == "Yes"));
            Assert.True(transportationElements.Any(x => x.Element(ns + "reliable")?.Value == "No"));
        }

        [Test]
        public void Write_Null_ThrowsException()
        {
            Assert.AreEqual(_writer.WriteFomSection(null, _xDoc), _xDoc);
            Assert.Catch<ArgumentNullException>(() => _writer.WriteFomSection(new TransportationSection(), null));
        }
    }
}
