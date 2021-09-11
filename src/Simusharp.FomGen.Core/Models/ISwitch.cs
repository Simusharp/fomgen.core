/*
 *   Copyright 2021 Simusharp
 *   Don't remove this header
 *   Distributed under the MIT License.
 */

namespace Simusharp.FomGen.Core.Models
{
    /// <summary>
    /// The Switch interface.
    /// </summary>
    public interface ISwitch
    {
        /// <summary>
        /// Gets or sets name of the switch
        /// </summary>
        string NodeName { get; set; }

        /// <summary>
        /// Gets attribute name of the switch
        /// </summary>
        string AttributeName { get; }

        /// <summary>
        /// Gets or sets the value stored in the switch
        /// </summary>
        string AttributeValue { get; set; }

        /// <summary>
        /// Gets allowed values for the switch
        /// </summary>
        string[] AllowedValues { get; }
    }
}
