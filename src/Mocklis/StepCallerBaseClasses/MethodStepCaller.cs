// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MethodStepCaller.cs">
//   Copyright © 2018 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.StepCallerBaseClasses
{
    #region Using Directives

    using Mocklis.Core;

    #endregion

    public abstract class MethodStepCaller<TParam, TResult> : IMethodStepCaller<TParam, TResult>
    {
        public IMethodStep<TParam, TResult> NextStep { get; private set; } =
            MissingMethodStep<TParam, TResult>.Instance;

        public TImplementation SetNextStep<TImplementation>(TImplementation step) where TImplementation : IMethodStep<TParam, TResult>
        {
            NextStep = step;
            return step;
        }
    }
}
