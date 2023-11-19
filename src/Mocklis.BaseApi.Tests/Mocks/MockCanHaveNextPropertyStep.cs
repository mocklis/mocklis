// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MockCanHaveNextPropertyStep.cs">
//   SPDX-License-Identifier: MIT
//   Copyright © 2019-2023 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.Mocks
{
    #region Using Directives

    using System.CodeDom.Compiler;
    using Mocklis.Core;

    #endregion

    [MocklisClass] [GeneratedCode("Mocklis", "1.2.0")]
    public class MockCanHaveNextPropertyStep<TValue> : ICanHaveNextPropertyStep<TValue>
    {
        // The contents of this class were created by the Mocklis code-generator.
        // Any changes you make will be overwritten if the contents are re-generated.

        private readonly TypedMockProvider _setNextStep = new TypedMockProvider();

        public FuncMethodMock<TStep, TStep> SetNextStep<TStep>() where TStep : IPropertyStep<TValue>
        {
            var key = new[] { typeof(TStep) };
            return (FuncMethodMock<TStep, TStep>)_setNextStep.GetOrAdd(key, keyString => new FuncMethodMock<TStep, TStep>(this, "MockCanHaveNextPropertyStep", "ICanHaveNextPropertyStep", "SetNextStep" + keyString, "SetNextStep" + keyString + "()", Strictness.Lenient));
        }

        TStep ICanHaveNextPropertyStep<TValue>.SetNextStep<TStep>(TStep step) => SetNextStep<TStep>().Call(step);
    }
}
