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
    public class UpdateRateMerger : IMerge<UpdateRatesSection>
    {
        public UpdateRatesSection Merge(UpdateRatesSection[] sections)
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

            var sec = new UpdateRatesSection();

            foreach (var updateRate in realSections[0])
            {
                sec.Add(updateRate);
            }

            for (var i = 1; i < realSections.Length; i++)
            {
                foreach (var updateRate in realSections[i])
                {
                    var duplicate = sec.FirstOrDefault(u => u.Name == updateRate.Name);

                    if (duplicate == null)
                    {
                        sec.Add(updateRate);
                    }
                    else
                    {
                        if (duplicate.Rate != updateRate.Rate)
                        {
                            throw new FomMergerException($"Update rate {updateRate.Name} has different rates", realSections[0].SectionName);
                        }

                        if (duplicate.Semantics != updateRate.Semantics)
                        {
                            throw new FomMergerException($"Update rate {updateRate.Name} has different semantics", realSections[0].SectionName);
                        }
                    }
                }
            }

            return sec;
        }
    }
}
