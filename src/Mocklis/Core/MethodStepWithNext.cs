// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MethodStepWithNext.cs">
//   Copyright © 2018 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.Core
{
    #region Using Directives

    using System;
    using Mocklis.Steps.Missing;

    #endregion

    public class MethodStepWithNext<TParam, TResult> : IMethodStep<TParam, TResult>, ICanHaveNextMethodStep<TParam, TResult>
    {
        protected IMethodStep<TParam, TResult> NextStep { get; private set; } = MissingMethodStep<TParam, TResult>.Instance;

        TStep ICanHaveNextMethodStep<TParam, TResult>.SetNextStep<TStep>(TStep step)
        {
            if (step == null)
            {
                throw new ArgumentNullException(nameof(step));
            }

            NextStep = step;
            return step;
        }

        public virtual TResult Call(IMockInfo mockInfo, TParam param)
        {
            return NextStep.Call(mockInfo, param);
        }
    }
}
