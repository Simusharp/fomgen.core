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
    internal class XmlUpdateRateSectionReader : IXmlFomSectionReader
    {
        public FomSection ReadFomSection(XDocument xDocument)
        {
            if (xDocument?.Root == null)
            {
                throw new ArgumentNullException(nameof(xDocument));
            }

            var ns = xDocument.Root.Name.Namespace;
            var updateRatesSection = new UpdateRatesSection();
            var elements = xDocument.Root.Elements(ns + updateRatesSection.SectionName).ToList();
            if (elements.Count == 0)
            {
                return null;
            }

            if (elements.Count > 1)
            {
                throw new ArgumentException("The xml document has multiple switches sections");
            }

            foreach (var xElement in elements.Elements())
            {
                var names = xElement.Elements(ns + "name").ToArray();
                if (names.Length != 1)
                {
                    throw new ArgumentException("Update Rate must contain one name element");
                }

                var rates = xElement.Elements(ns + "rate").ToArray();
                if (rates.Length != 1)
                {
                    throw new ArgumentException("Update Rate must contain one rate element");
                }

                updateRatesSection.Add(new UpdateRate
                {
                    Name = names[0].Value,
                    Rate = decimal.Parse(rates[0].Value),
                    Semantics = xElement.Element(ns + "semantics")?.Value
                });
            }

            return updateRatesSection;
        }
    }
}
