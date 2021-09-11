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
    [TestFixture()]
    public class XmlUpdateRateSectionReaderTests
    {
        private const string Data = "<?xml version=\"1.0\" encoding=\"utf-8\"?>"
                            + "<objectModel xmlns=\"http://standards.ieee.org/IEEE1516-2010\" "
                            + "xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" "
                            + "xsi:schemaLocation=\"http://standards.ieee.org/IEEE1516-2010 "
                            + "http://standards.ieee.org/downloads/1516/1516.2-2010/IEEE1516-DIF-2010.xsd\">"
                            + "<updateRates>"
                            + "<updateRate>"
                            + "<name>Low</name>"
                            + "<rate>10.56</rate>"
                            + "</updateRate>"
                            + "<updateRate>"
                            + "<name>Hi</name>"
                            + "<rate>60.0</rate>"
                            + "</updateRate>"
                            + "</updateRates>"
                            + "</objectModel>";

        private const string WrongData1 = "<?xml version=\"1.0\" encoding=\"utf-8\"?>"
                                    + "<objectModel xmlns=\"http://standards.ieee.org/IEEE1516-2010\" "
                                    + "xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" "
                                    + "xsi:schemaLocation=\"http://standards.ieee.org/IEEE1516-2010 "
                                    + "http://standards.ieee.org/downloads/1516/1516.2-2010/IEEE1516-DIF-2010.xsd\">"
                                    + "<updateRates>"
                                    + "<updateRate>"
                                    + "<name>Low</name>"
                                    + "<rate>10.56</rate>"
                                    + "</updateRate>"
                                    + "<updateRate>"
                                    + "<name>Hi</name>"
                                    + "<rate>60.0</rate>"
                                    + "</updateRate>"
                                    + "</updateRates>"
                                    + "<updateRates>"
                                    + "<updateRate>"
                                    + "<name>Low</name>"
                                    + "<rate>10.56</rate>"
                                    + "</updateRate>"
                                    + "<updateRate>"
                                    + "<name>Hi</name>"
                                    + "<rate>60.0</rate>"
                                    + "</updateRate>"
                                    + "</updateRates>"
                                    + "</objectModel>";

        private const string EmptyData = "<?xml version=\"1.0\" encoding=\"utf-8\"?>"
                                          + "<objectModel xmlns=\"http://standards.ieee.org/IEEE1516-2010\" "
                                          + "xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" "
                                          + "xsi:schemaLocation=\"http://standards.ieee.org/IEEE1516-2010 "
                                          + "http://standards.ieee.org/downloads/1516/1516.2-2010/IEEE1516-DIF-2010.xsd\">"
                                          + "</objectModel>";

        private XmlUpdateRateSectionReader _updateRateSectionReader;

        [SetUp]
        public void Init()
        {
            this._updateRateSectionReader = new XmlUpdateRateSectionReader();
        }

        [Test()]
        public void ReadFomSection_Missing_ReturnNull()
        {
            // Arrange
            var doc = XDocument.Parse(EmptyData);

            // Act
            var fomSection = (UpdateRatesSection)this._updateRateSectionReader.ReadFomSection(doc);

            // Assert
            Assert.Null(fomSection);
        }

        [Test()]
        public void ReadFomSection_2Sec_ThrowsException()
        {
            // Arrange
            var doc = XDocument.Parse(WrongData1);

            // Act
            // Assert
            Assert.Catch<ArgumentException>(() => this._updateRateSectionReader.ReadFomSection(doc));
        }

        [Test()]
        public void ReadFomSection_Normal_Valid()
        {
            // Arrange
            var doc = XDocument.Parse(Data);

            // Act
            var fomSection = (UpdateRatesSection)this._updateRateSectionReader.ReadFomSection(doc);

            // Assert
            Assert.Multiple(()=>
            {
                Assert.AreEqual(2, fomSection.Count);
                Assert.AreEqual("Low", fomSection[0].Name);
                Assert.AreEqual(10.56, fomSection[0].Rate);
                Assert.AreEqual("Hi", fomSection[1].Name);
                Assert.AreEqual(60, fomSection[1].Rate);
            });
        }
    }
}