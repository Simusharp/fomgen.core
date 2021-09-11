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
    internal class XmlDimensionSectionReader : IXmlFomSectionReader
    {
        public FomSection ReadFomSection(XDocument xDocument)
        {
            if (xDocument?.Root == null)
            {
                throw new ArgumentNullException(nameof(xDocument));
            }

            var dimensionSection = new DimensionSection();
            var ns = xDocument.Root.Name.Namespace;
            var elements = xDocument.Root.Elements(ns + dimensionSection.SectionName).ToArray();
            if (elements.Length == 0)
            {
                return null;
            }

            if (elements.Length > 1)
            {
                throw new FomReaderException("The xml document has multiple Dimension sections");
            }

            foreach (var xElement in elements[0].Elements())
            {
                var dimension = new Dimension
                {
                    Name = xElement.Element(ns + "name")?.Value,
                    DataType = xElement.Element(ns + "dataType")?.Value,
                    NormalizationFunction = xElement.Element(ns + "normalization")?.Value,
                    ValueWhenUnspecified = xElement.Element(ns + "value")?.Value
                };

                var upperBound = xElement.Element(ns + "upperBound")?.Value;
                if (!string.IsNullOrWhiteSpace(upperBound))
                {
                    dimension.UpperBound = int.Parse(upperBound);
                }

                dimensionSection.Add(dimension);
            }

            return dimensionSection;
        }
    }
}