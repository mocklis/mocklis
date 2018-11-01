// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IEventStepCaller.cs">
//   Copyright © 2018 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.Core
{
    #region Using Directives

    using System;

    #endregion

    public interface IEventStepCaller<THandler> where THandler : Delegate
    {
        IEventStep<THandler> NextStep { get; }
        TStep SetNextStep<TStep>(TStep step) where TStep : IEventStep<THandler>;
    }
}
