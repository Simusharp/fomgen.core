/*
 *   Copyright 2021 Simusharp
 *   Don't remove this header
 *   Distributed under the MIT License.
 */

using System;

namespace Simusharp.FomGen.Core
{
    public class FomWriterException : Exception
    {
        public FomWriterException()
        {
        }

        public FomWriterException(string message)
            : base(message)
        {
        }

        public FomWriterException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
