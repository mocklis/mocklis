// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MockCanHaveNextIndexerStep.cs">
//   Copyright © 2019 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.Tests.Mocks
{
    #region Using Directives

    using Mocklis.Core;

    #endregion

    [MocklisClass]
    public class MockCanHaveNextIndexerStep<TKey, TValue> : ICanHaveNextIndexerStep<TKey, TValue>
    {
        private readonly TypedMockProvider _setNextStep = new TypedMockProvider();

        public FuncMethodMock<TStep, TStep> SetNextStep<TStep>() where TStep : IIndexerStep<TKey, TValue>
        {
            var key = new[] { typeof(TStep) };
            return (FuncMethodMock<TStep, TStep>)_setNextStep.GetOrAdd(key,
                keyString => new FuncMethodMock<TStep, TStep>(this, "MockCanHaveNextIndexerStep", "ICanHaveNextIndexerStep",
                    "SetNextStep" + keyString, "SetNextStep" + keyString + "()"));
        }

        TStep ICanHaveNextIndexerStep<TKey, TValue>.SetNextStep<TStep>(TStep step) => SetNextStep<TStep>().Call(step);
    }
}
