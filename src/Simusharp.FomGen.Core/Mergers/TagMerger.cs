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
    public class TagMerger : IMerge<TagSection>
    {
        /// <inheritdoc cref="Merge"/>
        public TagSection Merge(TagSection[] sections)
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

            var count = realSections[0].Count;

            if (realSections.Any(x => x.Count != count))
            {
                throw new FomMergerException("Sections have different number of tags", realSections[0].SectionName);
            }

            foreach (var tag in realSections[0])
            {
                for (var i = 1; i < realSections.Length; i++)
                {
                    if (!realSections[i].Contains(tag))
                    {
                        throw new FomMergerException($"tag {tag} is not matched in all sections", realSections[0].SectionName);
                    }
                }
            }

            return realSections[0];
        }
    }
}
