/*
 *   Copyright 2021 Simusharp
 *   Don't remove this header
 *   Distributed under the MIT License.
 */

using Simusharp.FomGen.Core.Mergers;
using Simusharp.FomGen.Core.Models;
using Simusharp.FomGen.Core.Services.Readers;
using Simusharp.FomGen.Core.Services.Writers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Xml;
using System.Xml.Schema;

namespace Simusharp.FomGen.Core
{
    public class FomPackage : IEnumerable<IFomModule>, IDisposable
    {
        private readonly IList<IFomModule> _fomModules = new List<IFomModule>();
        private readonly IFomModuleReader _fomModuleReader;
        private readonly IFomModuleWriter _fomModuleWriter;
        private IFomModule _mimModule;
        private const string ReservedMergeKeyword = "MergedFom";
        private Stream _stream = null;

        internal FomPackage(IFomModuleReader fomModuleReader, IFomModuleWriter fomModuleWriter)
        {
            _fomModuleReader = fomModuleReader ?? throw new ArgumentNullException(nameof(fomModuleReader));
            _fomModuleWriter = fomModuleWriter ?? throw new ArgumentNullException(nameof(fomModuleWriter));
            var resourceStream = this.GetType().Assembly.GetManifestResourceStream("Simusharp.FomGen.Core._FOMs.HLAstandardMIM.xml");
            if (resourceStream == null)
            {
                throw new InvalidOperationException("Error retrieving MIM from embedded resource");
            }

            _mimModule = _fomModuleReader.ReadFomModule(resourceStream);
            _mimModule.Name = "HLAstandardMIM";
            resourceStream.Dispose();
        }

        /// <summary>
        /// Set the MIM module for the package
        /// </summary>
        /// <param name="module">The custom MIM module</param>
        public void SetMimModule(IFomModule module)
        {
            if (module == null)
            {
                throw new ArgumentNullException(nameof(module));
            }

            if (string.IsNullOrWhiteSpace(module.Name))
            {
                module.Name = GenerateUniqueName();
            }

            if (!IsValidName(module.Name))
            {
                throw new DuplicateNameException($"Another module has the same name: {module.Name}");
            }

            _mimModule = module;
        }

        /// <summary>
        /// Add a new module with the specified name
        /// </summary>
        /// <param name="name">a unique name, within the package, for the module</param>
        public void AddFomModule(string name)
        {
            if (!IsValidName(name))
            {
                throw new DuplicateNameException($"Another module has the same name: {name} or Reserved keyword: {ReservedMergeKeyword}");
            }

            _fomModules.Add(new FomModule { Name = name });
        }

        /// <summary>
        /// Add a new module with a random name assigned
        /// </summary>
        public void AddFomModule()
        {
            var name = GenerateUniqueName();
            AddFomModule(name);
        }

        /// <summary>
        /// Remove a module with the specified name if exists
        /// </summary>
        /// <param name="name">name of the module to be removed</param>
        public void RemoveModule(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("Name can't be null or empty");
            }

            var module =
                _fomModules.FirstOrDefault(x => x.Name.Equals(name, StringComparison.InvariantCultureIgnoreCase));
            if (module != null)
            {
                _fomModules.Remove(module);
            }
        }

        /// <summary>
        /// Remove the specified module
        /// </summary>
        /// <param name="module">Module to be removed</param>
        public void RemoveModule(IFomModule module)
        {
            _fomModules.Remove(module);
        }

        /// <summary>
        /// Read FOM module from stream and add it to the package
        /// </summary>
        /// <param name="stream">Stream of the FOM module</param>
        /// <param name="name">Name to be assigned to the module</param>
        /// <returns>The read module</returns>
        public IFomModule ReadFomModule(Stream stream, string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("Name can't be null or empty");
            }

            if (!IsValidName(name))
            {
                throw new DuplicateNameException($"Another module has the same name: {name}");
            }

            var module = _fomModuleReader.ReadFomModule(stream);
            module.Name = name;
            _fomModules.Add(module);

