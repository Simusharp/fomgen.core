/*
 *   Copyright 2021 Simusharp
 *   Don't remove this header
 *   Distributed under the MIT License.
 */

using Simusharp.FomGen.Core.Models;
using System;
using System.Linq;
using System.Xml.Linq;

namespace Simusharp.FomGen.Core.Services.Writers.SectionsWriters
{
    internal class XmlTagSectionWriter : XmlSectionWriterBase, IXmlFomSectionWriter
    {
        public XDocument WriteFomSection(FomSection fomSection, XDocument xDocument)
        {
            if (fomSection == null)
            {
                return xDocument;
            }

            var parentElement = GetParentElement(fomSection, xDocument);

            if (fomSection is not TagSection tagSection)
            {
                throw new ArgumentException("Error casting to Tag section");
            }

            if (tagSection.Count > 0)
            {
                var ns = xDocument.Root?.Name.Namespace;
                var tsElement = new XElement(ns + tagSection.SectionName);
                foreach (var tag in tagSection.OrderBy(x => x.Category))
                {
                    var tagElement = new XElement(ns + EnumToString(tag.Category));
                    if (!string.IsNullOrWhiteSpace(tag.DataType))
                    {
                        tagElement.Add(new XElement(ns + "dataType", tag.DataType));
                    }

                    if (!string.IsNullOrWhiteSpace(tag.Semantics))
                    {
                        tagElement.Add(new XElement(ns + "semantics", tag.Semantics));
                    }

                    tsElement.Add(tagElement);
                }

                parentElement.Add(tsElement); 
            }

            return xDocument;
        }

        private static string EnumToString(UserTagType userTagType)
        {
            switch (userTagType)
            {
                case UserTagType.UpdateReflectTag:
                    return "updateReflectTag";
                case UserTagType.SendReceiveTag:
                    return "sendReceiveTag";
                case UserTagType.DeleteRemoveTag:
                    return "deleteRemoveTag";
                case UserTagType.DivestitureRequestTag:
                    return "divestitureRequestTag";
                case UserTagType.DivestitureCompletionTag:
                    return "divestitureCompletionTag";
                case UserTagType.AcquisitionRequestTag:
                    return "acquisitionRequestTag";
                case UserTagType.RequestUpdateTag:
                    return "requestUpdateTag";
                default:
                    throw new FomWriterException($"Unknown user tag type: {nameof(userTagType)}");
            }
        }
    }
}
