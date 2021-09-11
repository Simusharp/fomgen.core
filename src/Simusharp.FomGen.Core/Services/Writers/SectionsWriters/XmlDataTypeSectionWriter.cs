/*
 *   Copyright 2021 Simusharp
 *   Don't remove this header
 *   Distributed under the MIT License.
 */

using Simusharp.FomGen.Core.Models;
using System;
using System.Xml.Linq;

namespace Simusharp.FomGen.Core.Services.Writers.SectionsWriters
{
    internal class XmlDataTypeSectionWriter : XmlSectionWriterBase, IXmlFomSectionWriter
    {
        public XDocument WriteFomSection(FomSection fomSection, XDocument xDocument)
        {
            if (fomSection == null)
            {
                return xDocument;
            }

            var parentElement = GetParentElement(fomSection, xDocument);

            if (fomSection is not DataTypeSection dataTypeSection)
            {
                throw new FomWriterException("Error casting to Data Type section");
            }

            var ns = xDocument.Root?.Name.Namespace;
            var dataTypeElement = new XElement(ns + dataTypeSection.SectionName);
            var shouldAdd = false;

            // Basic data
            if (dataTypeSection.BasicData.Count > 0)
            {
                shouldAdd = true;
                var basicElement = new XElement(ns + "basicDataRepresentations");
                foreach (var basicData in dataTypeSection.BasicData)
                {
                    var element = new XElement(ns + "basicData",
                        new XElement(ns + "name", basicData.Name));
                    if (basicData.Size.HasValue)
                    {
                        element.Add(new XElement(ns + "size", basicData.Size));
                    }

                    if (!string.IsNullOrWhiteSpace(basicData.Interpretation))
                    {
                        element.Add(new XElement(ns + "interpretation", basicData.Interpretation));
                    }

                    if (!string.IsNullOrWhiteSpace(basicData.Endian))
                    {
                        element.Add(new XElement(ns + "endian", basicData.Endian));
                    }

                    if (!string.IsNullOrWhiteSpace(basicData.Encoding))
                    {
                        element.Add(new XElement(ns + "encoding", basicData.Encoding));
                    }

                    basicElement.Add(element);
                }

                dataTypeElement.Add(basicElement);
            }

            // Simple data
            if (dataTypeSection.SimpleData.Count > 0)
            {
                shouldAdd = true;
                var simpleElement = new XElement(ns + "simpleDataTypes");
                foreach (var simpleData in dataTypeSection.SimpleData)
                {
                    var element = new XElement(ns + "simpleData",
                        new XElement(ns + "name", simpleData.Name));

                    if (!string.IsNullOrWhiteSpace(simpleData.Representation))
                    {
                        element.Add(new XElement(ns + "representation", simpleData.Representation));
                    }

                    if (!string.IsNullOrWhiteSpace(simpleData.Units))
                    {
                        element.Add(new XElement(ns + "units", simpleData.Units));
                    }

                    if (!string.IsNullOrWhiteSpace(simpleData.Resolution))
                    {
                        element.Add(new XElement(ns + "resolution", simpleData.Resolution));
                    }

                    if (!string.IsNullOrWhiteSpace(simpleData.Accuracy))
                    {
                        element.Add(new XElement(ns + "accuracy", simpleData.Accuracy));
                    }

                    if (!string.IsNullOrWhiteSpace(simpleData.Semantics))
                    {
                        element.Add(new XElement(ns + "semantics", simpleData.Semantics));
                    }

                    simpleElement.Add(element);
                }

                dataTypeElement.Add(simpleElement);
            }

            // Enumerated data
            if (dataTypeSection.EnumeratedData.Count > 0)
            {
                shouldAdd = true;
                var enumeratedDataTypeElement = new XElement(ns + "enumeratedDataTypes");
                foreach (var enumerated in dataTypeSection.EnumeratedData)
                {
                    var enumeratedElement = new XElement(ns + "enumeratedData",
                        new XElement(ns + "name", enumerated.Name));

                    if (!string.IsNullOrWhiteSpace(enumerated.Representation))
                    {
                        enumeratedElement.Add(new XElement(ns + "representation", enumerated.Representation));
                    }

                    if (!string.IsNullOrWhiteSpace(enumerated.Semantics))
                    {
                        enumeratedElement.Add(new XElement(ns + "semantics", enumerated.Semantics));
                    }

                    foreach (var enumeratedItem in enumerated.Enumerator)
                    {
                        var enumeratedItemElement = new XElement(ns + "enumerator",
                            new XElement(ns + "name", enumeratedItem.Name));
                        if (!string.IsNullOrWhiteSpace(enumeratedItem.Value))
                        {
                            enumeratedItemElement.Add(new XElement(ns + "value", enumeratedItem.Value));
                        }

                        enumeratedElement.Add(enumeratedItemElement);
                    }

                    enumeratedDataTypeElement.Add(enumeratedElement);
                }

                dataTypeElement.Add(enumeratedDataTypeElement);
            }

            // Array data
            if (dataTypeSection.ArrayData.Count > 0)
            {
                shouldAdd = true;
                var arrayDataTypeElement = new XElement(ns + "arrayDataTypes");
                foreach (var enumerated in dataTypeSection.ArrayData)
                {
                    var element = new XElement(ns + "arrayData",
                        new XElement(ns + "name", enumerated.Name));
                    if (!string.IsNullOrWhiteSpace(enumerated.DataType))
                    {
                        element.Add(new XElement(ns + "dataType", enumerated.DataType));
                    }

                    if (!string.IsNullOrWhiteSpace(enumerated.Cardinality))
                    {
                        element.Add(new XElement(ns + "cardinality", enumerated.Cardinality));
                    }

                    if (!string.IsNullOrWhiteSpace(enumerated.Encoding))
                    {
                        element.Add(new XElement(ns + "encoding", enumerated.Encoding));
                    }

                    if (!string.IsNullOrWhiteSpace(enumerated.Semantics))
                    {
                        element.Add(new XElement(ns + "semantics", enumerated.Semantics));
                    }

                    arrayDataTypeElement.Add(element);
                }

                dataTypeElement.Add(arrayDataTypeElement);
            }

            // Fixed record data
            if (dataTypeSection.FixedRecordData.Count > 0)
            {
                shouldAdd = true;
                var fixedRecordElement = new XElement(ns + "fixedRecordDataTypes");
                foreach (var fixedRecordData in dataTypeSection.FixedRecordData)
                {
                    var recordElement = new XElement(ns + "fixedRecordData",
                        new XElement(ns + "name", fixedRecordData.Name));
                    if (!string.IsNullOrWhiteSpace(fixedRecordData.Encoding))
                    {
                        recordElement.Add(new XElement(ns + "encoding", fixedRecordData.Encoding));
                    }

                    if (!string.IsNullOrWhiteSpace(fixedRecordData.Semantics))
                    {
                        recordElement.Add(new XElement(ns + "semantics", fixedRecordData.Semantics));
                    }

                    foreach (var field in fixedRecordData.Fields)
                    {
                        var element = new XElement(ns + "field",
                            new XElement(ns + "name", field.Name));
                        if (!string.IsNullOrWhiteSpace(field.DataType))
                        {
                            element.Add(new XElement(ns + "dataType", field.DataType));
                        }

                        if (!string.IsNullOrWhiteSpace(field.Semantics))
                        {
                            element.Add(new XElement(ns + "semantics", field.Semantics));
                        }

                        recordElement.Add(element);
                    }

                    fixedRecordElement.Add(recordElement);
                }

                dataTypeElement.Add(fixedRecordElement);
            }

            // Variant record data
            if (dataTypeSection.VariantRecordData.Count > 0)
            {
                shouldAdd = true;
                var variantDataTypeElement = new XElement(ns + "variantRecordDataTypes");
                foreach (var variantRecord in dataTypeSection.VariantRecordData)
                {
                    var variantElement = new XElement(ns + "variantRecordData",
                        new XElement(ns + "name", variantRecord.Name));
                    if (!string.IsNullOrWhiteSpace(variantRecord.Discriminant))
                    {
                        variantElement.Add(new XElement(ns + "discriminant", variantRecord.Discriminant));
                    }

                    if (!string.IsNullOrWhiteSpace(variantRecord.DataType))
                    {
                        variantElement.Add(new XElement(ns + "dataType", variantRecord.DataType));
                    }

                    foreach (var alternative in variantRecord.Alternatives)
                    {
                        var alternativeElement = new XElement(ns + "alternative");

                        if (!string.IsNullOrWhiteSpace(alternative.Enumerator))
                        {
                            alternativeElement.Add(new XElement(ns + "enumerator", alternative.Enumerator));
                        }

                        if (!string.IsNullOrWhiteSpace(alternative.Name))
                        {
                            alternativeElement.Add(new XElement(ns + "name", alternative.Name));
                        }

                        if (!string.IsNullOrWhiteSpace(alternative.DataType))
                        {
                            alternativeElement.Add(new XElement(ns + "dataType", alternative.DataType));
                        }

                        if (!string.IsNullOrWhiteSpace(alternative.Semantics))
                        {
                            alternativeElement.Add(new XElement(ns + "semantics", alternative.Semantics));
                        }

                        variantElement.Add(alternativeElement);
                    }

                    if (!string.IsNullOrWhiteSpace(variantRecord.Encoding))
                    {
                        variantElement.Add(new XElement(ns + "encoding", variantRecord.Encoding));
                    }

                    if (!string.IsNullOrWhiteSpace(variantRecord.Semantics))
                    {
                        variantElement.Add(new XElement(ns + "semantics", variantRecord.Semantics));
                    }

                    variantDataTypeElement.Add(variantElement);
                }

                dataTypeElement.Add(variantDataTypeElement);
            }

            if (shouldAdd)
            {
                parentElement.Add(dataTypeElement);
            }

            return xDocument;
        }
    }
}
