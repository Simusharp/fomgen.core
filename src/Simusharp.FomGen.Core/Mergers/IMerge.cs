/*
 *   Copyright 2021 Simusharp
 *   Don't remove this header
 *   Distributed under the MIT License.
 */

namespace Simusharp.FomGen.Core.Mergers
{
    /// <summary>
    /// This interface captures the merge behavior
    /// </summary>
    /// <typeparam name="T">Type to be merged</typeparam>
    public interface IMerge<T>
    {
        /// <summary>
        /// Merge this object with other
        /// </summary>
        /// <param name="sections">An array of sections to be merged</param>
        /// <returns>
        /// The result of the merge
        /// </returns>
        T Merge(T[] sections);
    }
}
