/*
 *   Copyright 2021 Simusharp
 *   Don't remove this header
 *   Distributed under the MIT License.
 */

using Simusharp.FomGen.Core.Models;
using System;
using System.Linq;
using Simusharp.FomGen.Core.Util;

namespace Simusharp.FomGen.Core.Mergers
{
    public class ObjectClassMerger : IMerge<ObjectClassSection>
    {
        public ObjectClassSection Merge(ObjectClassSection[] sections)
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

            var mergeSection = realSections[0];
            foreach (var section in realSections.Skip(1))
            {
                MergeNode(mergeSection, section.Root);
            }

            return mergeSection;
        }

        private void MergeNode(ObjectClassSection section, TreeNode<ObjectClass> node)
        {
            var duplicateNode = section.Root.Find(x => x.GetQualifiedName().Equals(node.GetQualifiedName()));

            if (duplicateNode != null)
            {
                if (!node.Value.IsScaffold)
                {
                    if (duplicateNode.Value.IsScaffold)
                    {
                        // Replace duplicate with new node
                        duplicateNode.Value.Semantics = node.Value.Semantics;
                        duplicateNode.Value.Sharing = node.Value.Sharing;
                        foreach (var attribute in node.Value.Attributes)
                        {
                            duplicateNode.Value.AddAttribute(attribute);
                        }
                    }
                    else
                    {
                        // they must match
                        if (!duplicateNode.Value.IsMatch(node.Value))
                        {
                            throw new FomMergerException($"Class {node.Value.Name} is different between FOM modules", section.SectionName);
                        }
                    }
                }
            }
            else
            {
                if (node.Parent == null)
                {
                    throw new FomMergerException($"Class {node.Value.Name} is a top parent but doesn't match parent from other FOM modules", section.SectionName);
                }
                else
                {
                    var parent = section.Root.Find(x => x.GetQualifiedName().Equals(node.Parent.GetQualifiedName()));
                    if (parent == null)
                    {
                        throw new FomMergerException($"The parent of class {node.Value.Name} can't be found", section.SectionName);
                    }
                    else
                    {
                        parent.Add(node);
                    }
                }
            }

            foreach (var child in node.Children)
            {
                MergeNode(section, child);
            }
        }
    }
}
