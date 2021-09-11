/*
 *   Copyright 2021 Simusharp
 *   Don't remove this header
 *   Distributed under the MIT License.
 */

using System;
using System.Collections.Generic;
using Simusharp.FomGen.Core.Models;

namespace Simusharp.FomGen.Core.Util
{
    public class TreeNode<T> where T : IName
    {
        private readonly List<TreeNode<T>> _children = new List<TreeNode<T>>();

        public TreeNode(T value)
        {
            Value = value;
        }

        public TreeNode<T> Parent { get; set; }

        public T Value { get; }

        public IReadOnlyList<TreeNode<T>> Children => this._children;

        public void Add(TreeNode<T> child)
        {
            child.Parent = this;
            _children.Add(child);
        }

        public void Remove(TreeNode<T> child)
        {
            if (child.Parent.Equals(this))
            {
                child.Parent = null;
                _children.Remove(child);
            }
        }

        public string GetQualifiedName()
        {
            var name = Value.Name;
            if (Parent != null)
            {
                name = $"{Parent.GetQualifiedName()}.{name}";
            }

            return name;
        }

        public TreeNode<T> Find(Predicate<TreeNode<T>> p)
        {
            if (p(this))
            {
                return this;
            }

            var v = _children.Find(p);
            if (v != null)
            {
                return v;
            }

            foreach (var child in _children)
            {
                var childValue = child.Find(p);
                if (childValue != null)
                {
                    return childValue;
                }
            }

            return null;
        }

        /// <summary>
        /// Get the number of nodes including this one
        /// </summary>
        public int Count
        {
            get
            {
                var i = 1;
                foreach (var child in _children)
                {
                    i += child.Count;
                }

                return i;
            }
        }
    }
}
