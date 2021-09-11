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
    public class XmlTagSectionWriterTests
    {
        private XDocument _xDoc;
        private XmlTagSectionWriter _writer;
        private TagSection _section;

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
            _writer = new XmlTagSectionWriter();
            _section = new TagSection
            {
                new() {Category = UserTagType.AcquisitionRequestTag, DataType = "D1", Semantics = "S1"},
                new() {Category = UserTagType.DeleteRemoveTag, DataType = "D2", Semantics = "S2"}
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

            var tagSection = sections[0].Elements(ns + "tags").ToArray();
            Assert.AreEqual(1, tagSection.Length);
            var tagElements = tagSection[0].Elements().ToArray();
            Assert.AreEqual(2, tagElements.Length);
            Assert.True(tagElements.Any(x => x.Name.LocalName == "acquisitionRequestTag"));
            Assert.True(tagElements.Any(x => x.Name.LocalName == "deleteRemoveTag"));
            Assert.True(tagElements.Any(x => x.Element(ns + "dataType")?.Value == "D1"));
            Assert.True(tagElements.Any(x => x.Element(ns + "dataType")?.Value == "D2"));
        }

        [Test]
        public void Write_Null_ThrowsException()
        {
            Assert.AreEqual(_writer.WriteFomSection(null, _xDoc), _xDoc);
            Assert.Catch<ArgumentNullException>(() => _writer.WriteFomSection(new TagSection(), null));
        }
    }
}
