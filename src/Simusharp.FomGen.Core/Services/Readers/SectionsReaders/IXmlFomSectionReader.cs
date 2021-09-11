/*
 *   Copyright 2021 Simusharp
 *   Don't remove this header
 *   Distributed under the MIT License.
 */

using System.Xml.Linq;
using Simusharp.FomGen.Core.Models;

namespace Simusharp.FomGen.Core.Services.Readers.SectionsReaders
{
    public interface IXmlFomSectionReader
    {
        FomSection ReadFomSection(XDocument xDocument);
    }
}
