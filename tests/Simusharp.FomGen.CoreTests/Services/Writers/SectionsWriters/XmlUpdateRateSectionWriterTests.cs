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
    public class XmlUpdateRateSectionWriterTests
    {
        private XDocument _xDoc;
        private XmlUpdateRateSectionWriter _writer;
        private UpdateRatesSection _section;

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
            _writer = new XmlUpdateRateSectionWriter();
            _section = new UpdateRatesSection
            {
                new UpdateRate {Name = "Hi", Rate = (decimal) 50.5},
                new UpdateRate {Name = "Low", Rate = (decimal) 10.32}
            };
        }

        [Test]
        public void WriteFomSection_Normal_Valid()
        {
            // Arrange
            // Act
            _xDoc = _writer.WriteFomSection(_section, _xDoc);

            // Assert
            var ns = _xDoc.Root?.Name.Namespace;
            var nodeList = _xDoc.Descendants(ns + "updateRates").ToList();
            Assert.AreEqual(1, nodeList.Count);

            nodeList = _xDoc.Descendants(ns + "updateRate").ToList();
            Assert.AreEqual(2, nodeList.Count);

            var names = _xDoc.Descendants(ns + "name").ToArray();
            Assert.AreEqual(2, names.Length);
            Assert.True(names.Select(x => x.Value).Contains("Hi"));
            Assert.True(names.Select(x => x.Value).Contains("Low"));

            var rates = _xDoc.Descendants(ns + "rate").ToArray();
            Assert.AreEqual(2, rates.Length);
            Assert.True(rates.Select(x => x.Value).Contains("50.5"));
            Assert.True(rates.Select(x => x.Value).Contains("10.32"));
        }

        [Test]
        public void Write_Null_ThrowsException()
        {
            Assert.AreEqual(_writer.WriteFomSection(null, _xDoc), _xDoc);
            Assert.Catch<ArgumentNullException>(() => _writer.WriteFomSection(new DimensionSection(), null));
        }
    }
}