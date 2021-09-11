/*
 *   Copyright 2021 Simusharp
 *   Don't remove this header
 *   Distributed under the MIT License.
 */

using System;
using System.Xml.Linq;
using Simusharp.FomGen.Core.Models;
using Simusharp.FomGen.Core.Util;

namespace Simusharp.FomGen.Core.Services.Writers.SectionsWriters
{
    internal class XmlObjectClassSectionWriter : XmlSectionWriterBase, IXmlFomSectionWriter
    {
        public XDocument WriteFomSection(FomSection fomSection, XDocument xDocument)
        {
            if (fomSection == null)
            {
                return xDocument;
            }

            var parentElement = GetParentElement(fomSection, xDocument);

            if (fomSection is not ObjectClassSection objectClassSection)
            {
                throw new FomWriterException("Error casting to object class section");
            }

            var ns = xDocument.Root?.Name.Namespace;
            var objectsElement = new XElement(ns + objectClassSection.SectionName);
            objectsElement.Add(WriteObjectTree(objectClassSection.Root, ns));
            parentElement.Add(objectsElement);
            return xDocument;
        }

        private static XElement WriteObjectTree(TreeNode<ObjectClass> node, XNamespace ns)
        {
            var element = new XElement(ns + "objectClass",
                new XElement(ns + "name", node.Value.Name));
            if (!string.IsNullOrWhiteSpace(node.Value.Sharing))
            {
                element.Add(new XElement(ns + "sharing", node.Value.Sharing));
            }

            if (!string.IsNullOrWhiteSpace(node.Value.Semantics))
            {
                element.Add(new XElement(ns + "semantics", node.Value.Semantics));
            }

            foreach (var attribute in node.Value.Attributes)
            {
                var attributeElement = new XElement(ns + "attribute",
                    new XElement(ns + "name", attribute.Name));

                if (!string.IsNullOrWhiteSpace(attribute.DataType))
                {
                    attributeElement.Add(new XElement(ns + "dataType", attribute.DataType));
                }

                if (!string.IsNullOrWhiteSpace(attribute.UpdateType))
                {
                    attributeElement.Add(new XElement(ns + "updateType", attribute.UpdateType));
                }

                if (!string.IsNullOrWhiteSpace(attribute.UpdateCondition))
                {
                    attributeElement.Add(new XElement(ns + "updateCondition", attribute.UpdateCondition));
                }

                if (!string.IsNullOrWhiteSpace(attribute.Ownership))
                {
                    attributeElement.Add(new XElement(ns + "ownership", attribute.Ownership));
                }

                if (!string.IsNullOrWhiteSpace(attribute.Sharing))
                {
                    attributeElement.Add(new XElement(ns + "sharing", attribute.Sharing));
                }

                if (attribute.Dimensions.Count > 0)
                {
                    var dimensionElements = new XElement(ns + "dimensions");
                    foreach (var dimension in attribute.Dimensions)
                    {
                        dimensionElements.Add(new XElement(ns + "dimension", dimension));
                    }

                    attributeElement.Add(dimensionElements);
                }

                if (!string.IsNullOrWhiteSpace(attribute.Transportation))
                {
                    attributeElement.Add(new XElement(ns + "transportation", attribute.Transportation));
                }

                if (!string.IsNullOrWhiteSpace(attribute.Order))
                {
                    attributeElement.Add(new XElement(ns + "order", attribute.Order));
                }

                if (!string.IsNullOrWhiteSpace(attribute.Semantics))
                {
                    attributeElement.Add(new XElement(ns + "semantics", attribute.Semantics));
                }

                element.Add(attributeElement);
            }

            foreach (var child in node.Children)
            {
                element.Add(WriteObjectTree(child, ns));
            }

            return element;
        }
    }
}
