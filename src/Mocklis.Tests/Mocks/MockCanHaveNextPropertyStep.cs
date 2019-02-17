// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MockCanHaveNextPropertyStep.cs">
//   Copyright © 2019 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.Tests.Mocks
{
    #region Using Directives

    using Mocklis.Core;

    #endregion

    [MocklisClass]
    public class MockCanHaveNextPropertyStep<TValue> : ICanHaveNextPropertyStep<TValue>
    {
        private readonly TypedMockProvider _setNextStep = new TypedMockProvider();

        public FuncMethodMock<TStep, TStep> SetNextStep<TStep>() where TStep : IPropertyStep<TValue>
        {
            var key = new[] { typeof(TStep) };
            return (FuncMethodMock<TStep, TStep>)_setNextStep.GetOrAdd(key,
                keyString => new FuncMethodMock<TStep, TStep>(this, "MockCanHaveNextPropertyStep", "ICanHaveNextPropertyStep",
                    "SetNextStep" + keyString, "SetNextStep" + keyString + "()"));
        }

        TStep ICanHaveNextPropertyStep<TValue>.SetNextStep<TStep>(TStep step) => SetNextStep<TStep>().Call(step);
    }
}
