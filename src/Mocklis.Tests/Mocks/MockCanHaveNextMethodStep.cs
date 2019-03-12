// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MockCanHaveNextMethodStep.cs">
//   SPDX-License-Identifier: MIT
//   Copyright © 2019 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.Tests.Mocks
{
    #region Using Directives

    using Mocklis.Core;

    #endregion

    [MocklisClass]
    public class MockCanHaveNextMethodStep<TParam, TResult> : ICanHaveNextMethodStep<TParam, TResult>
    {
        private readonly TypedMockProvider _setNextStep = new TypedMockProvider();

        public FuncMethodMock<TStep, TStep> SetNextStep<TStep>() where TStep : IMethodStep<TParam, TResult>
        {
            var key = new[] { typeof(TStep) };
            return (FuncMethodMock<TStep, TStep>)_setNextStep.GetOrAdd(key,
                keyString => new FuncMethodMock<TStep, TStep>(this, "MockCanHaveNextMethodStep", "ICanHaveNextMethodStep", "SetNextStep" + keyString,
                    "SetNextStep" + keyString + "()"));
        }

        TStep ICanHaveNextMethodStep<TParam, TResult>.SetNextStep<TStep>(TStep step) => SetNextStep<TStep>().Call(step);
    }
}
