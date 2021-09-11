/*
 *   Copyright 2021 Simusharp
 *   Don't remove this header
 *   Distributed under the MIT License.
 */

using System;
using System.Linq;
using Simusharp.FomGen.Core.Models;

namespace Simusharp.FomGen.Core.Mergers
{
    public class TransportationMerger : IMerge<TransportationSection>
    {
        public TransportationSection Merge(TransportationSection[] sections)
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

            var transportationSection = new TransportationSection();

            foreach (var section in realSections)
            {
                foreach (var transportationType in section)
                {
                    if (transportationSection.All(x => x.Name != transportationType.Name))
                    {
                        transportationSection.Add(transportationType);
                    }
                    else
                    {
                        var duplicateTransportation =
                            transportationSection.First(x => x.Name == transportationType.Name);
                        if (!duplicateTransportation.Semantics.Equals(transportationType.Semantics))
                        {
                            throw new FomMergerException($"Class {transportationType.Name} is different between FOM modules", section.SectionName);
                        }
                    }
                }
            }

            return transportationSection;
        }
    }
}
