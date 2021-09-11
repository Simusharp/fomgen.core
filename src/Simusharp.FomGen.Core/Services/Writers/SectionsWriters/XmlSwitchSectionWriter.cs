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
    internal class XmlSwitchSectionWriter : XmlSectionWriterBase, IXmlFomSectionWriter
    {
        public XDocument WriteFomSection(FomSection fomSection, XDocument xDocument)
        {
            if (fomSection == null)
            {
                return xDocument;
            }

            var parentElement = GetParentElement(fomSection, xDocument);

            if (fomSection is not SwitchSection ss)
            {
                throw new FomWriterException("Error casting to switch section");
            }

            var ns = xDocument.Root?.Name.Namespace;
            var switchSecElement = new XElement(ns + ss.SectionName, string.Empty);
            switchSecElement.Add(new XElement(ns + "autoProvide", new XAttribute("isEnabled", ss.AutoProvide)));
            switchSecElement.Add(new XElement(ns + "conveyRegionDesignatorSets", new XAttribute("isEnabled", ss.ConveyRegionDesignatorSets)));
            switchSecElement.Add(new XElement(ns + "conveyProducingFederate", new XAttribute("isEnabled", ss.ConveyProducingFederate)));
            switchSecElement.Add(new XElement(ns + "attributeScopeAdvisory", new XAttribute("isEnabled", ss.AttributeScopeAdvisory)));
            switchSecElement.Add(new XElement(ns + "attributeRelevanceAdvisory", new XAttribute("isEnabled", ss.AttributeRelevanceAdvisory)));
            switchSecElement.Add(new XElement(ns + "objectClassRelevanceAdvisory", new XAttribute("isEnabled", ss.ObjectClassRelevanceAdvisory)));
            switchSecElement.Add(new XElement(ns + "interactionRelevanceAdvisory", new XAttribute("isEnabled", ss.InteractionRelevanceAdvisory)));
            switchSecElement.Add(new XElement(ns + "serviceReporting", new XAttribute("isEnabled", ss.ServiceReporting)));
            switchSecElement.Add(new XElement(ns + "exceptionReporting", new XAttribute("isEnabled", ss.ExceptionReporting)));
            switchSecElement.Add(new XElement(ns + "delaySubscriptionEvaluation", new XAttribute("isEnabled", ss.DelaySubscriptionEvaluation)));
            if (ss.AutomaticResignSwitch.HasValue)
            {
                switchSecElement.Add(new XElement(ns + "automaticResignAction", new XAttribute("resignAction", ss.AutomaticResignSwitch.Value.ToString("g"))));
            }

            parentElement.Add(switchSecElement);
            return xDocument;
        }
    }
}
