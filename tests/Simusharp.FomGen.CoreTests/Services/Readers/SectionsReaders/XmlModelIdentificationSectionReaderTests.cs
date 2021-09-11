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
    public class XmlModelIdentificationSectionReaderTests
    {
        private XDocument _xDoc;
        private XmlModelIdentificationSectionReader _identificationSectionReader;

        [SetUp]
        public void Init()
        {
            _xDoc = XDocument.Load(AppDomain.CurrentDomain.BaseDirectory + "/SuppFiles/RestaurantFOMmodule.xml");
            _identificationSectionReader = new XmlModelIdentificationSectionReader();
        }

        [Test]
        public void ReadModelIdentificationSection_Normal_Succeed()
        {
            // Arrange
            // Act
            var model = (ModelIdentificationSection)_identificationSectionReader.ReadFomSection(_xDoc);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.AreEqual("Restaurant FOM Module Example", model.Name);
                Assert.AreEqual("FOM", model.Type);
                Assert.AreEqual("3.0", model.Version);
                Assert.AreEqual(new DateTime(2010, 8, 16), model.ModificationDate);
                Assert.AreEqual(1, model.ReleaseRestrictions.Count);
                Assert.AreEqual(3, model.UseHistory.Count);
                Assert.AreEqual(3, model.Keywords.Count);
                Assert.True(model.Keywords.Any(x => x.Taxonomy == "Food Service Industry Taxonomy"));
                Assert.True(model.Keywords.Any(x => x.KeywordValue == "Restaurant"));
                Assert.AreEqual("R0lGODlhIAAgAKIAAAAAAP///wD//8DAwICAgP///wAAAAAAACH5BAEAAAUALAAAAAAgACAAAAOrGLLc/nCpSKu9mIXNu//eA47kJpbgMHznALxESRBq6GzEq69fPtAxzimne/E4PuBPeAsQi4COKzdzdYZQI7LouzYDWUDQqaPaGhwX9PgUs764dWAqPnrRIzURdcrv+HA9ZYB4IESHJX0eiE92dxqCbnFab4VbL4uDZ5AcRY5gmkyFapQfXI8SG6d+oS1FKKQAPFilJKSinDMoHjUmgbskisDBGcXGx2jIFwEJADs=\n", model.Glyph.DecodedImage);
                Assert.AreEqual(32, model.Glyph.Height);
                Assert.AreEqual(32, model.Glyph.Width);
                Assert.AreEqual("Restaurant", model.Glyph.Alt);
            });
        }

        [Test]
        public void ReadModelIdentificationSection_Null_ThrowsException()
        {
            // Arrange
            // Act
            // Assert
            Assert.Throws<ArgumentNullException>(() => this._identificationSectionReader.ReadFomSection(null));
        }

        [Test]
        public void ReadModelIdentificationSection_MissingSection_ThrowsException()
        {
            // Arrange
            var ns = _xDoc.Root?.Name.Namespace;
            var element = _xDoc.Descendants(ns + "modelIdentification");
            element?.Remove();

            // Act
            // Assert
            Assert.Null(this._identificationSectionReader.ReadFomSection(_xDoc));
        }

        [Test]
        public void ReadModelIdentificationSection_2Sections_ThrowsException()
        {
            // Arrange
            var ns = _xDoc.Root?.Name.Namespace;
            _xDoc.Root?.Add(new XElement(ns + "modelIdentification"));

            // Act
            // Assert
            Assert.Throws<FomReaderException>(() => this._identificationSectionReader.ReadFomSection(_xDoc));
        }
    }
}