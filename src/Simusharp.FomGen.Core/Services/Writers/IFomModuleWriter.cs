/*
 *   Copyright 2021 Simusharp
 *   Don't remove this header
 *   Distributed under the MIT License.
 */

using System.IO;
using System.Xml.Linq;
using Simusharp.FomGen.Core.Models;

namespace Simusharp.FomGen.Core.Services.Writers
{
    public interface IFomModuleWriter
    {
        void WriteModule(IFomModule fomModule, Stream stream);

        XDocument GetXml(IFomModule fomModule);
    }
}
