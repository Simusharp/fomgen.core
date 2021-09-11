/*
 *   Copyright 2021 Simusharp
 *   Don't remove this header
 *   Distributed under the MIT License.
 */

using System.IO;
using System.Xml;
using System.Xml.Linq;
using Simusharp.FomGen.Core.Models;
using Simusharp.FomGen.Core.Services.Readers.SectionsReaders;

namespace Simusharp.FomGen.Core.Services.Readers
{
    public class XmlFomModuleReader : IFomModuleReader
    {
        private readonly XmlDataTypeSectionReader _dataTypeSectionReader = new();
        private readonly XmlDimensionSectionReader _dimensionSectionReader = new();
        private readonly XmlInteractionClassSectionReader _interactionClassSectionReader = new();
        private readonly XmlModelIdentificationSectionReader _modelIdentificationSectionReader = new();
        private readonly XmlObjectClassSectionReader _objectClassSectionReader = new();
        private readonly XmlSwitchSectionReader _switchSectionReader = new();
        private readonly XmlSynchronizationSectionReader _synchronizationSectionReader = new();
        private readonly XmlTagSectionReader _tagSectionReader = new();
        private readonly XmlTransportationSectionReader _transportationSectionReader = new();
        private readonly XmlUpdateRateSectionReader _updateRateSectionReader = new();
        private readonly XmlTimeSectionReader _timeSectionReader = new();

        public IFomModule ReadFomModule(Stream stream)
        {
            using (var reader = XmlReader.Create(stream))
            {
                var fom = new FomModule();
                var xDocument = XDocument.Load(reader);

                fom.DataTypeSection = (DataTypeSection) _dataTypeSectionReader.ReadFomSection(xDocument);
                fom.DimensionSection = (DimensionSection) _dimensionSectionReader.ReadFomSection(xDocument);
                fom.InteractionClassSection = (InteractionClassSection) _interactionClassSectionReader.ReadFomSection(xDocument);
                fom.ModelIdentificationSection = (ModelIdentificationSection) _modelIdentificationSectionReader.ReadFomSection(xDocument);
                fom.ObjectClassSection = (ObjectClassSection) _objectClassSectionReader.ReadFomSection(xDocument);
                fom.SwitchSection = (SwitchSection) _switchSectionReader.ReadFomSection(xDocument);
                fom.SynchronizationSection = (SynchronizationSection)_synchronizationSectionReader.ReadFomSection(xDocument);
                fom.TagSection = (TagSection) _tagSectionReader.ReadFomSection(xDocument);
                fom.TransportationSection = (TransportationSection) _transportationSectionReader.ReadFomSection(xDocument);
                fom.UpdateRatesSection = (UpdateRatesSection) _updateRateSectionReader.ReadFomSection(xDocument);
                fom.TimeSection = (TimeSection) _timeSectionReader.ReadFomSection(xDocument);

                return fom;
            }
        }
    }
}
