/*
 *   Copyright 2021 Simusharp
 *   Don't remove this header
 *   Distributed under the MIT License.
 */

using Simusharp.FomGen.Core.Services.Readers;
using Simusharp.FomGen.Core.Services.Writers;

namespace Simusharp.FomGen.Core
{
    public class FomPackageBuilder
    {
        private readonly IFomModuleReader _fomModuleReader;
        private readonly IFomModuleWriter _fomModuleWriter;

        public FomPackageBuilder()
            : this(new XmlFomModuleReader(), new XmlFomModuleWriter())
        {
        }

        private FomPackageBuilder(
            IFomModuleReader fomModuleReader,
            IFomModuleWriter fomModuleWriter)
        {
            this._fomModuleReader = fomModuleReader;
            this._fomModuleWriter = fomModuleWriter;
        }

        public FomPackageBuilder WithReader(IFomModuleReader newValuesQuery)
        {
            return new(newValuesQuery, this._fomModuleWriter);
        }

        public FomPackageBuilder WithWriter(IFomModuleWriter newDispatcher)
        {
            return new(this._fomModuleReader, newDispatcher);
        }

        public FomPackage Create()
        {
            return new(this._fomModuleReader, this._fomModuleWriter);
        }

        public IFomModuleReader FomModuleReader => this._fomModuleReader;

        public IFomModuleWriter FomModuleWriter => this._fomModuleWriter;
    }
}
