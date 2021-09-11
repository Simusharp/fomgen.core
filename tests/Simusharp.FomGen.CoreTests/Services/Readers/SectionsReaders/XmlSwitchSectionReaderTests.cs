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
    public class XmlSwitchSectionReaderTests
    {
        private const string Data = "<?xml version=\"1.0\" encoding=\"utf-8\"?>"
                            + "<objectModel xmlns=\"http://standards.ieee.org/IEEE1516-2010\" "
                            + "xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" "
                            + "xsi:schemaLocation=\"http://standards.ieee.org/IEEE1516-2010 "
                            + "http://standards.ieee.org/downloads/1516/1516.2-2010/IEEE1516-DIF-2010.xsd\">"
                            + "<switches>"
                            + "<autoProvide isEnabled=\"false\" />"
                            + "<conveyRegionDesignatorSets isEnabled=\"false\" />"
                            + "<conveyProducingFederate isEnabled=\"false\" />"
                            + "<attributeScopeAdvisory isEnabled=\"true\" />"
                            + "<attributeRelevanceAdvisory isEnabled=\"false\" />"
                            + "<objectClassRelevanceAdvisory isEnabled=\"false\" />"
                            + "<interactionRelevanceAdvisory isEnabled=\"false\" />"
                            + "<serviceReporting isEnabled=\"false\" />"
                            + "<exceptionReporting isEnabled=\"false\" />"
                            + "<delaySubscriptionEvaluation isEnabled=\"false\" />"
                            + "<automaticResignAction resignAction=\"NoAction\" />"
                            + "</switches>"
                            + "</objectModel>";

        private const string WrongData1 = "<?xml version=\"1.0\" encoding=\"utf-8\"?>"
                                    + "<objectModel xmlns=\"http://standards.ieee.org/IEEE1516-2010\" "
                                    + "xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" "
                                    + "xsi:schemaLocation=\"http://standards.ieee.org/IEEE1516-2010 "
                                    + "http://standards.ieee.org/downloads/1516/1516.2-2010/IEEE1516-DIF-2010.xsd\">"
                                    + "<switches>"
                                    + "<autoProvide isEnabled=\"false\" />"
                                    + "<conveyRegionDesignatorSets isEnabled=\"false\" />"
                                    + "<conveyProducingFederate isEnabled=\"false\" />"
                                    + "<attributeScopeAdvisory isEnabled=\"true\" />"
                                    + "<attributeRelevanceAdvisory isEnabled=\"false\" />"
                                    + "<objectClassRelevanceAdvisory isEnabled=\"false\" />"
                                    + "<interactionRelevanceAdvisory isEnabled=\"false\" />"
                                    + "<serviceReporting isEnabled=\"false\" />"
                                    + "<exceptionReporting isEnabled=\"false\" />"
                                    + "<delaySubscriptionEvaluation isEnabled=\"false\" />"
                                    + "<automaticResignAction resignAction=\"NoAction\" />"
                                    + "</switches>"
                                    + "<switches>"
                                    + "</switches>"
                                    + "</objectModel>";

        private const string WrongData2 = "<?xml version=\"1.0\" encoding=\"utf-8\"?>"
                                          + "<objectModel xmlns=\"http://standards.ieee.org/IEEE1516-2010\" "
                                          + "xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" "
                                          + "xsi:schemaLocation=\"http://standards.ieee.org/IEEE1516-2010 "
                                          + "http://standards.ieee.org/downloads/1516/1516.2-2010/IEEE1516-DIF-2010.xsd\">"
                                          + "</objectModel>";

        private XmlSwitchSectionReader _switchSectionReader;

        [SetUp]
        public void Init()
        {
            this._switchSectionReader = new XmlSwitchSectionReader();
        }

        [Test]
        public void ReadFomSectionTestSucceed()
        {
            // Arrange
            var doc = XDocument.Parse(Data);

            // Act
            var fomSection = (SwitchSection)this._switchSectionReader.ReadFomSection(doc);

            // Assert
            Assert.False(fomSection.AutoProvide);
            Assert.True(fomSection.AttributeScopeAdvisory);
            Assert.AreEqual(ResignSwitchType.NoAction, fomSection.AutomaticResignSwitch);
        }

        [Test]
        public void ReadFomSectionTestFail2Sections()
        {
            // Arrange
            var doc = XDocument.Parse(WrongData1);

            // Act
            // Assert
            Assert.Throws<FomReaderException>(() => this._switchSectionReader.ReadFomSection(doc));
        }

        [Test]
        public void ReadFomSectionTestFail0Section()
        {
            // Arrange
            var doc = XDocument.Parse(WrongData2);

            // Act
            // Assert
            Assert.Null(this._switchSectionReader.ReadFomSection(doc));
        }

        [Test]
        public void ReadFomSectionTestFailNullDoc()
        {
            // Arrange
            // Act
            // Assert
            Assert.Throws<ArgumentNullException>(() => this._switchSectionReader.ReadFomSection(null));
        }
    }
}