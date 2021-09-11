/*
 *   Copyright 2021 Simusharp
 *   Don't remove this header
 *   Distributed under the MIT License.
 */

using Simusharp.FomGen.Core.Models;
using System;
using System.Linq;
using System.Xml.Linq;

namespace Simusharp.FomGen.Core.Services.Readers.SectionsReaders
{
    internal class XmlTimeSectionReader : IXmlFomSectionReader
    {
        public FomSection ReadFomSection(XDocument xDocument)
        {
            if (xDocument?.Root == null)
            {
                throw new ArgumentNullException(nameof(xDocument));
            }

            var timeSection = new TimeSection();
            var ns = xDocument.Root.Name.Namespace;
            var elements = xDocument.Root.Elements(ns + timeSection.SectionName).ToArray();
            if (elements.Length == 0)
            {
                return null;
            }

            if (elements.Length > 1)
            {
                throw new ArgumentException("The xml document has multiple Time sections");
            }

            foreach (var xElement in elements[0].Elements())
            {
                if ("timeStamp".Equals(xElement.Name.LocalName))
                {
                    timeSection.TimeStamp = new Time
                    {
                        DataType = xElement.Element(ns + "dataType")?.Value,
                        Semantics = xElement.Element(ns + "semantics")?.Value
                    };
                }
                else if ("lookahead".Equals(xElement.Name.LocalName))
                {
                    timeSection.LookAhead = new Time
                    {
                        DataType = xElement.Element(ns + "dataType")?.Value,
                        Semantics = xElement.Element(ns + "semantics")?.Value
                    };
                }
                else
                {
                    throw new FomReaderException(
                        $"Time section contains '{xElement.Name.LocalName}', allowed values are timeStamp & lookahead");
                }
            }

            return timeSection;
        }
    }
}
