/*
 *   Copyright 2021 Simusharp
 *   Don't remove this header
 *   Distributed under the MIT License.
 */

using System.Collections.Generic;
using System.Linq;

namespace Simusharp.FomGen.Core.Models
{
    public class InteractionClass : IName
    {
        private readonly List<ParameterItem> _parameters = new List<ParameterItem>();
        private readonly List<string> _dimensions = new List<string>();

        public string Name { get; set; }

        public string Sharing { get; set; }

        public string Transportation { get; set; }

        public string Order { get; set; }

        public string Semantics { get; set; }

        public IReadOnlyList<string> Dimensions => this._dimensions;

        public IReadOnlyList<ParameterItem> Parameters => this._parameters;

        public void AddDimension(string dimension)
        {
            this._dimensions.Add(dimension);
        }

        public void RemoveDimension(string dimension)
        {
            this._dimensions.Remove(dimension);
        }

        public void AddParameter(ParameterItem parameter)
        {
            this._parameters.Add(parameter);
        }

        public void RemoveParameter(ParameterItem parameter)
        {
            this._parameters.Remove(parameter);
        }

        public bool IsScaffold => string.IsNullOrWhiteSpace(Sharing) && string.IsNullOrWhiteSpace(Semantics) &&
                                  string.IsNullOrWhiteSpace(Transportation) && string.IsNullOrWhiteSpace(Order) &&
                                  _parameters.Count == 0;

        public bool IsMatch(InteractionClass other)
        {
            var isMatch = string.Equals(Semantics, other.Semantics) && string.Equals(Sharing, other.Sharing) && _parameters.Count == other.Parameters.Count && _dimensions.Count == other.Dimensions.Count;
            if (isMatch)
            {
                foreach (var dimension in _dimensions)
                {
                    isMatch = other.Dimensions.Contains(dimension);

                    if (!isMatch)
                    {
                        break;
                    }
                }
            }

            if (isMatch)
            {
                foreach (var parameter in _parameters)
                {
                    var otherAttribute = other.Parameters.FirstOrDefault(x => x.Name.Equals(parameter.Name));
                    isMatch = parameter == otherAttribute;

                    if (!isMatch)
                    {
                        break;
                    }
                }
            }

            return isMatch;
        }
    }
}