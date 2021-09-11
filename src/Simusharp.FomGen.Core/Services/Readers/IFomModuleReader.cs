/*
 *   Copyright 2021 Simusharp
 *   Don't remove this header
 *   Distributed under the MIT License.
 */

using System.IO;
using Simusharp.FomGen.Core.Models;

namespace Simusharp.FomGen.Core.Services.Readers
{
    public interface IFomModuleReader
    {
        IFomModule ReadFomModule(Stream stream);
    }
}
