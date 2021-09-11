/*
 *   Copyright 2021 Simusharp
 *   Don't remove this header
 *   Distributed under the MIT License.
 */

using NUnit.Framework;
using Simusharp.FomGen.Core;
using Simusharp.FomGen.Core.Models;
using Simusharp.FomGen.Core.Services.Readers.SectionsReaders;
using System;
using System.Linq;
using System.Xml.Linq;

namespace Simusharp.FomGen.CoreTests.Services.Readers.SectionsReaders
{
    [TestFixture]
    public class XmlDimensionSectionReaderTests
    {
        private XDocument _xDoc;
        private XmlDimensionSectionReader _sectionReader;

        [SetUp]
        public void Init()
        {
            _xDoc = XDocument.Load(AppDomain.CurrentDomain.BaseDirectory + "/SuppFiles/RestaurantFOMmodule.xml");
            _sectionReader = new XmlDimensionSectionReader();
        }

        [Test]
        public void ReadSection_Normal_Succeed()
        {
            // Arrange
            // Act
            var dimensionSection = (DimensionSection)_sectionReader.ReadFomSection(_xDoc);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.AreEqual(3, dimensionSection.Count);
                Assert.True(dimensionSection.Any(x => x.Name == "SodaFlavor"));
                Assert.True(dimensionSection.Any(x => x.Name == "BarQuantity"));
                Assert.True(dimensionSection.Any(x => x.Name == "WaiterId"));
                Assert.True(dimensionSection.Any(x => x.DataType == "FlavorType"));
                Assert.True(dimensionSection.Any(x => x.DataType == "DrinkCount"));
                Assert.True(dimensionSection.Any(x => x.DataType == "EmplId"));
                Assert.True(dimensionSection.Any(x => x.NormalizationFunction == "linearEnumerated (Flavor, [Cola, Orange, RootBeer])"));
                Assert.True(dimensionSection.Any(x => x.NormalizationFunction == "linear (NumberCups, 1, 25)"));
                Assert.True(dimensionSection.Any(x => x.NormalizationFunction == "linear (WaiterId, 1, 20)"));
                Assert.True(dimensionSection.Any(x => x.ValueWhenUnspecified == "[0..3)"));
                Assert.True(dimensionSection.Any(x => x.ValueWhenUnspecified == "[0)"));
                Assert.True(dimensionSection.Any(x => x.ValueWhenUnspecified == "Excluded"));
                Assert.True(dimensionSection.Any(x => x.UpperBound == 3));
                Assert.True(dimensionSection.Any(x => x.UpperBound == 25));
                Assert.True(dimensionSection.Any(x => x.UpperBound == 20));
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
            var element = _xDoc.Descendants(ns + "dimensions");
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
            _xDoc.Root?.Add(new XElement(ns + "dimensions"));

            // Act
            // Assert
            Assert.Throws<FomReaderException>(() => this._sectionReader.ReadFomSection(_xDoc));
        }
    }
}
