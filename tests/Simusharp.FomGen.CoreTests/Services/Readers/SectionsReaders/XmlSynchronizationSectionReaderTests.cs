/*
 *   Copyright 2021 Simusharp
 *   Don't remove this header
 *   Distributed under the MIT License.
 */

using System;
using System.Linq;
using System.Xml.Linq;
using NUnit.Framework;
using Simusharp.FomGen.Core;
using Simusharp.FomGen.Core.Models;
using Simusharp.FomGen.Core.Services.Readers.SectionsReaders;

namespace Simusharp.FomGen.CoreTests.Services.Readers.SectionsReaders
{
    [TestFixture]
    public class XmlSynchronizationSectionReaderTests
    {
        private XDocument _xDoc;
        private XmlSynchronizationSectionReader _synchronizationSectionReader;

        [SetUp]
        public void Init()
        {
            _xDoc = XDocument.Load(AppDomain.CurrentDomain.BaseDirectory + "/SuppFiles/RestaurantFOMmodule.xml");
            _synchronizationSectionReader = new XmlSynchronizationSectionReader();
        }

        [Test]
        public void ReadTransportationSection_Normal_Succeed()
        {
            // Arrange
            // Act
            var synchronizationSection = (SynchronizationSection)_synchronizationSectionReader.ReadFomSection(_xDoc);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.AreEqual(4, synchronizationSection.Count);
                Assert.True(synchronizationSection.Any(x => x.Label == "InitialPublish"));
                Assert.True(synchronizationSection.Any(x => x.TagDataType == "TimeType"));
                Assert.AreEqual(3, synchronizationSection.Count(x => x.TagDataType == "NA"));
                Assert.AreEqual(3, synchronizationSection.Count(x => x.Capability == CapabilityType.Achieve));
                Assert.AreEqual(1, synchronizationSection.Count(x => x.Capability == CapabilityType.RegisterAchieve));
                Assert.False(string.IsNullOrWhiteSpace(synchronizationSection[0].Semantics));
            });
        }

        [Test]
        public void ReadTransportationSection_Null_ThrowsException()
        {
            // Arrange
            // Act
            // Assert
            Assert.Throws<ArgumentNullException>(() => this._synchronizationSectionReader.ReadFomSection(null));
        }

        [Test]
        public void ReadTransportationSection_MissingSection_ReturnsNull()
        {
            // Arrange
            var ns = _xDoc.Root?.Name.Namespace;
            var element = _xDoc.Descendants(ns + "synchronizations");
            element.Remove();

            // Act
            var result = this._synchronizationSectionReader.ReadFomSection(_xDoc);

            // Assert
            Assert.Null(result);
        }

        [Test]
        public void ReadTransportationSection_2Sections_ThrowsException()
        {
            // Arrange
            var ns = _xDoc.Root?.Name.Namespace;
            _xDoc.Root?.Add(new XElement(ns + "synchronizations"));

            // Act
            // Assert
            Assert.Throws<FomReaderException>(() => this._synchronizationSectionReader.ReadFomSection(_xDoc));
        }
    }
}
