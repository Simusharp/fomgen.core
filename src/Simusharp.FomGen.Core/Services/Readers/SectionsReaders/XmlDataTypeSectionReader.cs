/*
 *   Copyright 2021 Simusharp
 *   Don't remove this header
 *   Distributed under the MIT License.
 */

using Simusharp.FomGen.Core.Models;
using System;
using System.Linq;
using System.Xml.Linq;

namespace Simusharp.FomGen.Core.Services.Readers.SectionsReaders
{
    internal class XmlDataTypeSectionReader : IXmlFomSectionReader
    {
        public FomSection ReadFomSection(XDocument xDocument)
        {
            if (xDocument?.Root == null)
            {
                throw new ArgumentNullException(nameof(xDocument));
            }

            var dataTypeSection = new DataTypeSection();
            var ns = xDocument.Root.Name.Namespace;
            var elements = xDocument.Root.Elements(ns + dataTypeSection.SectionName).ToArray();
            if (elements.Length == 0)
            {
                return null;
            }

            if (elements.Length > 1)
            {
                throw new FomReaderException("The xml document has multiple Data Type sections");
            }

            // Read basic data
            var basicDataSection = elements.Elements(ns + "basicDataRepresentations").ToArray();
            if (basicDataSection.Length > 1)
            {
                throw new FomReaderException("The xml document has multiple Basic Data Type sections");
            }

            if (basicDataSection.Length == 1)
            {
                foreach (var element in basicDataSection.Elements())
                {
                    if (!"basicData".Equals(element.Name.LocalName))
                    {
                        throw new FomReaderException("Unknown element in the basic data section");
                    }

                    var sizeElement = element.Element(ns + "size");
                    int? size = null;
                    if (sizeElement != null)
                    {
                        if (!int.TryParse(sizeElement.Value, out var s))
                        {
                            throw new FomReaderException($"{nameof(element.Name)}: Size element should be an integer");
                        }

                        size = s;
                    }

                    dataTypeSection.AddBasicData(new BasicData
                    {
                        Name = element.Element(ns + "name")?.Value,
                        Encoding = element.Element(ns + "encoding")?.Value,
                        Endian = element.Element(ns + "endian")?.Value,
                        Interpretation = element.Element(ns + "interpretation")?.Value,
                        Size = size
                    });
                }
            }

            // Read simple data
            var simpleDataSection = elements.Elements(ns + "simpleDataTypes").ToArray();
            if (simpleDataSection.Length > 1)
            {
                throw new FomReaderException("The xml document has multiple Simple Data Type sections");
            }

            if (simpleDataSection.Length == 1)
            {
                foreach (var element in simpleDataSection.Elements())
                {
                    if (!"simpleData".Equals(element.Name.LocalName))
                    {
                        throw new FomReaderException("Unknown element in the simple data section");
                    }

                    dataTypeSection.AddSimpleData(new SimpleData
                    {
                        Name = element.Element(ns + "name")?.Value,
                        Accuracy = element.Element(ns + "accuracy")?.Value,
                        Representation = element.Element(ns + "representation")?.Value,
                        Resolution = element.Element(ns + "resolution")?.Value,
                        Semantics = element.Element(ns + "semantics")?.Value,
                        Units = element.Element(ns + "units")?.Value
                    });
                }
            }

            // Read enumerated data
            var enumeratedDataSection = elements.Elements(ns + "enumeratedDataTypes").ToArray();
            if (enumeratedDataSection.Length > 1)
            {
                throw new FomReaderException("The xml document has multiple Enumerated Data Type sections");
            }

            if (enumeratedDataSection.Length == 1)
            {
                foreach (var element in enumeratedDataSection.Elements())
                {
                    if (!"enumeratedData".Equals(element.Name.LocalName))
                    {
                        throw new FomReaderException("Unknown element in the enumerated data section");
                    }

                    var enumeratedData = new EnumeratedData
                    {
                        Name = element.Element(ns + "name")?.Value,
                        Representation = element.Element(ns + "representation")?.Value,
                        Semantics = element.Element(ns + "semantics")?.Value
                    };

                    foreach (var enumeratorElement in element.Elements(ns + "enumerator"))
                    {
                        enumeratedData.AddEnumerator(new EnumeratedItem
                        {
                            Name = enumeratorElement.Element(ns + "name")?.Value,
                            Value = enumeratorElement.Element(ns + "value")?.Value
                        });
                    }

                    dataTypeSection.AddEnumeratedData(enumeratedData);
                }
            }

            // Read array data
            var arrayDataSection = elements.Elements(ns + "arrayDataTypes").ToArray();
            if (arrayDataSection.Length > 1)
            {
                throw new FomReaderException("The xml document has multiple Array Data Type sections");
            }

            if (arrayDataSection.Length == 1)
            {
                foreach (var element in arrayDataSection.Elements())
                {
                    if (!"arrayData".Equals(element.Name.LocalName))
                    {
                        throw new FomReaderException("Unknown element in the Array data section");
                    }

                    dataTypeSection.AddArrayData(new ArrayData
                    {
                        Name = element.Element(ns + "name")?.Value,
                        Encoding = element.Element(ns + "encoding")?.Value,
                        DataType = element.Element(ns + "dataType")?.Value,
                        Cardinality = element.Element(ns + "cardinality")?.Value,
                        Semantics = element.Element(ns + "semantics")?.Value
                    });
                }
            }

            // Read fixed record data
            var fixedDataSection = elements.Elements(ns + "fixedRecordDataTypes").ToArray();
            if (fixedDataSection.Length > 1)
            {
                throw new FomReaderException("The xml document has multiple Fixed Record Data Type sections");
            }

            if (fixedDataSection.Length == 1)
            {
                foreach (var element in fixedDataSection.Elements())
                {
                    if (!"fixedRecordData".Equals(element.Name.LocalName))
                    {
                        throw new FomReaderException("Unknown element in the fixed record data section");
                    }

                    var fixedRecordData = new FixedRecordData
                    {
                        Name = element.Element(ns + "name")?.Value,
                        Encoding = element.Element(ns + "encoding")?.Value,
                        Semantics = element.Element(ns + "semantics")?.Value
                    };

                    foreach (var fieldElement in element.Elements(ns + "field"))
                    {
                        fixedRecordData.AddField(new FixedRecordField
                        {
                            Name = fieldElement.Element(ns + "name")?.Value,
                            DataType = fieldElement.Element(ns + "dataType")?.Value,
                            Semantics = fieldElement.Element(ns + "semantics")?.Value
                        });
                    }

                    dataTypeSection.AddFixedRecordData(fixedRecordData);
                }
            }

            // Read variant record data
            var variantDataSection = elements.Elements(ns + "variantRecordDataTypes").ToArray();
            if (variantDataSection.Length > 1)
            {
                throw new FomReaderException("The xml document has multiple Variant Record Data Type sections");
            }

            if (variantDataSection.Length == 1)
            {
                foreach (var element in variantDataSection.Elements())
                {
                    if (!"variantRecordData".Equals(element.Name.LocalName))
                    {
                        throw new FomReaderException("Unknown element in the variant record data section");
                    }

                    var variantRecordData = new VariantRecordData
                    {
                        Name = element.Element(ns + "name")?.Value,
                        Discriminant = element.Element(ns + "discriminant")?.Value,
                        DataType = element.Element(ns + "dataType")?.Value,
                        Encoding = element.Element(ns + "encoding")?.Value,
                        Semantics = element.Element(ns + "semantics")?.Value
                    };

                    foreach (var alternativeElement in element.Elements(ns + "alternative"))
                    {
                        variantRecordData.AddAlternative(new AlternativeItem
                        {
                            Enumerator = alternativeElement.Element(ns + "enumerator")?.Value,
                            Name = alternativeElement.Element(ns + "name")?.Value,
                            DataType = alternativeElement.Element(ns + "dataType")?.Value,
                            Semantics = alternativeElement.Element(ns + "semantics")?.Value
                        });
                    }

                    dataTypeSection.AddVariantRecordData(variantRecordData);
                }
            }

            return dataTypeSection;
        }
    }
}
