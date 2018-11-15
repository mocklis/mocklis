// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BaseMethodMock.cs">
//   Copyright © 2018 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.Core
{
    #region Using Directives

    using System;

    #endregion

    public abstract class BaseMethodMock<TParam, TResult> : MemberMock, IMethodStepCaller<TParam, TResult>
    {
        private IMethodStep<TParam, TResult> _nextStep = MissingMethodStep<TParam, TResult>.Instance;

        protected internal BaseMethodMock(object mockInstance, string interfaceName, string memberName, string memberMockName)
            : base(mockInstance, interfaceName, memberName, memberMockName)
        {
        }

        public TStep SetNextStep<TStep>(TStep step) where TStep : IMethodStep<TParam, TResult>
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
            return _nextStep.Call(MockInstance, this, param);
        }
    }
}
