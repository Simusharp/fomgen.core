/*
 *   Copyright 2021 Simusharp
 *   Don't remove this header
 *   Distributed under the MIT License.
 */

using NUnit.Framework;
using Simusharp.FomGen.Core;
using Simusharp.FomGen.Core.Models;
using Simusharp.FomGen.Core.Services.Writers.SectionsWriters;
using System;
using System.Linq;
using System.Xml.Linq;

namespace Simusharp.FomGen.CoreTests.Services.Writers.SectionsWriters
{
    [TestFixture]
    public class XmlSwitchSectionWriterTests
    {
        private XDocument _xDoc;
        private XmlSwitchSectionWriter _writer;

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
            _writer = new XmlSwitchSectionWriter();
        }

        [Test]
        public void WriteFomSectionTestSucceed()
        {
            // Arrange
            var s = new SwitchSection
            {
                AutoProvide = true,
                ConveyRegionDesignatorSets = true,
                AutomaticResignSwitch = ResignSwitchType.DeleteObjects
            };

            // Act
            _xDoc = _writer.WriteFomSection(s, _xDoc);

            // Assert
            var ns = _xDoc.Root?.Name.Namespace;
            var nodeList = _xDoc.Descendants(ns + "switches").ToList();
            Assert.AreEqual(1, nodeList.Count);

            nodeList = _xDoc.Descendants(ns + "autoProvide").ToList();
            Assert.AreEqual(1, nodeList.Count);
            var attrib = nodeList[0].Attributes("isEnabled").First();
            Assert.AreEqual("true", attrib.Value);

            nodeList = _xDoc.Descendants(ns + "automaticResignAction").ToList();
            Assert.AreEqual(1, nodeList.Count);
            attrib = nodeList[0].Attributes("resignAction").First();
            Assert.AreEqual("DeleteObjects", attrib.Value);
        }

        [Test]
        public void Write_Null_ReturnSame()
        {
            // Arrange
            // Act
            // Assert
            Assert.AreEqual(_writer.WriteFomSection(null, _xDoc), _xDoc);
        }

        [Test]
        public void WriteFomSectionTestFailNullXDocument()
        {
            // Arrange
            var fomSec = new SwitchSection();

            // Act
            // Assert
            Assert.Throws<ArgumentNullException>(() => _writer.WriteFomSection(fomSec, null));
        }

        [Test]
        public void WriteFomSectionTestFail2ObjectModel()
        {
            // Arrange
            const string data = "<?xml version=\"1.0\" encoding=\"utf-8\"?>"
                                + "<objectModel xmlns=\"http://standards.ieee.org/IEEE1516-2010\" "
                                + "xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" "
                                + "xsi:schemaLocation=\"http://standards.ieee.org/IEEE1516-2010 "
                                + "http://standards.ieee.org/downloads/1516/1516.2-2010/IEEE1516-DIF-2010.xsd\">"
                                + "<objectModel>"
                                + "</objectModel>"
                                + "</objectModel>";
            var doc = XDocument.Parse(data);
            var fomSec = new SwitchSection();

            // Act
            // Assert
            Assert.Throws<FomReaderException>(() => _writer.WriteFomSection(fomSec, doc));
        }

        [Test]
        public void WriteFomSectionTestFailZeroObjectModel()
        {
            // Arrange
            const string data = "<?xml version=\"1.0\" encoding=\"utf-8\"?>"
                                + "<o xmlns=\"http://standards.ieee.org/IEEE1516-2010\" "
                                + "xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" "
                                + "xsi:schemaLocation=\"http://standards.ieee.org/IEEE1516-2010 "
                                + "http://standards.ieee.org/downloads/1516/1516.2-2010/IEEE1516-DIF-2010.xsd\">"
                                + "</o>";
            var doc = XDocument.Parse(data);
            var fomSec = new SwitchSection();

            // Act
            // Assert
            Assert.Throws<FomReaderException>(() => _writer.WriteFomSection(fomSec, doc));
        }
    }
}