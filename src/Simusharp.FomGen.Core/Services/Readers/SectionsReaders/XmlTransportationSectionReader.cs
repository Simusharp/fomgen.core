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
    internal class XmlTransportationSectionReader : IXmlFomSectionReader
    {
        public FomSection ReadFomSection(XDocument xDocument)
        {
            if (xDocument?.Root == null)
            {
                throw new ArgumentNullException(nameof(xDocument));
            }

            var transportationSection = new TransportationSection();
            var ns = xDocument.Root.Name.Namespace;
            var elements = xDocument.Root.Elements(ns + transportationSection.SectionName).ToArray();
            if (elements.Length == 0)
            {
                return null;
            }

            if (elements.Length > 1)
            {
                throw new ArgumentException("The xml document has multiple Transportation sections");
            }


            foreach (var xElement in elements[0].Elements())
            {
                transportationSection.Add(new Transportation
                {
                    Name = xElement.Element(ns + "name")?.Value,
                    Reliability = xElement.Element(ns + "reliable")?.Value == "Yes" ? TransportationType.Reliable : TransportationType.BestEffort,
                    Semantics = xElement.Element(ns + "semantics")?.Value
                });
            }

            return transportationSection;
        }
    }
}
