// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MedialMethodStep.cs">
//   Copyright © 2018 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.Core
{
    #region Using Directives

    using System;
    using Mocklis.Steps.Missing;

    #endregion

    public class MedialMethodStep<TParam, TResult> : IMethodStep<TParam, TResult>, IMethodStepCaller<TParam, TResult>
    {
        protected IMethodStep<TParam, TResult> NextStep { get; private set; } = MissingMethodStep<TParam, TResult>.Instance;

        public TStep SetNextStep<TStep>(TStep step) where TStep : IMethodStep<TParam, TResult>
        {
            if (step == null)
            {
                throw new ArgumentNullException(nameof(step));
            }

            NextStep = step;
            return step;
        }

        public virtual TResult Call(object instance, MemberMock memberMock, TParam param)
        {
            return NextStep.Call(instance, memberMock, param);
        }
    }
}
