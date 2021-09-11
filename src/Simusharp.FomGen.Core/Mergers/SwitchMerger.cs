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
    /// <inheritdoc />
    /// <summary>
    /// Merge switches section
    /// </summary>
    public class SwitchMerger : IMerge<SwitchSection>
    {
        /// <inheritdoc cref="Merge"/>
        public SwitchSection Merge(SwitchSection[] sections)
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

            for (var i = 1; i < realSections.Length; i++)
            {
                if (realSections[i].AutoProvide != realSections[0].AutoProvide)
                {
                    throw new FomMergerException("Switch AutoProvide is not matched in all sections", realSections[0].SectionName);
                }

                if (realSections[i].ConveyRegionDesignatorSets != realSections[0].ConveyRegionDesignatorSets)
                {
                    throw new FomMergerException("Switch ConveyRegionDesignatorSets is not matched in all sections", realSections[0].SectionName);
                }

                if (realSections[i].ConveyProducingFederate != realSections[0].ConveyProducingFederate)
                {
                    throw new FomMergerException("Switch ConveyProducingFederate is not matched in all sections", realSections[0].SectionName);
                }

                if (realSections[i].AttributeScopeAdvisory != realSections[0].AttributeScopeAdvisory)
                {
                    throw new FomMergerException("Switch AttributeScopeAdvisory is not matched in all sections", realSections[0].SectionName);
                }

                if (realSections[i].AttributeRelevanceAdvisory != realSections[0].AttributeRelevanceAdvisory)
                {
                    throw new FomMergerException("Switch AttributeRelevanceAdvisory is not matched in all sections", realSections[0].SectionName);
                }

                if (realSections[i].ObjectClassRelevanceAdvisory != realSections[0].ObjectClassRelevanceAdvisory)
                {
                    throw new FomMergerException("Switch ObjectClassRelevanceAdvisory is not matched in all sections", realSections[0].SectionName);
                }

                if (realSections[i].InteractionRelevanceAdvisory != realSections[0].InteractionRelevanceAdvisory)
                {
                    throw new FomMergerException("Switch InteractionRelevanceAdvisory is not matched in all sections", realSections[0].SectionName);
                }

                if (realSections[i].ServiceReporting != realSections[0].ServiceReporting)
                {
                    throw new FomMergerException("Switch ServiceReporting is not matched in all sections", realSections[0].SectionName);
                }

                if (realSections[i].ExceptionReporting != realSections[0].ExceptionReporting)
                {
                    throw new FomMergerException("Switch ExceptionReporting is not matched in all sections", realSections[0].SectionName);
                }

                if (realSections[i].DelaySubscriptionEvaluation != realSections[0].DelaySubscriptionEvaluation)
                {
                    throw new FomMergerException("Switch DelaySubscriptionEvaluation is not matched in all sections", realSections[0].SectionName);
                }

                if (realSections[i].AutomaticResignSwitch != realSections[0].AutomaticResignSwitch)
                {
                    throw new FomMergerException("Switch AutomaticResignSwitch is not matched in all sections", realSections[0].SectionName);
                }
            }

            return realSections[0];
        }
    }
}
