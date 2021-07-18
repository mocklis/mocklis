// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MockCanHaveNextEventStep.cs">
//   SPDX-License-Identifier: MIT
//   Copyright © 2019-2021 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.Mocks
{
    #region Using Directives

    using System;
    using System.CodeDom.Compiler;
    using Mocklis.Core;

    #endregion

    [MocklisClass] [GeneratedCode("Mocklis", "1.2.0")]
    public class MockCanHaveNextEventStep<THandler> : ICanHaveNextEventStep<THandler> where THandler : Delegate
    {
        // The contents of this class were created by the Mocklis code-generator.
        // Any changes you make will be overwritten if the contents are re-generated.

        private readonly TypedMockProvider _setNextStep = new TypedMockProvider();

        public FuncMethodMock<TStep, TStep> SetNextStep<TStep>() where TStep : IEventStep<THandler>
        {
            var key = new[] { typeof(TStep) };
            return (FuncMethodMock<TStep, TStep>)_setNextStep.GetOrAdd(key, keyString => new FuncMethodMock<TStep, TStep>(this, "MockCanHaveNextEventStep", "ICanHaveNextEventStep", "SetNextStep" + keyString, "SetNextStep" + keyString + "()", Strictness.Lenient));
        }

        TStep ICanHaveNextEventStep<THandler>.SetNextStep<TStep>(TStep step) => SetNextStep<TStep>().Call(step);
    }
}
