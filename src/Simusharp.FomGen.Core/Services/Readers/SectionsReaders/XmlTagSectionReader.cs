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
    internal class XmlTagSectionReader : IXmlFomSectionReader
    {
        public FomSection ReadFomSection(XDocument xDocument)
        {
            if (xDocument?.Root == null)
            {
                throw new ArgumentNullException(nameof(xDocument));
            }

            var tagSection = new TagSection();
            var ns = xDocument.Root.Name.Namespace;
            var elements = xDocument.Root.Elements(ns + tagSection.SectionName).ToArray();
            if (elements.Length == 0)
            {
                return null;
            }

            if (elements.Length > 1)
            {
                throw new FomReaderException("The xml document has multiple Tag sections");
            }

            foreach (var xElement in elements[0].Elements())
            {
                tagSection.Add(new UserTag
                {
                    Category = ToEnum(xElement.Name.LocalName),
                    DataType = xElement.Element(ns + "dataType")?.Value,
                    Semantics = xElement.Element(ns + "semantics")?.Value
                });
            }

            return tagSection;
        }

        private static UserTagType ToEnum(string value)
        {
            switch (value)
            {
                case "updateReflectTag":
                    return UserTagType.UpdateReflectTag;
                case "sendReceiveTag":
                    return UserTagType.SendReceiveTag;
                case "deleteRemoveTag":
                    return UserTagType.DeleteRemoveTag;
                case "divestitureRequestTag":
                    return UserTagType.DivestitureRequestTag;
                case "divestitureCompletionTag":
                    return UserTagType.DivestitureCompletionTag;
                case "acquisitionRequestTag":
                    return UserTagType.AcquisitionRequestTag;
                case "requestUpdateTag":
                    return UserTagType.RequestUpdateTag;
                default:
                    throw new FomReaderException($"Unknown user tag type: {value}");
            }
        }
    }
}
