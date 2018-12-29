// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MethodMockBase.cs">
//   Copyright © 2018 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.Core
{
    #region Using Directives

    using System;
    using Mocklis.Steps.Missing;

    #endregion

    public abstract class MethodMockBase<TParam, TResult> : MemberMock, ICanHaveNextMethodStep<TParam, TResult>
    {
        private IMethodStep<TParam, TResult> _nextStep = MissingMethodStep<TParam, TResult>.Instance;

        protected internal MethodMockBase(object mockInstance, string mocklisClassName, string interfaceName, string memberName,
            string memberMockName)
            : base(mockInstance, mocklisClassName, interfaceName, memberName, memberMockName)
        {
        }

        TStep ICanHaveNextMethodStep<TParam, TResult>.SetNextStep<TStep>(TStep step)
        {
            if (step == null)
            {
                throw new ArgumentNullException(nameof(step));
            }

            _nextStep = step;
            return step;
        }

        protected TResult Call(TParam param)
        {
            return _nextStep.Call(this, param);
        }
    }
}
