// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IfMethodStep.cs">
//   Copyright © 2018 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.Conditional
{
    #region Using Directives

    using System;
    using Mocklis.Core;

    #endregion

    public class IfMethodStep<TParam, TResult> : MedialMethodStep<TParam, TResult>
    {
        private readonly Func<TParam, bool> _condition;
        private readonly MedialMethodStep<TParam, TResult> _branch = new MedialMethodStep<TParam, TResult>();

        public IfMethodStep(Func<TParam, bool> condition, Action<IMethodStepCaller<TParam, TResult>> branch)
        {
            _condition = condition;
            branch(_branch);
        }

        public override TResult Call(object instance, MemberMock memberMock, TParam param)
        {
            return _condition(param) ? _branch.Call(instance, memberMock, param) : base.Call(instance, memberMock, param);
        }
    }
}