            return module;
        }

        /// <summary>
        /// Read FOM module from stream and add it to the package
        /// </summary>
        /// <param name="stream">Stream of the FOM module</param>
        /// <returns>The read module</returns>
        public IFomModule ReadFomModule(Stream stream)
        {
            return ReadFomModule(stream, GenerateUniqueName());
        }

        /// <summary>
        /// Write FOM modules as zip file to a stream
        /// </summary>
        /// <param name="includeMim">Include the MIM module in the package</param>
        /// <returns>A stream of fom modules as zip file</returns>
        public Stream Write(bool includeMim = false)
        {
            _stream = new MemoryStream();
            using var archive = new ZipArchive(_stream, ZipArchiveMode.Create, true);
            if (includeMim)
            {
                var mimFile = archive.CreateEntry($"{_mimModule.Name}.xml");
                using (var stream = mimFile.Open())
                {
                    _fomModuleWriter.WriteModule(_mimModule, stream);
                }
            }

            foreach (var fomModule in _fomModules)
            {
                var file = archive.CreateEntry($"{fomModule.Name}.xml");
                using (var stream = file.Open())
                {
                    _fomModuleWriter.WriteModule(fomModule, stream);
                }
            }

            return _stream;
        }

        /// <summary>
        /// Merge FOM module into one module
        /// </summary>
        /// <param name="includeMim">Include MIM in merging</param>
        /// <returns>The merged Module</returns>
        public IFomModule Merge(bool includeMim)
        {
            var mergedFom = new FomModule { Name = ReservedMergeKeyword };

            var modelSections = _fomModules.Select(x => x.ModelIdentificationSection).ToList();
            var objectSections = _fomModules.Select(x => x.ObjectClassSection).ToList();
            var interactionSections = _fomModules.Select(x => x.InteractionClassSection).ToList();
            var dimensionSections = _fomModules.Select(x => x.DimensionSection).ToList();
            var transportationSections = _fomModules.Select(x => x.TransportationSection).ToList();
            var dataSections = _fomModules.Select(x => x.DataTypeSection).ToList();
            var syncSections = _fomModules.Select(x => x.SynchronizationSection).ToList();
            var tagSections = _fomModules.Select(x => x.TagSection).ToList();
            var timeSections = _fomModules.Select(x => x.TimeSection).ToList();
            var updateSections = _fomModules.Select(x => x.UpdateRatesSection).ToList();
            var switchSections = _fomModules.Select(x => x.SwitchSection).ToList();

            if (includeMim)
            {
                modelSections.Add(_mimModule.ModelIdentificationSection);
                objectSections.Add(_mimModule.ObjectClassSection);
                interactionSections.Add(_mimModule.InteractionClassSection);
                dimensionSections.Add(_mimModule.DimensionSection);
                transportationSections.Add(_mimModule.TransportationSection);
                dataSections.Add(_mimModule.DataTypeSection);
                syncSections.Add(_mimModule.SynchronizationSection);
                tagSections.Add(_mimModule.TagSection);
                timeSections.Add(_mimModule.TimeSection);
                updateSections.Add(_mimModule.UpdateRatesSection);
                switchSections.Add(_mimModule.SwitchSection);
            }

            var modelMerger = new ModelIdentificationMerger();
            mergedFom.ModelIdentificationSection = modelMerger.Merge(modelSections.ToArray());

            var objectMerger = new ObjectClassMerger();
            mergedFom.ObjectClassSection = objectMerger.Merge(objectSections.ToArray());

            var interactionMerger = new InteractionClassMerger();
            mergedFom.InteractionClassSection = interactionMerger.Merge(interactionSections.ToArray());

            var dimensionMerger = new DimensionMerger();
            mergedFom.DimensionSection = dimensionMerger.Merge(dimensionSections.ToArray());

            var transportationMerger = new TransportationMerger();
            mergedFom.TransportationSection = transportationMerger.Merge(transportationSections.ToArray());

            var dataMerger = new DataTypeMerger();
            mergedFom.DataTypeSection = dataMerger.Merge(dataSections.ToArray());

            var syncMerger = new SynchronizationMerger();
            mergedFom.SynchronizationSection = syncMerger.Merge(syncSections.ToArray());

            var tagMerger = new TagMerger();
            mergedFom.TagSection = tagMerger.Merge(tagSections.ToArray());

            var timeMerger = new TimeMerger();
            mergedFom.TimeSection = timeMerger.Merge(timeSections.ToArray());

            var updateMerger = new UpdateRateMerger();
            mergedFom.UpdateRatesSection = updateMerger.Merge(updateSections.ToArray());

            var switchMerger = new SwitchMerger();
            mergedFom.SwitchSection = switchMerger.Merge(switchSections.ToArray());

            // Fill required fields in Model Identification Section, this is required to pass OMT validation
            mergedFom.ModelIdentificationSection.Name = "Merged FOM by FomGen";
            mergedFom.ModelIdentificationSection.Type = "FOM";
            mergedFom.ModelIdentificationSection.Version = "1.0";
            mergedFom.ModelIdentificationSection.ModificationDate = DateTime.Today;
            mergedFom.ModelIdentificationSection.SecurityClassification = "Unclassified";
            mergedFom.ModelIdentificationSection.Description = "Merged FOM by FomGen";
            mergedFom.ModelIdentificationSection.AddPoc(new PointOfContact
            {
                Type = "Primary Author"
            });

            return mergedFom;
        }

        /// <summary>
        /// Merge FOM module into one module and write it to stream
        /// </summary>
        /// <param name="includeMim">Include MIM in merging</param>
        /// <returns>A stream of the merged FOM</returns>
        public Stream MergeToStream(bool includeMim)
        {
            _stream = new MemoryStream();
            var mergedModule = Merge(includeMim);
            _fomModuleWriter.WriteModule(mergedModule, _stream);
            return _stream;
        }

        /// <summary>
        /// Validate the package against naming rules, FDD schema, and OMT schema
        /// </summary>
        /// <returns>A dictionary of errors and associated FOM module</returns>
        public IDictionary<string, IList<ValidationResult>> Validate()
        {
            var dict = new Dictionary<string, IList<ValidationResult>>();
            //TODO: for now we won't validate MIM naming as it contains the standard MIM which have many items starts with HLA
            //var mimErrors = _mimModule.Validate();
            //dict.Add(_mimModule.Name, mimErrors.Select(e => new ValidationResult(e.ErrorMessage, new[] { e.PropertyName })).ToList());

            foreach (var fomModule in _fomModules)
            {
                var errors = fomModule.Validate();
                dict.Add(fomModule.Name, errors.Select(e => new ValidationResult(e.ErrorMessage, new[] { e.PropertyName })).ToList());
            }

            var mergedFom = Merge(true);
            var xDocument = _fomModuleWriter.GetXml(mergedFom);
            var fddSchemaSet = new XmlSchemaSet();
            var omtSchemaSet = new XmlSchemaSet();
            var fddStream = this.GetType().Assembly.GetManifestResourceStream("Simusharp.FomGen.Core._FOMs.IEEE1516-FDD-2010.xsd");
            var omtStream = this.GetType().Assembly.GetManifestResourceStream("Simusharp.FomGen.Core._FOMs.IEEE1516-OMT-2010.xsd");
            if (fddStream == null || omtStream == null)
            {
                throw new InvalidOperationException("Error retrieving XSD from embedded resource");
            }

            fddSchemaSet.Add(null, XmlReader.Create(fddStream));
            omtSchemaSet.Add(null, XmlReader.Create(omtStream));
            var fddResult = new List<ValidationResult>();
            var omtResult = new List<ValidationResult>();

            xDocument.Validate(fddSchemaSet, (_, e) =>
            {
                fddResult.Add(new("FDD Schema", new[] { e.Message }));
            });

            xDocument.Validate(omtSchemaSet, (_, e) =>
            {
                omtResult.Add(new("OMT Schema", new[] { e.Message }));
            });

            dict.Add($"{mergedFom.Name}_FDD", fddResult);
            dict.Add($"{mergedFom.Name}_OMT", omtResult);

            return dict;
        }

        public IEnumerator<IFomModule> GetEnumerator()
        {
            return _fomModules.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        private string GenerateUniqueName()
        {
            string name;
            do
            {
                name = Path.GetFileNameWithoutExtension(Path.GetRandomFileName());
            } while (!IsValidName(name));

            return name;
        }

        private bool IsValidName(string name)
        {
            return !name.Equals(ReservedMergeKeyword, StringComparison.InvariantCultureIgnoreCase) && !name.Equals(_mimModule.Name) && !_fomModules.Any(x => x.Name.Equals(name, StringComparison.InvariantCultureIgnoreCase));
        }

        public void Dispose()
        {
            if (_stream != null)
            {
                _stream.Dispose();
            }
        }
    }
}
