// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MockCanHaveNextIndexerStep.cs">
//   SPDX-License-Identifier: MIT
//   Copyright © 2019-2020 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.Mocks
{
    #region Using Directives

    using Mocklis.Core;

    #endregion

    [MocklisClass]
    public class MockCanHaveNextIndexerStep<TKey, TValue> : ICanHaveNextIndexerStep<TKey, TValue>
    {
        // The contents of this class were created by the Mocklis code-generator.
        // Any changes you make will be overwritten if the contents are re-generated.

        private readonly TypedMockProvider _setNextStep = new TypedMockProvider();

        public FuncMethodMock<TStep, TStep> SetNextStep<TStep>() where TStep : IIndexerStep<TKey, TValue>
        {
            var key = new[] { typeof(TStep) };
            return (FuncMethodMock<TStep, TStep>)_setNextStep.GetOrAdd(key, keyString => new FuncMethodMock<TStep, TStep>(this, "MockCanHaveNextIndexerStep", "ICanHaveNextIndexerStep", "SetNextStep" + keyString, "SetNextStep" + keyString + "()", Strictness.Lenient));
        }

        TStep ICanHaveNextIndexerStep<TKey, TValue>.SetNextStep<TStep>(TStep step) => SetNextStep<TStep>().Call(step);
    }
}
