/*
 *   Copyright 2021 Simusharp
 *   Don't remove this header
 *   Distributed under the MIT License.
 */

using NUnit.Framework;
using Simusharp.FomGen.Core.Models;
using Simusharp.FomGen.Core.Services.Readers.SectionsReaders;
using System;
using System.Xml.Linq;

namespace Simusharp.FomGen.CoreTests.Services.Readers.SectionsReaders
{
    [TestFixture]
    public class XmlTimeSectionReaderTests
    {
        private XDocument _xDoc;
        private XmlTimeSectionReader _sectionReader;

        [SetUp]
        public void Init()
        {
            _xDoc = XDocument.Load(AppDomain.CurrentDomain.BaseDirectory + "/SuppFiles/RestaurantFOMmodule.xml");
            _sectionReader = new XmlTimeSectionReader();
        }

        [Test]
        public void ReadSection_Normal_Succeed()
        {
            // Arrange
            // Act
            var timeSection = (TimeSection)_sectionReader.ReadFomSection(_xDoc);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.True(timeSection.TimeStamp.DataType == "TimeType");
                Assert.True(timeSection.LookAhead.DataType == "LAType");
                Assert.False(string.IsNullOrWhiteSpace(timeSection.TimeStamp.Semantics));
                Assert.False(string.IsNullOrWhiteSpace(timeSection.LookAhead.Semantics));
            });
        }

        [Test]
        public void ReadSection_Null_ThrowsException()
        {
            // Arrange
            // Act
            // Assert
            Assert.Throws<ArgumentNullException>(() => this._sectionReader.ReadFomSection(null));
        }

        [Test]
        public void ReadSection_MissingSection_ThrowsException()
        {
            // Arrange
            var ns = _xDoc.Root?.Name.Namespace;
            var element = _xDoc.Descendants(ns + "time");
            element.Remove();

            // Act
            // Assert
            Assert.Null(this._sectionReader.ReadFomSection(_xDoc));
        }

        [Test]
        public void ReadSection_2Sections_ThrowsException()
        {
            // Arrange
            var ns = _xDoc.Root?.Name.Namespace;
            _xDoc.Root?.Add(new XElement(ns + "time"));

            // Act
            // Assert
            Assert.Throws<ArgumentException>(() => this._sectionReader.ReadFomSection(_xDoc));
        }
    }
}
