// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EventStepCaller.cs">
//   Copyright © 2018 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.StepCallerBaseClasses
{
    #region Using Directives

    using System;
    using Mocklis.Core;

    #endregion

    public abstract class EventStepCaller<THandler> : IEventStepCaller<THandler> where THandler : Delegate
    {
        public IEventStep<THandler> NextStep { get; private set; } = MissingEventStep<THandler>.Instance;

        public TImplementation SetNextStep<TImplementation>(TImplementation step) where TImplementation : IEventStep<THandler>
        {
            NextStep = step;
            return step;
        }
    }
}
