// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FuncMethodMockSetNextStepTests.cs">
//   SPDX-License-Identifier: MIT
//   Copyright © 2019-2023 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.Core
{
    #region Using Directives

    using System;
    using Mocklis.Mocks;
    using Xunit;

    #endregion

    public class FuncMethodMockSetNextStepTests
    {
        private readonly FuncMethodMock<string> _parameterLessFuncMock;
        private readonly FuncMethodMock<int, string> _funcMock;


        public FuncMethodMockSetNextStepTests()
        {
            _parameterLessFuncMock =
                new FuncMethodMock<string>(new object(), "ClassName", "InterfaceName", "MemberName", "MockName", Strictness.Lenient);
            _funcMock = new FuncMethodMock<int, string>(new object(), "ClassName", "InterfaceName", "MemberName", "MockName", Strictness.Lenient);
        }

        [Fact]
        public void RequireStep()
        {
            var exception = Assert.Throws<ArgumentNullException>(() =>
                ((ICanHaveNextMethodStep<int, string>)_funcMock).SetNextStep((IMethodStep<int, string>)null!));
            Assert.Equal("step", exception.ParamName);
        }

        [Fact]
        public void ReturnNewStep()
        {
            var newStep = new MockMethodStep<int, string>();
            var returnedStep = ((ICanHaveNextMethodStep<int, string>)_funcMock).SetNextStep(newStep);
            Assert.Same(newStep, returnedStep);
        }

        [Fact]
        public void SetStepUsedByCall()
        {
            bool called = false;
            var newStep = new MockMethodStep<ValueTuple, string>();

            newStep.Call.Func(_ =>
            {
                called = true;
                return string.Empty;
            });
            ((ICanHaveNextMethodStep<ValueTuple, string>)_parameterLessFuncMock).SetNextStep(newStep);
            _parameterLessFuncMock.Call();
            Assert.True(called);
        }

        [Fact]
        public void SetStepUsedByCallForParameterCase()
        {
            bool called = false;
            var newStep = new MockMethodStep<int, string>();
            newStep.Call.Func(_ =>
            {
                called = true;
                return string.Empty;
            });
            ((ICanHaveNextMethodStep<int, string>)_funcMock).SetNextStep(newStep);
            _funcMock.Call(5);
            Assert.True(called);
        }
    }
}
