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
using System.Xml.Linq;

namespace Simusharp.FomGen.CoreTests.Services.Readers.SectionsReaders
{
    [TestFixture]
    public class XmlTagSectionReaderTests
    {
        private XDocument _xDoc;
        private XmlTagSectionReader _tagSectionReader;

        [SetUp]
        public void Init()
        {
            _xDoc = XDocument.Load(AppDomain.CurrentDomain.BaseDirectory + "/SuppFiles/RestaurantFOMmodule.xml");
            _tagSectionReader = new XmlTagSectionReader();
        }

        [Test]
        public void ReadSection_Normal_Succeed()
        {
            // Arrange
            // Act
            var tagSection = (TagSection)_tagSectionReader.ReadFomSection(_xDoc);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.AreEqual(1, tagSection.Count);
                Assert.AreEqual(UserTagType.DeleteRemoveTag, tagSection[0].Category);
                Assert.AreEqual("HLAASCIIstring", tagSection[0].DataType);
                Assert.False(string.IsNullOrWhiteSpace(tagSection[0].Semantics));
            });
        }

        [Test]
        public void ReadSection_Null_ThrowsException()
        {
            // Arrange
            // Act
            // Assert
            Assert.Throws<ArgumentNullException>(() => this._tagSectionReader.ReadFomSection(null));
        }

        [Test]
        public void ReadSection_MissingSection_ThrowsException()
        {
            // Arrange
            var ns = _xDoc.Root?.Name.Namespace;
            var element = _xDoc.Descendants(ns + "tags");
            element.Remove();

            // Act
            // Assert
            Assert.Null(this._tagSectionReader.ReadFomSection(_xDoc));
        }

        [Test]
        public void ReadSection_2Sections_ThrowsException()
        {
            // Arrange
            var ns = _xDoc.Root?.Name.Namespace;
            _xDoc.Root?.Add(new XElement(ns + "tags"));

            // Act
            // Assert
            Assert.Throws<FomReaderException>(() => this._tagSectionReader.ReadFomSection(_xDoc));
        }
    }
}
