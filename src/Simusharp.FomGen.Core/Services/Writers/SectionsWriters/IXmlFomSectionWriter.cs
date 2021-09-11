/*
 *   Copyright 2021 Simusharp
 *   Don't remove this header
 *   Distributed under the MIT License.
 */

using System.Xml.Linq;
using Simusharp.FomGen.Core.Models;

namespace Simusharp.FomGen.Core.Services.Writers.SectionsWriters
{
    public interface IXmlFomSectionWriter
    {
        XDocument WriteFomSection(FomSection fomSection, XDocument xDocument);
    }
}
