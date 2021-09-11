/*
 *   Copyright 2021 Simusharp
 *   Don't remove this header
 *   Distributed under the MIT License.
 */

using System;
using System.Xml.Linq;
using Simusharp.FomGen.Core.Models;

namespace Simusharp.FomGen.Core.Services.Writers.SectionsWriters
{
    internal class XmlModelIdentificationSectionWriter : XmlSectionWriterBase, IXmlFomSectionWriter
    {
        public XDocument WriteFomSection(FomSection fomSection, XDocument xDocument)
        {
            if (fomSection == null)
            {
                return xDocument;
            }

            var parentElement = GetParentElement(fomSection, xDocument);

            var ns = xDocument.Root?.Name.Namespace;

            if (fomSection is not ModelIdentificationSection model)
            {
                throw new FomWriterException("Error casting to Model Identification section");
            }

            var modelIdentificationElement = new XElement(ns + model.SectionName, string.Empty);
            var shouldAdd = false;
            if (!string.IsNullOrWhiteSpace(model.Name))
            {
                shouldAdd = true;
                modelIdentificationElement.Add(new XElement(ns + "name", model.Name));
            }

            if (!string.IsNullOrWhiteSpace(model.Type))
            {
                shouldAdd = true;
                modelIdentificationElement.Add(new XElement(ns + "type", model.Type));
            }

            if (!string.IsNullOrWhiteSpace(model.Version))
            {
                shouldAdd = true;
                modelIdentificationElement.Add(new XElement(ns + "version", model.Version));
            }

            if (model.ModificationDate.HasValue)
            {
                shouldAdd = true;
                modelIdentificationElement.Add(new XElement(ns + "modificationDate", model.ModificationDate.Value.ToString("yyyy-MM-dd")));
            }

            if (!string.IsNullOrWhiteSpace(model.SecurityClassification))
            {
                shouldAdd = true;
                modelIdentificationElement.Add(new XElement(ns + "securityClassification", model.SecurityClassification));
            }

            foreach (var item in model.ReleaseRestrictions)
            {
                shouldAdd = true;
                modelIdentificationElement.Add(new XElement(ns + "releaseRestriction", item));
            }

            if (!string.IsNullOrWhiteSpace(model.Purpose))
            {
                shouldAdd = true;
                modelIdentificationElement.Add(new XElement(ns + "purpose", model.Purpose));
            }

            if (!string.IsNullOrWhiteSpace(model.ApplicationDomain))
            {
                shouldAdd = true;
                modelIdentificationElement.Add(new XElement(ns + "applicationDomain", model.ApplicationDomain));
            }

            if (!string.IsNullOrWhiteSpace(model.Description))
            {
                shouldAdd = true;
                modelIdentificationElement.Add(new XElement(ns + "description", model.Description));
            }

            if (!string.IsNullOrWhiteSpace(model.UseLimitation))
            {
                shouldAdd = true;
                modelIdentificationElement.Add(new XElement(ns + "useLimitation", model.UseLimitation));
            }

            foreach (var item in model.UseHistory)
            {
                shouldAdd = true;
                modelIdentificationElement.Add(new XElement(ns + "useHistory", item));
            }

            foreach (var keyword in model.Keywords)
            {
                shouldAdd = true;
                var keywordElement = new XElement(ns + "keyword");
                if (!string.IsNullOrWhiteSpace(keyword.Taxonomy))
                {
                    keywordElement.Add(new XElement(ns + "taxonomy", keyword.Taxonomy));
                }

                if (!string.IsNullOrWhiteSpace(keyword.KeywordValue))
                {
                    keywordElement.Add(new XElement(ns + "keywordValue", keyword.KeywordValue));
                }

                modelIdentificationElement.Add(keywordElement);
            }

            foreach (var poc in model.POC)
            {
                shouldAdd = true;
                var pocElement = new XElement(ns + "poc");
                if (!string.IsNullOrWhiteSpace(poc.Type))
                {
                    pocElement.Add(new XElement(ns + "pocType", poc.Type));
                }

                if (!string.IsNullOrWhiteSpace(poc.Name))
                {
                    pocElement.Add(new XElement(ns + "pocName", poc.Name));
                }

                if (!string.IsNullOrWhiteSpace(poc.Organization))
                {
                    pocElement.Add(new XElement(ns + "pocOrg", poc.Organization));
                }

                foreach (var phone in poc.Phones)
                {
                    pocElement.Add(new XElement(ns + "pocTelephone", phone));
                }

                foreach (var email in poc.Emails)
                {
                    pocElement.Add(new XElement(ns + "pocEmail", email));
                }

                modelIdentificationElement.Add(pocElement);
            }

            foreach (var reference in model.References)
            {
                shouldAdd = true;
                var refElement = new XElement(ns + "reference");
                if (!string.IsNullOrWhiteSpace(reference.Type))
                {
                    refElement.Add(new XElement(ns + "type", reference.Type));
                }

                if (!string.IsNullOrWhiteSpace(reference.Identification))
                {
                    refElement.Add(new XElement(ns + "identification", reference.Identification));
                }

                modelIdentificationElement.Add(refElement);
            }

            if (!string.IsNullOrWhiteSpace(model.Other))
            {
                shouldAdd = true;
                modelIdentificationElement.Add(new XElement(ns + "other", model.Other));
            }

            if (model.Glyph != null)
            {
                shouldAdd = true;
                var glyphElement = new XElement(ns + "glyph",
                    new XAttribute("type", model.Glyph.Type),
                    model.Glyph.DecodedImage);

                if (!string.IsNullOrWhiteSpace(model.Glyph.Alt))
                {
                    glyphElement.Add(new XAttribute("alt", model.Glyph.Alt));
                }

                if (model.Glyph.Height.HasValue)
                {
                    glyphElement.Add(new XAttribute("height", model.Glyph.Height.Value));
                }

                if (model.Glyph.Width.HasValue)
                {
                    glyphElement.Add(new XAttribute("width", model.Glyph.Width.Value));
                }

                modelIdentificationElement.Add(glyphElement);
            }

            if (shouldAdd)
            {
                parentElement.Add(modelIdentificationElement); 
            }

            return xDocument;
        }
    }
}
