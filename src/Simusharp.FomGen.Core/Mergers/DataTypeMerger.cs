/*
 *   Copyright 2021 Simusharp
 *   Don't remove this header
 *   Distributed under the MIT License.
 */

using Simusharp.FomGen.Core.Models;
using System;
using System.Linq;

namespace Simusharp.FomGen.Core.Mergers
{
    public class DataTypeMerger : IMerge<DataTypeSection>
    {
        public DataTypeSection Merge(DataTypeSection[] sections)
        {
            if (sections == null)
            {
                throw new ArgumentNullException(nameof(sections));
            }

            // Exclude null entries
            var realSections = sections.Where(x => x != null).ToArray();

            if (realSections.Length == 0)
            {
                return null;
            }

            var mergeSection = new DataTypeSection();
            foreach (var section in realSections)
            {
                foreach (var data in section.BasicData)
                {
                    if (mergeSection.BasicData.All(x => x.Name != data.Name))
                    {
                        mergeSection.AddBasicData(data);
                    }
                    else
                    {
                        var duplicateData =
                            mergeSection.BasicData.First(x => x.Name == data.Name);
                        if (!duplicateData.Encoding.Equals(data.Encoding) ||
                            duplicateData.Size != data.Size ||
                            !duplicateData.Endian.Equals(data.Endian) ||
                            !duplicateData.Interpretation.Equals(data.Interpretation))
                        {
                            throw new FomMergerException($"Class {data.Name} is different between FOM modules", section.SectionName);
                        }
                    }
                }

                foreach (var data in section.SimpleData)
                {
                    if (mergeSection.SimpleData.All(x => x.Name != data.Name))
                    {
                        mergeSection.AddSimpleData(data);
                    }
                    else
                    {
                        var duplicateData =
                            mergeSection.SimpleData.First(x => x.Name == data.Name);
                        if (!duplicateData.Accuracy.Equals(data.Accuracy) ||
                            !duplicateData.Representation.Equals(data.Representation) ||
                            !duplicateData.Resolution.Equals(data.Resolution) ||
                            !duplicateData.Semantics.Equals(data.Semantics) ||
                            !duplicateData.Units.Equals(data.Units))
                        {
                            throw new FomMergerException($"Class {data.Name} is different between FOM modules", section.SectionName);
                        }
                    }
                }

                foreach (var data in section.ArrayData)
                {
                    if (mergeSection.ArrayData.All(x => x.Name != data.Name))
                    {
                        mergeSection.AddArrayData(data);
                    }
                    else
                    {
                        var duplicateData =
                            mergeSection.ArrayData.First(x => x.Name == data.Name);
                        if (!duplicateData.Cardinality.Equals(data.Cardinality) ||
                            !duplicateData.DataType.Equals(data.DataType) ||
                            !duplicateData.Encoding.Equals(data.Encoding) ||
                            !duplicateData.Semantics.Equals(data.Semantics))
                        {
                            throw new FomMergerException($"Class {data.Name} is different between FOM modules", section.SectionName);
                        }
                    }
                }

                foreach (var data in section.EnumeratedData)
                {
                    if (mergeSection.EnumeratedData.All(x => x.Name != data.Name))
                    {
                        mergeSection.AddEnumeratedData(data);
                    }
                    else
                    {
                        var duplicateData =
                            mergeSection.EnumeratedData.First(x => x.Name == data.Name);
                        if (!duplicateData.Representation.Equals(data.Representation) ||
                            !duplicateData.Semantics.Equals(data.Semantics))
                        {
                            throw new FomMergerException($"Class {data.Name} is different between FOM modules", section.SectionName);
                        }

                        foreach (var item in data.Enumerator)
                        {
                            var duplicateItem =
                                duplicateData.Enumerator.FirstOrDefault(x => x.Name.Equals(item.Name));
                            if (duplicateItem == null || !duplicateItem.Value.Equals(item.Value))
                            {
                                throw new FomMergerException($"Class {data.Name} is different between FOM modules", section.SectionName);
                            }
                        }
                    }
                }

                foreach (var data in section.FixedRecordData)
                {
                    if (mergeSection.FixedRecordData.All(x => x.Name != data.Name))
                    {
                        mergeSection.AddFixedRecordData(data);
                    }
                    else
                    {
                        var duplicateData =
                            mergeSection.FixedRecordData.First(x => x.Name == data.Name);
                        if (!duplicateData.Encoding.Equals(data.Encoding) ||
                            !duplicateData.Semantics.Equals(data.Semantics))
                        {
                            throw new FomMergerException($"Class {data.Name} is different between FOM modules", section.SectionName);
                        }

                        foreach (var field in data.Fields)
                        {
                            var duplicateItem =
                                duplicateData.Fields.FirstOrDefault(x => x.Name.Equals(field.Name));
                            if (duplicateItem == null ||
                                !duplicateItem.DataType.Equals(field.DataType) ||
                                !duplicateItem.Semantics.Equals(field.Semantics))
                            {
                                throw new FomMergerException($"Class {data.Name} is different between FOM modules", section.SectionName);
                            }
                        }
                    }
                }

                foreach (var data in section.VariantRecordData)
                {
                    if (mergeSection.VariantRecordData.All(x => x.Name != data.Name))
                    {
                        mergeSection.AddVariantRecordData(data);
                    }
                    else
                    {
                        var duplicateData =
                            mergeSection.VariantRecordData.First(x => x.Name == data.Name);
                        if (!duplicateData.Encoding.Equals(data.Encoding) ||
                            !duplicateData.DataType.Equals(data.DataType) ||
                            !duplicateData.Discriminant.Equals(data.Discriminant) ||
                            !duplicateData.Semantics.Equals(data.Semantics))
                        {
                            throw new FomMergerException($"Class {data.Name} is different between FOM modules", section.SectionName);
                        }

                        foreach (var item in data.Alternatives)
                        {
                            var duplicateItem =
                                duplicateData.Alternatives.FirstOrDefault(x => x.Name.Equals(item.Name));
                            if (duplicateItem == null ||
                                !duplicateItem.DataType.Equals(item.DataType) ||
                                !duplicateItem.Enumerator.Equals(item.Enumerator) ||
                                !duplicateItem.Semantics.Equals(item.Semantics))
                            {
                                throw new FomMergerException($"Class {data.Name} is different between FOM modules", section.SectionName);
                            }
                        }
                    }
                }
            }

            return mergeSection;
        }
    }
}
