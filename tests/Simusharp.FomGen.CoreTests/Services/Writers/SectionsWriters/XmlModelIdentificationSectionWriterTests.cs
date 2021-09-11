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
    public class XmlModelIdentificationSectionWriterTests
    {
        private XDocument _xDoc;
        private XmlModelIdentificationSectionWriter _writer;
        private ModelIdentificationSection _model;

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
            _writer = new XmlModelIdentificationSectionWriter();

            _model = new ModelIdentificationSection();
            _model.Name = "N";
            _model.Type = "FOM";
            _model.Version = "1.0.0";
            _model.ModificationDate = new DateTime(2019, 10, 13);
            _model.SecurityClassification = "Classified";
            _model.AddReleaseRestriction("R1");
            _model.AddReleaseRestriction("R2");
            _model.Purpose = "Testing";
            _model.ApplicationDomain = "Simulation";
            _model.Description = "A detailed description";
            _model.UseLimitation = "NA";
            _model.AddUseHistory("H1");
            _model.AddUseHistory("H2");
            _model.AddKeyword(new Keyword { KeywordValue = "K1", Taxonomy = "First" });
            _model.AddKeyword(new Keyword { KeywordValue = "K2", Taxonomy = "Second" });
            _model.AddPoc(new PointOfContact
            {
                Name = "Mostafa",
                Type = "Individual",
                Organization = "Simusharp",
                Phones = { "XXX-XXXX-XXXX" },
                Emails = { "mostafa@example.com" }
            });
            _model.AddReference(new Reference { Type = "Standalone", Identification = "NA" });
            _model.Other = "Any other info comes here";
            _model.Glyph = new Glyph
            {
                Type = "GIF",
                Alt = "A simple picture",
                Height = 30,
                Width = 30,
                DecodedImage = "DecodedImage"
            };
        }

        [Test]
        public void WriteModelIdentificationTest_Normal_Succeed()
        {
            // Act
            var updatedDoc = _writer.WriteFomSection(_model, _xDoc);

            // Assert
            var ns = updatedDoc.Root?.Name.Namespace;
            var sections = updatedDoc.Descendants(ns + "objectModel").ToArray();
            Assert.AreEqual(1, sections.Length);

            var names = sections[0].Elements(ns + "modelIdentification").Elements(ns + "name").ToArray();
            Assert.AreEqual(1, names.Length);
            Assert.AreEqual(names[0].Value, _model.Name);

            var glyphs = sections[0].Elements(ns + "modelIdentification").Elements(ns + "glyph").ToArray();
            Assert.AreEqual(1, glyphs.Length);
            Assert.AreEqual(_model.Glyph.Width, double.Parse(glyphs[0].Attribute("width")?.Value));
        }

        [Test]
        public void WriteModelIdentificationTest_Null_ThrowsException()
        {
            Assert.AreEqual(_writer.WriteFomSection(null, _xDoc), _xDoc);
            Assert.Catch<ArgumentNullException>(() => _writer.WriteFomSection(new ModelIdentificationSection(), null));
        }
    }
}