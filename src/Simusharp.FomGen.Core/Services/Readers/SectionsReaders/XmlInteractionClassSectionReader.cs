/*
 *   Copyright 2021 Simusharp
 *   Don't remove this header
 *   Distributed under the MIT License.
 */

using Simusharp.FomGen.Core.Models;
using Simusharp.FomGen.Core.Util;
using System;
using System.Linq;
using System.Xml.Linq;

namespace Simusharp.FomGen.Core.Services.Readers.SectionsReaders
{
    public class XmlInteractionClassSectionReader : IXmlFomSectionReader
    {
        public FomSection ReadFomSection(XDocument xDocument)
        {
            if (xDocument?.Root == null)
            {
                throw new ArgumentNullException(nameof(xDocument));
            }

            var section = new InteractionClassSection();
            var ns = xDocument.Root.Name.Namespace;
            var elements = xDocument.Root.Elements(ns + section.SectionName).ToArray();
            if (elements.Length == 0)
            {
                return null;
            }

            if (elements.Length > 1)
            {
                throw new FomReaderException("The xml document has multiple interaction class sections");
            }

            var rootElements = elements.Elements(ns + "interactionClass").ToArray();
            if (rootElements.Length > 1)
            {
                throw new FomReaderException("The xml document has multiple parent interaction classes");
            }

            if (rootElements.Length == 0)
            {
                throw new FomReaderException("The xml document has no parent interaction class");
            }

            var nameElement = rootElements.Elements(ns + "name").FirstOrDefault();
            if (nameElement == null || !"HLAinteractionRoot".Equals(nameElement.Value))
            {
                throw new FomReaderException("The parent interaction class must be 'HLAinteractionRoot'");
            }

            section.Root = ReadInteractionClass(rootElements[0]);

            return section;
        }

        private static TreeNode<InteractionClass> ReadInteractionClass(XElement element)
        {
            var ns = element.GetDefaultNamespace();
            var interactionClass = new InteractionClass
            {
                Name = element.Element(ns + "name")?.Value,
                Sharing = element.Element(ns + "sharing")?.Value,
                Transportation = element.Element(ns + "transportation")?.Value,
                Order = element.Element(ns + "order")?.Value,
                Semantics = element.Element(ns + "semantics")?.Value
            };

            foreach (var dimElement in element.Elements(ns + "dimensions"))
            {
                interactionClass.AddDimension(dimElement.Element(ns+ "dimension")?.Value);
            }

            foreach (var attributeElement in element.Elements(ns + "parameter"))
            {
                interactionClass.AddParameter(new ParameterItem
                {
                    Name = attributeElement.Element(ns + "name")?.Value,
                    DataType = attributeElement.Element(ns + "dataType")?.Value,
                    Semantics = attributeElement.Element(ns + "semantics")?.Value
                });
            }

            var node = new TreeNode<InteractionClass>(interactionClass);
            foreach (var childElement in element.Elements(ns + "interactionClass"))
            {
                node.Add(ReadInteractionClass(childElement));
            }

            return node;
        }
    }
}
