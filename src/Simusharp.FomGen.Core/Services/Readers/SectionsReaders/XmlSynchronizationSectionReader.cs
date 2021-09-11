/*
 *   Copyright 2021 Simusharp
 *   Don't remove this header
 *   Distributed under the MIT License.
 */

using System;
using System.Linq;
using System.Xml.Linq;
using Simusharp.FomGen.Core.Models;

namespace Simusharp.FomGen.Core.Services.Readers.SectionsReaders
{
    internal class XmlSynchronizationSectionReader : IXmlFomSectionReader
    {
        public FomSection ReadFomSection(XDocument xDocument)
        {
            if (xDocument?.Root == null)
            {
                throw new ArgumentNullException(nameof(xDocument));
            }

            var synchronizationSection = new SynchronizationSection();
            var ns = xDocument.Root.Name.Namespace;
            var elements = xDocument.Root.Elements(ns + synchronizationSection.SectionName).ToArray();
            if (elements.Length == 0)
            {
                return null;
            }

            if (elements.Length > 1)
            {
                throw new FomReaderException("The xml document has multiple Synchronization sections");
            }


            foreach (var xElement in elements[0].Elements())
            {
                synchronizationSection.Add(new Synchronization
                {
                    Label = xElement.Element(ns + "label")?.Value,
                    TagDataType = xElement.Element(ns + "dataType")?.Value ?? "NA",
                    Capability = ToEnum(xElement.Element(ns + "capability")?.Value),
                    Semantics = xElement.Element(ns + "semantics")?.Value
                });
            }

            return synchronizationSection;
        }

        private static CapabilityType? ToEnum(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return null;
            }

            switch (value)
            {
                case "Register":
                    return CapabilityType.Register;
                case "Achieve":
                    return CapabilityType.Achieve;
                case "RegisterAchieve":
                    return CapabilityType.RegisterAchieve;
                case "NoSynch":
                    return CapabilityType.NoSynch;
                case "NA":
                    return CapabilityType.NA;
                default:
                    throw new FomReaderException($"Unknown capability type: {value}");
            }
        }
    }
}
