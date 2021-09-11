/*
 *   Copyright 2021 Simusharp
 *   Don't remove this header
 *   Distributed under the MIT License.
 */

using System;

namespace Simusharp.FomGen.Core
{
    public class FomMergerException : Exception
    {
        public FomMergerException()
        {
        }

        public FomMergerException(string message, string sectionName)
            : base(message)
        {
            SectionName = sectionName;
        }

        public FomMergerException(string message, Exception inner, string sectionName)
            : base(message, inner)
        {
            SectionName = sectionName;
        }

        public string SectionName { get; }
    }
}
