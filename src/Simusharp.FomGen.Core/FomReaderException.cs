/*
 *   Copyright 2021 Simusharp
 *   Don't remove this header
 *   Distributed under the MIT License.
 */

using System;

namespace Simusharp.FomGen.Core
{
    public class FomReaderException : Exception
    {
        public FomReaderException()
        {
        }

        public FomReaderException(string message)
            : base(message)
        {
        }

        public FomReaderException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
