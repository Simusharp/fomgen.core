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
    public class ModelIdentificationMerger : IMerge<ModelIdentificationSection>
    {
        public ModelIdentificationSection Merge(ModelIdentificationSection[] sections)
        {
            if (sections == null)
            {
                throw new ArgumentNullException(nameof(sections));
            }

            var mergedSection = new ModelIdentificationSection();
            foreach (var section in sections.Where(x => x != null))
            {
                mergedSection.AddReference(new Reference
                {
                    Type = "Composed_From",
                    Identification = section.Name
                });
            }

            return mergedSection;
        }
    }
}
