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
    internal class XmlSynchronizationSectionWriter : XmlSectionWriterBase, IXmlFomSectionWriter
    {
        public XDocument WriteFomSection(FomSection fomSection, XDocument xDocument)
        {
            if (fomSection == null)
            {
                return xDocument;
            }

            var parentElement = GetParentElement(fomSection, xDocument);

            if (fomSection is not SynchronizationSection synchronizationSection)
            {
                throw new FomWriterException("Error casting to Synchronization section");
            }

            if (synchronizationSection.Count > 0)
            {
                var ns = xDocument.Root?.Name.Namespace;
                var tsElement = new XElement(ns + synchronizationSection.SectionName);
                foreach (var synchronization in synchronizationSection)
                {
                    var syncPointElement = new XElement(ns + "synchronizationPoint",
                        new XElement(ns + "label", synchronization.Label));
                    if (!string.IsNullOrWhiteSpace(synchronization.TagDataType) && !synchronization.TagDataType.Equals("NA"))
                    {
                        syncPointElement.Add(new XElement(ns + "dataType", synchronization.TagDataType));
                    }

                    if (synchronization.Capability.HasValue)
                    {
                        syncPointElement.Add(new XElement(ns + "capability", EnumToString(synchronization.Capability.Value)));
                    }

                    if (!string.IsNullOrWhiteSpace(synchronization.Semantics))
                    {
                        syncPointElement.Add(new XElement(ns + "semantics", synchronization.Semantics));
                    }

                    tsElement.Add(syncPointElement);
                }

                parentElement.Add(tsElement); 
            }

            return xDocument;
        }

        private static string EnumToString(CapabilityType capabilityType)
        {
            switch (capabilityType)
            {
                case CapabilityType.Register:
                    return "Register";
                case CapabilityType.Achieve:
                    return "Achieve";
                case CapabilityType.RegisterAchieve:
                    return "RegisterAchieve";
                case CapabilityType.NoSynch:
                    return "NoSynch";
                case CapabilityType.NA:
                    return "NA";
                default:
                    throw new ArgumentOutOfRangeException(nameof(capabilityType), capabilityType, null);
            }
        }
    }
}
