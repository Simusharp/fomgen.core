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
    internal class XmlDimensionSectionWriter : XmlSectionWriterBase, IXmlFomSectionWriter
    {
        public XDocument WriteFomSection(FomSection fomSection, XDocument xDocument)
        {
            if (fomSection == null)
            {
                return xDocument;
            }

            var parentElement = GetParentElement(fomSection, xDocument);

            if (fomSection is not DimensionSection dimensionSection)
            {
                throw new FomWriterException("Error casting to Dimension section");
            }

            var ns = xDocument.Root?.Name.Namespace;
            var dimElement = new XElement(ns + dimensionSection.SectionName);
            foreach (var dimension in dimensionSection)
            {
                var element = new XElement(ns + "dimension",
                    new XElement(ns + "name", dimension.Name));
                if (!string.IsNullOrWhiteSpace(dimension.DataType))
                {
                    element.Add(new XElement(ns + "dataType", dimension.DataType));
                }

                if (dimension.UpperBound.HasValue)
                {
                    element.Add(new XElement(ns + "upperBound", dimension.UpperBound.Value));
                }

                if (!string.IsNullOrWhiteSpace(dimension.NormalizationFunction))
                {
                    element.Add(new XElement(ns + "normalization", dimension.NormalizationFunction));
                }

                if (!string.IsNullOrWhiteSpace(dimension.ValueWhenUnspecified))
                {
                    element.Add(new XElement(ns + "value", dimension.ValueWhenUnspecified));
                }

                dimElement.Add(element);
            }

            parentElement.Add(dimElement);
            return xDocument;
        }
    }
}
