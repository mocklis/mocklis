// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ActionMethodMockSetNextStepTests.cs">
//   SPDX-License-Identifier: MIT
//   Copyright © 2019-2024 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.Core
{
    #region Using Directives

    using System;
    using Mocklis.Mocks;
    using Xunit;

    #endregion

    public class ActionMethodMockSetNextStepTests
    {
        private readonly ActionMethodMock _parameterLessActionMock;
        private readonly ActionMethodMock<int> _actionMock;

        public ActionMethodMockSetNextStepTests()
        {
            _parameterLessActionMock = new ActionMethodMock(new object(), "ClassName", "InterfaceName", "MemberName", "MockName", Strictness.Lenient);
            _actionMock = new ActionMethodMock<int>(new object(), "ClassName", "InterfaceName", "MemberName", "MockName", Strictness.Lenient);
        }

        [Fact]
        public void RequireStep()
        {
            var exception = Assert.Throws<ArgumentNullException>(() =>
                ((ICanHaveNextMethodStep<int, ValueTuple>)_actionMock).SetNextStep((IMethodStep<int, ValueTuple>)null!));
            Assert.Equal("step", exception.ParamName);
        }

        [Fact]
        public void ReturnNewStep()
        {
            var newStep = new MockMethodStep<int, ValueTuple>();
            var returnedStep = ((ICanHaveNextMethodStep<int, ValueTuple>)_actionMock).SetNextStep(newStep);
            Assert.Same(newStep, returnedStep);
        }

        [Fact]
        public void SetStepUsedByCall()
        {
            bool called = false;
            var newStep = new MockMethodStep<ValueTuple, ValueTuple>();

            newStep.Call.Action(_ => { called = true; });
            ((ICanHaveNextMethodStep<ValueTuple, ValueTuple>)_parameterLessActionMock).SetNextStep(newStep);
            _parameterLessActionMock.Call();
            Assert.True(called);
        }

        [Fact]
        public void SetStepUsedByCallForParameterCase()
        {
            bool called = false;
            var newStep = new MockMethodStep<int, ValueTuple>();
            newStep.Call.Action(_ => called = true);
            ((ICanHaveNextMethodStep<int, ValueTuple>)_actionMock).SetNextStep(newStep);
            _actionMock.Call(5);
            Assert.True(called);
        }
    }
}
