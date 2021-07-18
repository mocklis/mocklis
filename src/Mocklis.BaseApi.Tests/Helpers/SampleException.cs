// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SampleException.cs">
//   SPDX-License-Identifier: MIT
//   Copyright © 2019-2021 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.Helpers
{
    #region Using Directives

    using System;

    #endregion

    public class SampleException : Exception
    {
        public object? Instance { get; }

        public SampleException(object? instance = null)
        {
            Instance = instance;
        }
    }

    public class SampleException<TPayload> : SampleException
    {
        public TPayload Payload { get; }

        public SampleException(TPayload payload, object? instance = null) : base(instance)
        {
            Payload = payload;
        }
    }
}
