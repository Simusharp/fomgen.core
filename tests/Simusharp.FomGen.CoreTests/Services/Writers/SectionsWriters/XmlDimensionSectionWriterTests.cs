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
    public class XmlDimensionSectionWriterTests
    {
        private XDocument _xDoc;
        private XmlDimensionSectionWriter _writer;
        private DimensionSection _section;

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
            _writer = new XmlDimensionSectionWriter();
            _section = new DimensionSection
            {
                new Dimension {Name = "D1", DataType = "D1", UpperBound = 10, NormalizationFunction = "F1", ValueWhenUnspecified = "V1"},
                new Dimension {Name = "D2", DataType = "D2", UpperBound = 20, NormalizationFunction = "F2", ValueWhenUnspecified = "V2"}
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

            var dimSection = sections[0].Elements(ns + "dimensions").ToArray();
            Assert.AreEqual(1, dimSection.Length);
            var dimElements = dimSection[0].Elements().ToArray();
            Assert.AreEqual(2, dimElements.Length);
            Assert.True(dimElements.Any(x => x.Element(ns + "name")?.Value == "D1"));
            Assert.True(dimElements.Any(x => x.Element(ns + "name")?.Value == "D1"));
            Assert.True(dimElements.Any(x => x.Element(ns + "dataType")?.Value == "D1"));
            Assert.True(dimElements.Any(x => x.Element(ns + "dataType")?.Value == "D2"));
            Assert.True(dimElements.Any(x => x.Element(ns + "upperBound")?.Value == "10"));
            Assert.True(dimElements.Any(x => x.Element(ns + "upperBound")?.Value == "20"));
            Assert.True(dimElements.Any(x => x.Element(ns + "normalization")?.Value == "F1"));
            Assert.True(dimElements.Any(x => x.Element(ns + "normalization")?.Value == "F2"));
            Assert.True(dimElements.Any(x => x.Element(ns + "value")?.Value == "V1"));
            Assert.True(dimElements.Any(x => x.Element(ns + "value")?.Value == "V2"));
        }

        [Test]
        public void Write_Null_ThrowsException()
        {
            Assert.AreEqual(_writer.WriteFomSection(null, _xDoc), _xDoc);
            Assert.Catch<ArgumentNullException>(() => _writer.WriteFomSection(new DimensionSection(), null));
        }
    }
}
