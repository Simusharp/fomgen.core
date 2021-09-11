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
    public class DimensionMerger : IMerge<DimensionSection>
    {
        public DimensionSection Merge(DimensionSection[] sections)
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

            var dimensionSection = new DimensionSection();

            foreach (var section in realSections)
            {
                foreach (var dim in section)
                {
                    if (dimensionSection.All(x => x.Name != dim.Name))
                    {
                        dimensionSection.Add(dim);
                    }
                    else
                    {
                        var duplicateDim =
                            dimensionSection.First(x => x.Name == dim.Name);
                        if (!duplicateDim.DataType.Equals(dim.DataType) ||
                            duplicateDim.UpperBound != dim.UpperBound ||
                            !duplicateDim.NormalizationFunction.Equals(dim.NormalizationFunction) ||
                            !duplicateDim.ValueWhenUnspecified.Equals(dim.ValueWhenUnspecified))
                        {
                            throw new FomMergerException($"Class {dim.Name} is different between FOM modules", section.SectionName);
                        }
                    }
                }
            }

            return dimensionSection;
        }
    }
}
