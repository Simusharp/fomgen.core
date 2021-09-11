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
using Simusharp.FomGen.Core.Services.Readers.SectionsReaders;

namespace Simusharp.FomGen.CoreTests.Services.Readers.SectionsReaders
{
    [TestFixture]
    public class XmlDataTypeSectionReaderTests
    {
        private XDocument _xDoc;
        private XmlDataTypeSectionReader _dataTypeSectionReader;

        [SetUp]
        public void Init()
        {
            _xDoc = XDocument.Load(AppDomain.CurrentDomain.BaseDirectory + "/SuppFiles/RestaurantFOMmodule.xml");
            _dataTypeSectionReader = new XmlDataTypeSectionReader();
        }

        [Test]
        public void ReadBasicDataSection_Normal_Succeed()
        {
            // Arrange
            // Act
            var dataTypeSection = (DataTypeSection)_dataTypeSectionReader.ReadFomSection(_xDoc);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.AreEqual(1, dataTypeSection.BasicData.Count);
                var intData = dataTypeSection.BasicData.FirstOrDefault(x => x.Name.Equals("UnsignedShort"));
                Assert.NotNull(intData);
                Assert.AreEqual(16, intData.Size);
                Assert.AreEqual("Big", intData.Endian);
                Assert.False(string.IsNullOrWhiteSpace(intData.Encoding));
                Assert.False(string.IsNullOrWhiteSpace(intData.Interpretation));
            });
        }

        [Test]
        public void ReadSimpleDataSection_Normal_Succeed()
        {
            // Arrange
            // Act
            var dataTypeSection = (DataTypeSection)_dataTypeSectionReader.ReadFomSection(_xDoc);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.AreEqual(7, dataTypeSection.SimpleData.Count);
                var timeData = dataTypeSection.SimpleData.FirstOrDefault(x => x.Name.Equals("TimeType"));
                Assert.NotNull(timeData);
                Assert.AreEqual("HLAfloat32BE", timeData.Representation);
                Assert.AreEqual("Minutes", timeData.Units);
                Assert.AreEqual("0.01667", timeData.Resolution);
                Assert.AreEqual("NA", timeData.Accuracy);
                Assert.False(string.IsNullOrWhiteSpace(timeData.Semantics));
            });
        }

        [Test]
        public void ReadEnumerateDataSection_Normal_Succeed()
        {
            // Arrange
            // Act
            var dataTypeSection = (DataTypeSection)_dataTypeSectionReader.ReadFomSection(_xDoc);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.AreEqual(3, dataTypeSection.EnumeratedData.Count);
                var flavorData = dataTypeSection.EnumeratedData.FirstOrDefault(x => x.Name.Equals("FlavorType"));
                Assert.NotNull(flavorData);
                Assert.AreEqual("HLAinteger32BE", flavorData.Representation);
                Assert.False(string.IsNullOrWhiteSpace(flavorData.Semantics));
                Assert.AreEqual(4, flavorData.Enumerator.Count);
                var enumerator = flavorData.Enumerator.FirstOrDefault(x => x.Name.Equals("Orange"));
                Assert.NotNull(enumerator);
                Assert.AreEqual("102", enumerator.Value);
            });
        }

        [Test]
        public void ReadArrayDataSection_Normal_Succeed()
        {
            // Arrange
            // Act
            var dataTypeSection = (DataTypeSection)_dataTypeSectionReader.ReadFomSection(_xDoc);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.AreEqual(2, dataTypeSection.ArrayData.Count);
                var employeeData = dataTypeSection.ArrayData.FirstOrDefault(x => x.Name.Equals("Employees"));
                Assert.NotNull(employeeData);
                Assert.AreEqual("EmplId", employeeData.DataType);
                Assert.AreEqual("10", employeeData.Cardinality);
                Assert.AreEqual("HLAfixedArray", employeeData.Encoding);
                Assert.False(string.IsNullOrWhiteSpace(employeeData.Semantics));
            });
        }

        [Test]
        public void ReadFixedRecordDataSection_Normal_Succeed()
        {
            // Arrange
            // Act
            var dataTypeSection = (DataTypeSection)_dataTypeSectionReader.ReadFomSection(_xDoc);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.AreEqual(2, dataTypeSection.FixedRecordData.Count);
                var serviceData = dataTypeSection.FixedRecordData.FirstOrDefault(x => x.Name.Equals("ServiceStat"));
                Assert.NotNull(serviceData);
                Assert.AreEqual("HLAfixedRecord", serviceData.Encoding);
                Assert.False(string.IsNullOrWhiteSpace(serviceData.Semantics));
                Assert.AreEqual(3, serviceData.Fields.Count);
                var field = serviceData.Fields.FirstOrDefault(x => x.Name.Equals("Veggy1Ok"));
                Assert.NotNull(field);
                Assert.AreEqual("HLAboolean", field.DataType);
                Assert.False(string.IsNullOrWhiteSpace(field.Semantics));
            });
        }

        [Test]
        public void ReadVariantRecordDataSection_Normal_Succeed()
        {
            // Arrange
            // Act
            var dataTypeSection = (DataTypeSection)_dataTypeSectionReader.ReadFomSection(_xDoc);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.AreEqual(1, dataTypeSection.VariantRecordData.Count);
                var waiterData = dataTypeSection.VariantRecordData.FirstOrDefault(x => x.Name.Equals("WaiterValue"));
                Assert.NotNull(waiterData);
                Assert.AreEqual("ValIndex", waiterData.Discriminant);
                Assert.AreEqual("ExperienceLevel", waiterData.DataType);
                Assert.AreEqual("HLAvariantRecord", waiterData.Encoding);
                Assert.False(string.IsNullOrWhiteSpace(waiterData.Semantics));
                Assert.AreEqual(3, waiterData.Alternatives.Count);
                var alternative = waiterData.Alternatives.FirstOrDefault(x => x.Name.Equals("CoursePassed"));
                Assert.NotNull(alternative);
                Assert.AreEqual("Trainee", alternative.Enumerator);
                Assert.AreEqual("HLAboolean", alternative.DataType);
                Assert.False(string.IsNullOrWhiteSpace(alternative.Semantics));
            });
        }
    }
}