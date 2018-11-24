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

    public abstract class MethodMockBase<TParam, TResult> : MemberMock, IMethodStepCaller<TParam, TResult>
    {
        private IMethodStep<TParam, TResult> _nextStep = MissingMethodStep<TParam, TResult>.Instance;

        protected internal MethodMockBase(object mockInstance, string mocklisClassName, string interfaceName, string memberName,
            string memberMockName)
            : base(mockInstance, mocklisClassName, interfaceName, memberName, memberMockName)
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
            return _nextStep.Call(this, param);
        }
    }
}
