// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FuncMethodMock_Call_should.cs">
//   Copyright © 2018 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.Core.Tests.Core
{
    #region Using Directives

    using System;
    using Mocklis.Core.Tests.Mocks;
    using Xunit;

    #endregion

    public class FuncMethodMock_Call_should
    {
        private readonly FuncMethodMock<string> _parameterLessFuncMock;
        private readonly FuncMethodMock<int, string> _funcMock;

        public FuncMethodMock_Call_should()
        {
            _parameterLessFuncMock = new FuncMethodMock<string>(new object(), "ClassName", "InterfaceName", "MemberName", "MockName");
            _funcMock = new FuncMethodMock<int, string>(new object(), "ClassName", "InterfaceName", "MemberName", "MockName");
        }

        [Fact]
        public void send_mock_information_to_step_and_get_result_back()
        {
            IMockInfo sentMockInfo = null;

            var newStep = new MockMethodStep<ValueTuple, string>();
            newStep.Call.Func(p =>
            {
                sentMockInfo = p.mockInfo;
                return "5";
            });
            ((IMethodStepCaller<ValueTuple, string>)_parameterLessFuncMock).SetNextStep(newStep);

            _parameterLessFuncMock.Call();

            Assert.Same(_parameterLessFuncMock, sentMockInfo);
        }

        [Fact]
        public void send_mock_information_and_parameters_to_step_and_get_result_back()
        {
            IMockInfo sentMockInfo = null;
            int sentParam = 0;

            var newStep = new MockMethodStep<int, string>();
            newStep.Call.Func(p =>
            {
                sentMockInfo = p.mockInfo;
                sentParam = p.param;
                return "5";
            });
            ((IMethodStepCaller<int, string>)_funcMock).SetNextStep(newStep);

            string result = _funcMock.Call(5);

            Assert.Same(_funcMock, sentMockInfo);
            Assert.Equal(5, sentParam);
            Assert.Equal("5", result);
        }
    }
}