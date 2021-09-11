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
    internal class XmlSwitchSectionReader : IXmlFomSectionReader
    {
        public FomSection ReadFomSection(XDocument xDocument)
        {
            if (xDocument?.Root == null)
            {
                throw new ArgumentNullException(nameof(xDocument));
            }

            var ns = xDocument.Root.Name.Namespace;
            var switchSection = new SwitchSection();
            var elements = xDocument.Root.Elements(ns + switchSection.SectionName).ToList();
            if (elements.Count == 0)
            {
                return null;
            }

            if (elements.Count > 1)
            {
                throw new FomReaderException("The xml document has multiple switches sections");
            }

            foreach (var xElement in elements.Elements())
            {
                switch (xElement.Name.LocalName)
                {
                    case "autoProvide":
                        switchSection.AutoProvide = xElement.Attribute("isEnabled")?.Value == "true";
                        break;
                    case "conveyRegionDesignatorSets":
                        switchSection.ConveyRegionDesignatorSets = xElement.Attribute("isEnabled")?.Value == "true";
                        break;
                    case "conveyProducingFederate":
                        switchSection.ConveyProducingFederate = xElement.Attribute("isEnabled")?.Value == "true";
                        break;
                    case "attributeScopeAdvisory":
                        switchSection.AttributeScopeAdvisory = xElement.Attribute("isEnabled")?.Value == "true";
                        break;
                    case "attributeRelevanceAdvisory":
                        switchSection.AttributeRelevanceAdvisory = xElement.Attribute("isEnabled")?.Value == "true";
                        break;
                    case "objectClassRelevanceAdvisory":
                        switchSection.ObjectClassRelevanceAdvisory = xElement.Attribute("isEnabled")?.Value == "true";
                        break;
                    case "interactionRelevanceAdvisory":
                        switchSection.InteractionRelevanceAdvisory = xElement.Attribute("isEnabled")?.Value == "true";
                        break;
                    case "serviceReporting":
                        switchSection.ServiceReporting = xElement.Attribute("isEnabled")?.Value == "true";
                        break;
                    case "exceptionReporting":
                        switchSection.ExceptionReporting = xElement.Attribute("isEnabled")?.Value == "true";
                        break;
                    case "delaySubscriptionEvaluation":
                        switchSection.DelaySubscriptionEvaluation = xElement.Attribute("isEnabled")?.Value == "true";
                        break;
                    case "automaticResignAction":
                        switch (xElement.Attribute("resignAction")?.Value)
                        {
                            case "UnconditionallyDivestAttributes":
                                switchSection.AutomaticResignSwitch = ResignSwitchType.UnconditionallyDivestAttributes;
                                break;
                            case "DeleteObjects":
                                switchSection.AutomaticResignSwitch = ResignSwitchType.DeleteObjects;
                                break;
                            case "CancelPendingOwnershipAcquisitions":
                                switchSection.AutomaticResignSwitch = ResignSwitchType.CancelPendingOwnershipAcquisitions;
                                break;
                            case "DeleteObjectsThenDivest":
                                switchSection.AutomaticResignSwitch = ResignSwitchType.DeleteObjectsThenDivest;
                                break;
                            case "CancelThenDeleteThenDivest":
                                switchSection.AutomaticResignSwitch = ResignSwitchType.CancelThenDeleteThenDivest;
                                break;
                            case "NoAction":
                                switchSection.AutomaticResignSwitch = ResignSwitchType.NoAction;
                                break;
                        }

                        break;
                }
            }

            return switchSection;
        }
    }
}
