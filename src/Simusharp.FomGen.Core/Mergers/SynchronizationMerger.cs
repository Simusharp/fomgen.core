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
    public class SynchronizationMerger : IMerge<SynchronizationSection>
    {
        public SynchronizationSection Merge(SynchronizationSection[] sections)
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

            var synchronizationSection = new SynchronizationSection();

            foreach (var section in realSections)
            {
                foreach (var sync in section)
                {
                    if (synchronizationSection.All(x => x.Label != sync.Label))
                    {
                        synchronizationSection.Add(sync);
                    }
                    else
                    {
                        var duplicateTransportation =
                            synchronizationSection.First(x => x.Label == sync.Label);
                        if (!duplicateTransportation.Semantics.Equals(sync.Semantics) ||
                            !duplicateTransportation.Capability.Equals(sync.Capability) ||
                            !duplicateTransportation.TagDataType.Equals(sync.TagDataType))
                        {
                            throw new FomMergerException($"Class {sync.Label} is different between FOM modules", section.SectionName);
                        }
                    }
                }
            }

            return synchronizationSection;
        }
    }
}
