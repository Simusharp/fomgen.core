/*
 *   Copyright 2021 Simusharp
 *   Don't remove this header
 *   Distributed under the MIT License.
 */

using NUnit.Framework;
using Simusharp.FomGen.Core.Models;
using Simusharp.FomGen.Core.Services.Writers.SectionsWriters;
using System.Xml.Linq;
using Simusharp.FomGen.Core.Services.Readers.SectionsReaders;

namespace Simusharp.FomGen.CoreTests.Services.Writers.SectionsWriters
{
    [TestFixture]
    public class XmlDataTypeSectionWriterTests
    {
        private XDocument _xDoc;
        private XmlDataTypeSectionWriter _writer;
        private DataTypeSection _section;

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
            _writer = new XmlDataTypeSectionWriter();
            _section = new DataTypeSection();
            _section.AddBasicData(new BasicData
            {
                Name = "basic",
                Size = 16,
                Encoding = "encoding",
                Endian = "Big",
                Interpretation = "interpretation"
            });

            _section.AddSimpleData(new SimpleData
            {
                Name = "Simple",
                Accuracy = "N/A",
                Representation = "HlaFloat",
                Resolution = "0.1",
                Semantics = "semantic",
                Units = "cm"
            });

            var enumerated = new EnumeratedData
            {
                Name = "enumerated",
                Representation = "representation",
                Semantics = "semantics"
            };

            enumerated.AddEnumerator(new EnumeratedItem
            {
                Name = "e1",
                Value = "v1"
            });

            enumerated.AddEnumerator(new EnumeratedItem
            {
                Name = "e2",
                Value = "v2"
            });

            _section.AddEnumeratedData(enumerated);

            _section.AddArrayData(new ArrayData
            {
                Name = "array",
                Cardinality = "card",
                DataType = "dt",
                Encoding = "encoding",
                Semantics = "semantics"
            });

            var fixedRecord = new FixedRecordData
            {
                Name = "fixed",
                Encoding = "encoding",
                Semantics = "semantics"
            };

            fixedRecord.AddField(new FixedRecordField
            {
                Name = "f1",
                Semantics = "s1",
                DataType = "dt1"
            });

            fixedRecord.AddField(new FixedRecordField
            {
                Name = "f2",
                Semantics = "s2",
                DataType = "dt2"
            });

            _section.AddFixedRecordData(fixedRecord);

            var variantRecord = new VariantRecordData
            {
                Name = "variant",
                Encoding = "encoding",
                DataType = "dt",
                Semantics = "semantics",
                Discriminant = "disc"
            };

            variantRecord.AddAlternative(new AlternativeItem
            {
                Name = "a1",
                Enumerator = "e1",
                DataType = "dt1",
                Semantics = "semantics"
            });

            variantRecord.AddAlternative(new AlternativeItem
            {
                Name = "a2",
                Enumerator = "e2",
                DataType = "dt2",
                Semantics = "semantics"
            });

            _section.AddVariantRecordData(variantRecord);
        }

        [Test]
        public void WriteDataTypeNormal()
        {
            // Act
            var updatedDoc = _writer.WriteFomSection(_section, _xDoc);
            var reader = new XmlDataTypeSectionReader();
            var fomSection = (DataTypeSection)reader.ReadFomSection(updatedDoc);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.AreEqual(1, fomSection.SimpleData.Count);
                Assert.AreEqual(1, fomSection.BasicData.Count);
                Assert.AreEqual(1, fomSection.ArrayData.Count);
                Assert.AreEqual(1, fomSection.EnumeratedData.Count);
                Assert.AreEqual(1, fomSection.FixedRecordData.Count);
                Assert.AreEqual(1, fomSection.VariantRecordData.Count);
            });
        }
    }
}