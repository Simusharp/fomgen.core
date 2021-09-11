/*
 *   Copyright 2021 Simusharp
 *   Don't remove this header
 *   Distributed under the MIT License.
 */

using Simusharp.FomGen.Core.Models;
using Simusharp.FomGen.Core.Services.Writers.SectionsWriters;
using System.IO;
using System.Xml;
using System.Xml.Linq;

namespace Simusharp.FomGen.Core.Services.Writers
{
    internal class XmlFomModuleWriter : IFomModuleWriter
    {
        private readonly XmlDataTypeSectionWriter _dataTypeSectionWriter = new();
        private readonly XmlDimensionSectionWriter _dimensionSectionWriter = new();
        private readonly XmlInteractionClassSectionWriter _interactionClassSectionWriter = new();
        private readonly XmlModelIdentificationSectionWriter _modelIdentificationSectionWriter = new();
        private readonly XmlObjectClassSectionWriter _objectClassSectionWriter = new();
        private readonly XmlSwitchSectionWriter _switchSectionWriter = new();
        private readonly XmlSynchronizationSectionWriter _synchronizationSectionWriter = new();
        private readonly XmlTagSectionWriter _tagSectionWriter = new();
        private readonly XmlTransportationSectionWriter _transportationSectionWriter = new();
        private readonly XmlUpdateRateSectionWriter _updateRateSectionWriter = new();
        private readonly XmlTimeSectionWriter _timeSectionWriter = new();

        public void WriteModule(IFomModule fomModule, Stream stream)
        {
            using (var xmlWriter = XmlWriter.Create(stream))
            {
                var xDocument = GetXml(fomModule);
                xDocument.WriteTo(xmlWriter);
            }
        }

        public XDocument GetXml(IFomModule fomModule)
        {
            XNamespace defaultNs = "http://standards.ieee.org/IEEE1516-2010";
            XNamespace xsiNs = "http://www.w3.org/2001/XMLSchema-instance";
            XNamespace schemaNs = "http://standards.ieee.org/IEEE1516-2010 "
                                  + "http://standards.ieee.org/downloads/1516/1516.2-2010/IEEE1516-DIF-2010.xsd";
            var root = new XElement(
                defaultNs + "objectModel",
                new XAttribute("xmlns", defaultNs.NamespaceName),
                new XAttribute(XNamespace.Xmlns + "xsi", xsiNs.NamespaceName),
                new XAttribute(xsiNs + "schemaLocation", schemaNs.NamespaceName));
            var xDocument = new XDocument(new XDeclaration("1.0", "utf-8", null), root);
            xDocument = _modelIdentificationSectionWriter.WriteFomSection(fomModule.ModelIdentificationSection, xDocument);
            xDocument = _objectClassSectionWriter.WriteFomSection(fomModule.ObjectClassSection, xDocument);
            xDocument = _interactionClassSectionWriter.WriteFomSection(fomModule.InteractionClassSection, xDocument);
            xDocument = _dimensionSectionWriter.WriteFomSection(fomModule.DimensionSection, xDocument);
            xDocument = _timeSectionWriter.WriteFomSection(fomModule.TimeSection, xDocument);
            xDocument = _tagSectionWriter.WriteFomSection(fomModule.TagSection, xDocument);
            xDocument = _synchronizationSectionWriter.WriteFomSection(fomModule.SynchronizationSection, xDocument);
            xDocument = _transportationSectionWriter.WriteFomSection(fomModule.TransportationSection, xDocument);
            xDocument = _switchSectionWriter.WriteFomSection(fomModule.SwitchSection, xDocument);
            xDocument = _updateRateSectionWriter.WriteFomSection(fomModule.UpdateRatesSection, xDocument);
            xDocument = _dataTypeSectionWriter.WriteFomSection(fomModule.DataTypeSection, xDocument);
            return xDocument;
        }
    }
}
