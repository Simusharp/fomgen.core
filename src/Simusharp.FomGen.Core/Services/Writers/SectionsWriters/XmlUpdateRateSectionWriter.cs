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
    internal class XmlUpdateRateSectionWriter : XmlSectionWriterBase, IXmlFomSectionWriter
    {
        public XDocument WriteFomSection(FomSection fomSection, XDocument xDocument)
        {
            if (fomSection == null)
            {
                return xDocument;
            }

            var parentElement = GetParentElement(fomSection, xDocument);

            if (fomSection is not UpdateRatesSection updateRatesSection)
            {
                throw new ArgumentException("Error casting to Update Rate section");
            }

            if (updateRatesSection.Count > 0)
            {
                var ns = xDocument.Root?.Name.Namespace;
                var updateSecElement = new XElement(ns + updateRatesSection.SectionName);
                foreach (var s in updateRatesSection)
                {
                    var updateElement = new XElement(ns + "updateRate");
                    updateElement.Add(new XElement(ns + "name", s.Name));
                    updateElement.Add(new XElement(ns + "rate", s.Rate));
                    if (!string.IsNullOrWhiteSpace(s.Semantics))
                    {
                        updateElement.Add(new XElement(ns + "semantics", s.Semantics));
                    }

                    updateSecElement.Add(updateElement);
                }

                parentElement.Add(updateSecElement); 
            }

            return xDocument;
        }
    }
}
