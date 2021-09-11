/*
 *   Copyright 2021 Simusharp
 *   Don't remove this header
 *   Distributed under the MIT License.
 */

using System;
using System.Linq;
using System.Xml.Linq;
using Simusharp.FomGen.Core.Models;

namespace Simusharp.FomGen.Core.Services.Writers.SectionsWriters
{
    internal abstract class XmlSectionWriterBase
    {
        protected XElement GetParentElement(FomSection fomSection, XDocument xDocument)
        {
            if (fomSection == null)
            {
                throw new ArgumentNullException(nameof(fomSection));
            }

            if (xDocument?.Root == null)
            {
                throw new ArgumentNullException(nameof(xDocument));
            }

            var ns = xDocument.Root.Name.Namespace;
            var parentElements = xDocument.Descendants(ns + "objectModel").ToList();

            if (parentElements.Count == 0)
            {
                throw new FomReaderException("Document missing the parent element 'objectModel'");
            }

            if (parentElements.Count > 1)
            {
                throw new FomReaderException("Document has two parent element 'objectModel'");
            }

            return parentElements.First();
        }
    }
}
