/*
 *   Copyright 2021 Simusharp
 *   Don't remove this header
 *   Distributed under the MIT License.
 */

using System;
using System.Globalization;
using System.Linq;
using System.Xml.Linq;
using Simusharp.FomGen.Core.Models;

namespace Simusharp.FomGen.Core.Services.Readers.SectionsReaders
{
    public class XmlModelIdentificationSectionReader : IXmlFomSectionReader
    {
        public FomSection ReadFomSection(XDocument xDocument)
        {
            if (xDocument?.Root == null)
            {
                throw new ArgumentNullException(nameof(xDocument));
            }

            var ns = xDocument.Root.Name.Namespace;
            var section = new ModelIdentificationSection();
            var elements = xDocument.Root.Elements(ns + section.SectionName).ToList();
            if (elements.Count == 0)
            {
                return null;
            }

            if (elements.Count > 1)
            {
                throw new FomReaderException("The xml document has multiple modelIdentification sections");
            }

            var element = elements[0];
            section.Name = element.Element(ns + "name")?.Value;
            section.Type = element.Element(ns + "type")?.Value;
            section.Version = element.Element(ns + "version")?.Value;
            section.ModificationDate = DateTime.ParseExact(element.Element(ns + "modificationDate")?.Value, "yyyy-MM-dd", CultureInfo.InvariantCulture);
            section.SecurityClassification = element.Element(ns + "securityClassification")?.Value;
            foreach (var releaseElement in element.Elements(ns + "releaseRestriction"))
            {
                section.AddReleaseRestriction(releaseElement.Value);
            }

            section.Purpose = element.Element(ns + "purpose")?.Value;
            section.ApplicationDomain = element.Element(ns + "applicationDomain")?.Value;
            section.Description = element.Element(ns + "description")?.Value;
            section.UseLimitation = element.Element(ns + "useLimitation")?.Value;
            foreach (var historyElement in element.Elements(ns + "useHistory"))
            {
                section.AddUseHistory(historyElement.Value);
            }

            foreach (var keywordElement in element.Elements(ns + "keyword"))
            {
                section.AddKeyword(new Keyword
                {
                    Taxonomy = keywordElement.Element(ns + "taxonomy")?.Value,
                    KeywordValue = keywordElement.Element(ns + "keywordValue")?.Value
                });
            }

            foreach (var pocElement in element.Elements(ns + "poc"))
            {
                var poc = new PointOfContact
                {
                    Type = pocElement.Element(ns + "pocType")?.Value,
                    Name = pocElement.Element(ns + "pocName")?.Value,
                    Organization = pocElement.Element(ns + "pocOrg")?.Value
                };

                foreach (var phoneElement in pocElement.Elements(ns + "pocTelephone"))
                {
                    poc.Phones.Add(phoneElement.Value);
                }

                foreach (var emailElement in pocElement.Elements(ns + "pocEmail"))
                {
                    poc.Emails.Add(emailElement.Value);
                }

                section.AddPoc(poc);
            }

            foreach (var refElement in element.Elements(ns + "reference"))
            {
                section.AddReference(new Reference
                {
                    Type = refElement.Element(ns + "type")?.Value,
                    Identification = refElement.Element(ns + "identification")?.Value
                });
            }

            section.Other = element.Element(ns + "other")?.Value;
            var glyphElement = element.Element(ns + "glyph");

            var glyph = new Glyph
            {
                DecodedImage = glyphElement?.Value,
                Alt = glyphElement?.Attribute("alt")?.Value,
                Type = glyphElement?.Attribute("type")?.Value
            };

            if (double.TryParse(glyphElement?.Attribute("height")?.Value, out var h))
            {
                glyph.Height = h;
            }

            if (double.TryParse(glyphElement?.Attribute("width")?.Value, out var w))
            {
                glyph.Width = w;
            }

            section.Glyph = glyph;

            return section;
        }
    }
}
