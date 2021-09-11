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
    public class TimeMerger : IMerge<TimeSection>
    {
        public TimeSection Merge(TimeSection[] sections)
        {
            if (sections == null)
            {
                throw new ArgumentNullException(nameof(sections));
            }

            // exclude null entries
            var realSections = sections.Where(x => x != null).ToArray();

            if (realSections.Length == 0)
            {
                return null;
            }

            if (realSections.Any(x => x.LookAhead != null))
            {
                if (realSections.Any(x => x.LookAhead == null))
                {
                    throw new FomMergerException("Some FOM modules contain lookahead and some don't", realSections[0].SectionName);
                }
                else
                {
                    for (var i = 1; i < realSections.Length; i++)
                    {
                        if (!realSections[i].LookAhead.DataType.Equals(realSections[0].LookAhead.DataType))
                        {
                            throw new FomMergerException("Lookahead data type doesn't match", realSections[0].SectionName);
                        }

                        if (!realSections[i].LookAhead.Semantics.Equals(realSections[0].LookAhead.Semantics))
                        {
                            throw new FomMergerException("Lookahead semantics doesn't match", realSections[0].SectionName);
                        }
                    }
                }
            }

            if (realSections.Any(x => x.TimeStamp != null))
            {
                if (realSections.Any(x => x.TimeStamp == null))
                {
                    throw new FomMergerException("Some FOM modules contain Time Stamp and some don't", realSections[0].SectionName);
                }
                else
                {
                    for (var i = 1; i < realSections.Length; i++)
                    {
                        if (!realSections[i].TimeStamp.DataType.Equals(realSections[0].TimeStamp.DataType))
                        {
                            throw new FomMergerException("Time Stamp data type doesn't match", realSections[0].SectionName);
                        }

                        if (!realSections[i].TimeStamp.Semantics.Equals(realSections[0].TimeStamp.Semantics))
                        {
                            throw new FomMergerException("Time Stamp semantics doesn't match", realSections[0].SectionName);
                        }
                    }
                }
            }

            return realSections[0];
        }
    }
}
