/*
 *   Copyright 2021 Simusharp
 *   Don't remove this header
 *   Distributed under the MIT License.
 */

using System;
using System.Linq;
using System.Xml.Linq;
using Simusharp.FomGen.Core.Models;
using Simusharp.FomGen.Core.Util;

namespace Simusharp.FomGen.Core.Services.Readers.SectionsReaders
{
    internal class XmlObjectClassSectionReader : IXmlFomSectionReader
    {
        public FomSection ReadFomSection(XDocument xDocument)
        {
            if (xDocument?.Root == null)
            {
                throw new ArgumentNullException(nameof(xDocument));
            }

            var section = new ObjectClassSection();
            var ns = xDocument.Root.Name.Namespace;
            var elements = xDocument.Root.Elements(ns + section.SectionName).ToArray();
            if (elements.Length == 0)
            {
                return null;
            }

            if (elements.Length > 1)
            {
                throw new FomReaderException("The xml document has multiple object class sections");
            }

            var rootElements = elements.Elements(ns + "objectClass").ToArray();
            if (rootElements.Length > 1)
            {
                throw new FomReaderException("The xml document has multiple parent object classes");
            }

            if (rootElements.Length == 0)
            {
                throw new FomReaderException("The xml document has no parent object class");
            }

            var nameElement = rootElements.Elements(ns + "name").FirstOrDefault();
            if (nameElement == null || !"HLAobjectRoot".Equals(nameElement.Value))
            {
                throw new FomReaderException("The parent object class must be 'HLAobjectRoot'");
            }

            section.Root = ReadObjectClass(rootElements[0]);

            return section;
        }

        private static TreeNode<ObjectClass> ReadObjectClass(XElement element)
        {
            var ns = element.GetDefaultNamespace();
            var objectClass = new ObjectClass
            {
                Name = element.Element(ns + "name")?.Value,
                Sharing = element.Element(ns + "sharing")?.Value,
                Semantics = element.Element(ns + "semantics")?.Value
            };

            foreach (var attributeElement in element.Elements(ns + "attribute"))
            {
                var attributeItem = new AttributeItem
                {
                    Name = attributeElement.Element(ns + "name")?.Value,
                    DataType = attributeElement.Element(ns + "dataType")?.Value,
                    UpdateType = attributeElement.Element(ns + "updateType")?.Value,
                    UpdateCondition = attributeElement.Element(ns + "updateCondition")?.Value,
                    Ownership = attributeElement.Element(ns + "ownership")?.Value,
                    Sharing = attributeElement.Element(ns + "sharing")?.Value,
                    Transportation = attributeElement.Element(ns + "transportation")?.Value,
                    Order = attributeElement.Element(ns + "order")?.Value,
                    Semantics = attributeElement.Element(ns + "semantics")?.Value
                };

                var dimensionElements = attributeElement.Element(ns + "dimensions")?.Elements(ns + "dimension");
                if (dimensionElements != null)
                {
                    foreach (var dimensionElement in dimensionElements)
                    {
                        attributeItem.AddDimension(dimensionElement.Value);
                    } 
                }

                objectClass.AddAttribute(attributeItem);
            }

            var node = new TreeNode<ObjectClass>(objectClass);
            foreach (var childElement in element.Elements(ns + "objectClass"))
            {
                node.Add(ReadObjectClass(childElement));
            }

            return node;
        }
    }
}
