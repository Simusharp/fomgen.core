/*
 *   Copyright 2021 Simusharp
 *   Don't remove this header
 *   Distributed under the MIT License.
 */

using NUnit.Framework;
using Simusharp.FomGen.Core.Models;
using Simusharp.FomGen.Core.Services.Writers.SectionsWriters;
using System;
using System.Linq;
using System.Xml.Linq;

namespace Simusharp.FomGen.CoreTests.Services.Writers.SectionsWriters
{
    [TestFixture]
    public class XmlSynchronizationSectionWriterTests
    {
        private XDocument _xDoc;
        private XmlSynchronizationSectionWriter _writer;
        private SynchronizationSection _section;

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
            _writer = new XmlSynchronizationSectionWriter();
            _section = new SynchronizationSection()
            {
                new() {Label = "T1", Capability = CapabilityType.Achieve, TagDataType = "NA", Semantics = "S1"},
                new() {Label = "T2", Capability = CapabilityType.RegisterAchieve, TagDataType = "TimeType", Semantics = "S1"}
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

            var syncSection = sections[0].Elements(ns + "synchronizations").ToArray();
            Assert.AreEqual(1, syncSection.Length);
            var syncElements = syncSection[0].Elements(ns + "synchronizationPoint").ToArray();
            Assert.AreEqual(2, syncElements.Length);
            Assert.True(syncElements.Any(x => x.Element(ns + "label")?.Value == "T1"));
            Assert.True(syncElements.Any(x => x.Element(ns + "label")?.Value == "T2"));
            Assert.True(syncElements.Any(x => x.Element(ns + "capability")?.Value == "Achieve"));
            Assert.True(syncElements.Any(x => x.Element(ns + "capability")?.Value == "RegisterAchieve"));
            Assert.True(syncElements.Any(x => x.Element(ns + "dataType")?.Value == "TimeType"));
            Assert.False(syncElements.Any(x => x.Element(ns + "dataType")?.Value == "NA"));
        }

        [Test]
        public void Write_Null_ThrowsException()
        {
            Assert.AreEqual(_writer.WriteFomSection(null, _xDoc), _xDoc);
            Assert.Catch<ArgumentNullException>(() => _writer.WriteFomSection(new SynchronizationSection(), null));
        }
    }
}
