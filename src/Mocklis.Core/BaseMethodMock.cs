// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BaseMethodMock.cs">
//   Copyright © 2018 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.Core
{
    public abstract class BaseMethodMock<TParam, TResult> : MemberMock, IMethodStepCaller<TParam, TResult>
    {
        public IMethodStep<TParam, TResult> NextStep { get; private set; } = MissingMethodStep<TParam, TResult>.Instance;

        protected internal BaseMethodMock(string interfaceName, string memberName, string memberMockName)
            : base(interfaceName, memberName, memberMockName)
        {
        }

        public TStep SetNextStep<TStep>(TStep step) where TStep : IMethodStep<TParam, TResult>
        {
            NextStep = step;
            return step;
        }

        protected TResult Call(object instance, MemberMock memberMock, TParam param)
        {
            return NextStep.Call(instance, memberMock, param);
        }
    }
}
