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
    internal class XmlTransportationSectionWriter : XmlSectionWriterBase, IXmlFomSectionWriter
    {
        public XDocument WriteFomSection(FomSection fomSection, XDocument xDocument)
        {
            if (fomSection == null)
            {
                return xDocument;
            }

            var parentElement = GetParentElement(fomSection, xDocument);

            if (fomSection is not TransportationSection transportationSection)
            {
                throw new ArgumentException("Error casting to Transportation section");
            }

            var ns = xDocument.Root?.Name.Namespace;
            var tsElement = new XElement(ns + transportationSection.SectionName);
            foreach (var transportation in transportationSection)
            {
                var element = new XElement(ns + "transportation",
                    new XElement(ns + "name", transportation.Name),
                    new XElement(ns + "reliable",
                        transportation.Reliability == TransportationType.Reliable ? "Yes" : "No"));
                if (!string.IsNullOrWhiteSpace(transportation.Semantics))
                {
                    element.Add(new XElement(ns + "semantics", transportation.Semantics));
                }

                tsElement.Add(element);
            }

            parentElement.Add(tsElement);
            return xDocument;
        }
    }
}
