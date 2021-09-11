/*
 *   Copyright 2021 Simusharp
 *   Don't remove this header
 *   Distributed under the MIT License.
 */

using Simusharp.FomGen.Core.Models;
using System;
using System.Xml.Linq;

namespace Simusharp.FomGen.Core.Services.Writers.SectionsWriters
{
    internal class XmlTimeSectionWriter : XmlSectionWriterBase, IXmlFomSectionWriter
    {
        public XDocument WriteFomSection(FomSection fomSection, XDocument xDocument)
        {
            if (fomSection == null)
            {
                return xDocument;
            }

            var parentElement = GetParentElement(fomSection, xDocument);

            if (fomSection is not TimeSection timeSection)
            {
                throw new ArgumentException("Error casting to Time section");
            }

            if (timeSection.LookAhead != null || timeSection.TimeStamp != null)
            {
                var ns = xDocument.Root?.Name.Namespace;
                var tsElement = new XElement(ns + timeSection.SectionName);
                if (timeSection.TimeStamp != null)
                {
                    tsElement.Add(WriteTimeElement(ns, "timeStamp", timeSection.TimeStamp));
                }

                if (timeSection.LookAhead != null)
                {
                    tsElement.Add(WriteTimeElement(ns, "lookahead", timeSection.LookAhead));
                }

                parentElement.Add(tsElement);
            }

            return xDocument;
        }

        private static XElement WriteTimeElement(XNamespace ns, string name, Time time)
        {
            var timeElement = new XElement(ns + name);
            if (!string.IsNullOrWhiteSpace(time.DataType))
            {
                timeElement.Add(new XElement(ns + "dataType", time.DataType));
            }

            if (!string.IsNullOrWhiteSpace(time.Semantics))
            {
                timeElement.Add(new XElement(ns + "semantics", time.Semantics));
            }

            return timeElement;
        }
    }
}
