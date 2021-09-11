/*
 *   Copyright 2021 Simusharp
 *   Don't remove this header
 *   Distributed under the MIT License.
 */

using System.Collections.Generic;

namespace Simusharp.FomGen.Core.Models
{
    public class PointOfContact
    {
        public string Type { get; set; }

        public string Name { get; set; } = null;

        public string Organization { get; set; } = null;

        public IList<string> Phones { get; protected set; } = new List<string>();

        public IList<string> Emails { get; protected set; } = new List<string>();
    }
}
