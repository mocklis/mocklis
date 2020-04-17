// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MockCanHaveNextMethodStep.cs">
//   SPDX-License-Identifier: MIT
//   Copyright © 2019-2020 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.Mocks
{
    #region Using Directives

    using System.CodeDom.Compiler;
    using Mocklis.Core;

    #endregion

    [MocklisClass, GeneratedCode("Mocklis", "1.2.0")]
    public class MockCanHaveNextMethodStep<TParam, TResult> : ICanHaveNextMethodStep<TParam, TResult>
    {
        // The contents of this class were created by the Mocklis code-generator.
        // Any changes you make will be overwritten if the contents are re-generated.

        private readonly TypedMockProvider _setNextStep = new TypedMockProvider();

        public FuncMethodMock<TStep, TStep> SetNextStep<TStep>() where TStep : IMethodStep<TParam, TResult>
        {
            var key = new[] { typeof(TStep) };
            return (FuncMethodMock<TStep, TStep>)_setNextStep.GetOrAdd(key, keyString => new FuncMethodMock<TStep, TStep>(this, "MockCanHaveNextMethodStep", "ICanHaveNextMethodStep", "SetNextStep" + keyString, "SetNextStep" + keyString + "()", Strictness.Lenient));
        }

        TStep ICanHaveNextMethodStep<TParam, TResult>.SetNextStep<TStep>(TStep step) => SetNextStep<TStep>().Call(step);
    }
}
