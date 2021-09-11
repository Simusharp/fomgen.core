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
    internal class XmlInteractionClassSectionWriter : XmlSectionWriterBase, IXmlFomSectionWriter
    {
        public XDocument WriteFomSection(FomSection fomSection, XDocument xDocument)
        {
            if (fomSection == null)
            {
                return xDocument;
            }

            var parentElement = GetParentElement(fomSection, xDocument);

            if (fomSection is not InteractionClassSection interactionClassSection)
            {
                throw new FomWriterException("Error casting to interaction class section");
            }

            var ns = xDocument.Root?.Name.Namespace;
            var objectsElement = new XElement(ns + interactionClassSection.SectionName);
            objectsElement.Add(WriteInteractionTree(interactionClassSection.Root, ns));
            parentElement.Add(objectsElement);
            return xDocument;
        }

        private static XElement WriteInteractionTree(TreeNode<InteractionClass> node, XNamespace ns)
        {
            var element = new XElement(ns + "interactionClass",
                new XElement(ns + "name", node.Value.Name));

            if (!string.IsNullOrWhiteSpace(node.Value.Sharing))
            {
                element.Add(new XElement(ns + "sharing", node.Value.Sharing));
            }

            if (node.Value.Dimensions.Count > 0)
            {
                var dimElement = new XElement(ns + "dimensions");
                foreach (var dimension in node.Value.Dimensions)
                {
                    dimElement.Add(new XElement(ns + "dimension", dimension));
                }

                element.Add(dimElement);
            }

            if (!string.IsNullOrWhiteSpace(node.Value.Transportation))
            {
                element.Add(new XElement(ns + "transportation", node.Value.Transportation));
            }

            if (!string.IsNullOrWhiteSpace(node.Value.Order))
            {
                element.Add(new XElement(ns + "order", node.Value.Order));
            }

            if (!string.IsNullOrWhiteSpace(node.Value.Semantics))
            {
                element.Add(new XElement(ns + "semantics", node.Value.Semantics));
            }

            foreach (var parameter in node.Value.Parameters)
            {
                var parameterElement = new XElement(ns + "parameter",
                    new XElement(ns + "name", parameter.Name));
                if (!string.IsNullOrWhiteSpace(parameter.DataType))
                {
                    parameterElement.Add(new XElement(ns + "dataType", parameter.DataType));
                }

                if (!string.IsNullOrWhiteSpace(parameter.Semantics))
                {
                    parameterElement.Add(new XElement(ns + "semantics", parameter.Semantics));
                }

                element.Add(parameterElement);
            }

            foreach (var child in node.Children)
            {
                element.Add(WriteInteractionTree(child, ns));
            }

            return element;
        }
    }
}
