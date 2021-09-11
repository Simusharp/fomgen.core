/*
 *   Copyright 2021 Simusharp
 *   Don't remove this header
 *   Distributed under the MIT License.
 */

using System;
using System.Xml.Linq;
using NUnit.Framework;
using Simusharp.FomGen.Core.Models;
using Simusharp.FomGen.Core.Services.Readers.SectionsReaders;

namespace Simusharp.FomGen.CoreTests.Services.Readers.SectionsReaders
{
    [TestFixture]
    public class XmlTransportationSectionReaderTest
    {
        private XDocument _xDoc;
        private XmlTransportationSectionReader _transportationSectionReader;

        [SetUp]
        public void Init()
        {
            _xDoc = XDocument.Load(AppDomain.CurrentDomain.BaseDirectory + "/SuppFiles/RestaurantFOMmodule.xml");
            _transportationSectionReader = new XmlTransportationSectionReader();
        }

        [Test]
        public void ReadTransportationSection_Normal_Succeed()
        {
            // Arrange
            // Act
            var transportationSection = (TransportationSection)_transportationSectionReader.ReadFomSection(_xDoc);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.AreEqual(1, transportationSection.Count);
                Assert.AreEqual("LowLatency", transportationSection[0].Name);
                Assert.AreEqual(TransportationType.BestEffort, transportationSection[0].Reliability);
                Assert.False(string.IsNullOrWhiteSpace(transportationSection[0].Semantics));
            });
        }

        [Test]
        public void ReadTransportationSection_Null_ThrowsException()
        {
            // Arrange
            // Act
            // Assert
            Assert.Throws<ArgumentNullException>(() => this._transportationSectionReader.ReadFomSection(null));
        }

        [Test]
        public void ReadTransportationSection_MissingSection_ThrowsException()
        {
            // Arrange
            var ns = _xDoc.Root?.Name.Namespace;
            var element = _xDoc.Descendants(ns + "transportations");
            element.Remove();

            // Act
            // Assert
            Assert.Null(this._transportationSectionReader.ReadFomSection(_xDoc));
        }

        [Test]
        public void ReadTransportationSection_2Sections_ThrowsException()
        {
            // Arrange
            var ns = _xDoc.Root?.Name.Namespace;
            _xDoc.Root?.Add(new XElement(ns + "transportations"));

            // Act
            // Assert
            Assert.Throws<ArgumentException>(() => this._transportationSectionReader.ReadFomSection(_xDoc));
        }
    }
}
