// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PropertyStepCaller.cs">
//   Copyright © 2018 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.StepCallerBaseClasses
{
    #region Using Directives

    using Mocklis.Core;

    #endregion

    public abstract class PropertyStepCaller<TValue> : IPropertyStepCaller<TValue>
    {
        public IPropertyStep<TValue> NextStep { get; private set; } = MissingPropertyStep<TValue>.Instance;

        public TImplementation SetNextStep<TImplementation>(TImplementation step) where TImplementation : IPropertyStep<TValue>
        {
            NextStep = step;
            return step;
        }
    }
}
