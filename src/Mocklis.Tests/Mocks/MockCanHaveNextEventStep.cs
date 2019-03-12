// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MockCanHaveNextEventStep.cs">
//   SPDX-License-Identifier: MIT
//   Copyright © 2019 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.Tests.Mocks
{
    #region Using Directives

    using System;
    using Mocklis.Core;

    #endregion

    [MocklisClass]
    public class MockCanHaveNextEventStep<THandler> : ICanHaveNextEventStep<THandler> where THandler : Delegate
    {
        private readonly TypedMockProvider _setNextStep = new TypedMockProvider();

        public FuncMethodMock<TStep, TStep> SetNextStep<TStep>() where TStep : IEventStep<THandler>
        {
            var key = new[] { typeof(TStep) };
            return (FuncMethodMock<TStep, TStep>)_setNextStep.GetOrAdd(key,
                keyString => new FuncMethodMock<TStep, TStep>(this, "MockCanHaveNextEventStep", "ICanHaveNextEventStep", "SetNextStep" + keyString,
                    "SetNextStep" + keyString + "()"));
        }

        TStep ICanHaveNextEventStep<THandler>.SetNextStep<TStep>(TStep step) => SetNextStep<TStep>().Call(step);
    }
}
