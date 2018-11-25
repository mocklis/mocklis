// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IEventStep.cs">
//   Copyright © 2018 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.Core
{
    #region Using Directives

    using System;

    #endregion

    public interface IEventStep<in THandler> where THandler : Delegate
    {
        void Add(IMockInfo mockInfo, THandler value);
        void Remove(IMockInfo mockInfo, THandler value);
    }
}
