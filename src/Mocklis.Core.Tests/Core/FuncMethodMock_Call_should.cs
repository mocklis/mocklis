// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FuncMethodMock_Call_should.cs">
//   SPDX-License-Identifier: MIT
//   Copyright © 2019 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.Core.Tests.Core
{
    #region Using Directives

    using Mocklis.Core.Tests.Helpers;
    using Xunit;

    #endregion

    public class FuncMethodMock_Call_should
    {
        private static FakeNextMethodStep<TParam, TResult> NextStepFor<TParam, TResult>(ICanHaveNextMethodStep<TParam, TResult> mock, TResult result)
        {
            return new FakeNextMethodStep<TParam, TResult>(mock, result);
        }

        [Fact]
        public void send_mock_information_to_step_and_get_result_back()
        {
            var methodMock = new FuncMethodMock<string>(new object(), "ClassName", "InterfaceName", "MemberName", "MockName", Strictness.Lenient);
            var nextStep = NextStepFor(methodMock, "5");

            string result = methodMock.Call();

            Assert.Equal(1, nextStep.Count);
            Assert.Same(methodMock, nextStep.LastMockInfo);
            Assert.Equal("5", result);
        }

        [Fact]
        public void return_default_if_no_step_in_lenient_mode()
        {
            var methodMock = new FuncMethodMock<string>(new object(), "ClassName", "InterfaceName", "MemberName", "MockName", Strictness.Lenient);
            string result = methodMock.Call();
            Assert.Null(result);
        }

        [Fact]
        public void throw_if_no_step_in_strict_mode()
        {
            var methodMock = new FuncMethodMock<string>(new object(), "ClassName", "InterfaceName", "MemberName", "MockName", Strictness.Strict);
            var ex = Assert.Throws<MockMissingException>(() => methodMock.Call());
            Assert.Equal(MockType.Method, ex.MemberType);
        }

        [Fact]
        public void throw_if_no_step_in_very_strict_mode()
        {
            var methodMock = new FuncMethodMock<string>(new object(), "ClassName", "InterfaceName", "MemberName", "MockName", Strictness.VeryStrict);
            var ex = Assert.Throws<MockMissingException>(() => methodMock.Call());
            Assert.Equal(MockType.Method, ex.MemberType);
        }

        [Fact]
        public void return_default_if_cleared_in_lenient_mode()
        {
            var methodMock = new FuncMethodMock<string>(new object(), "ClassName", "InterfaceName", "MemberName", "MockName", Strictness.Lenient);
            var nextStep = NextStepFor(methodMock, "5");
            methodMock.Clear();
            string result = methodMock.Call();
            Assert.Equal(0, nextStep.Count);
            Assert.Null(result);
        }

        [Fact]
        public void throw_if_cleared_in_strict_mode()
        {
            var methodMock = new FuncMethodMock<string>(new object(), "ClassName", "InterfaceName", "MemberName", "MockName", Strictness.Strict);
            var nextStep = NextStepFor(methodMock, "5");
            methodMock.Clear();
            var ex = Assert.Throws<MockMissingException>(() => methodMock.Call());
            Assert.Equal(MockType.Method, ex.MemberType);
            Assert.Equal(0, nextStep.Count);
        }

        [Fact]
        public void throw_if_cleared_in_very_strict_mode()
        {
            var methodMock = new FuncMethodMock<string>(new object(), "ClassName", "InterfaceName", "MemberName", "MockName", Strictness.VeryStrict);
            var nextStep = NextStepFor(methodMock, "5");
            methodMock.Clear();
            var ex = Assert.Throws<MockMissingException>(() => methodMock.Call());
            Assert.Equal(MockType.Method, ex.MemberType);
            Assert.Equal(0, nextStep.Count);
        }

        [Fact]
        public void send_mock_information_and_parameters_to_step_and_get_result_back()
        {
            var methodMock =
                new FuncMethodMock<int, string>(new object(), "ClassName", "InterfaceName", "MemberName", "MockName", Strictness.Lenient);
            var nextStep = NextStepFor(methodMock, "5");

            string result = methodMock.Call(5);

            Assert.Equal(1, nextStep.Count);
            Assert.Same(methodMock, nextStep.LastMockInfo);
            Assert.Equal(5, nextStep.LastParam);
            Assert.Equal("5", result);
        }

        [Fact]
        public void return_default_if_no_step_in_lenient_mode_parameter_case()
        {
            var methodMock =
                new FuncMethodMock<int, string>(new object(), "ClassName", "InterfaceName", "MemberName", "MockName", Strictness.Lenient);
            string result = methodMock.Call(5);
            Assert.Null(result);
        }

        [Fact]
        public void throw_if_no_step_in_strict_mode_parameter_case()
        {
            var methodMock = new FuncMethodMock<int, string>(new object(), "ClassName", "InterfaceName", "MemberName", "MockName", Strictness.Strict);
            var ex = Assert.Throws<MockMissingException>(() => methodMock.Call(5));
            Assert.Equal(MockType.Method, ex.MemberType);
        }

        [Fact]
        public void throw_if_no_step_in_very_strict_mode_parameter_case()
        {
            var methodMock =
                new FuncMethodMock<int, string>(new object(), "ClassName", "InterfaceName", "MemberName", "MockName", Strictness.VeryStrict);
            var ex = Assert.Throws<MockMissingException>(() => methodMock.Call(5));
            Assert.Equal(MockType.Method, ex.MemberType);
        }

        [Fact]
        public void return_default_if_cleared_in_lenient_mode_parameter_case()
        {
            var methodMock =
                new FuncMethodMock<int, string>(new object(), "ClassName", "InterfaceName", "MemberName", "MockName", Strictness.Lenient);
            var nextStep = NextStepFor(methodMock, "5");
            methodMock.Clear();
            string result = methodMock.Call(5);
            Assert.Equal(0, nextStep.Count);
            Assert.Null(result);
        }

        [Fact]
        public void throw_if_cleared_in_strict_mode_parameter_case()
        {
            var methodMock = new FuncMethodMock<int, string>(new object(), "ClassName", "InterfaceName", "MemberName", "MockName", Strictness.Strict);
            var nextStep = NextStepFor(methodMock, "5");
            methodMock.Clear();
            var ex = Assert.Throws<MockMissingException>(() => methodMock.Call(5));
            Assert.Equal(MockType.Method, ex.MemberType);
            Assert.Equal(0, nextStep.Count);
        }

        [Fact]
        public void throw_if_cleared_in_very_strict_mode_parameter_case()
        {
            var methodMock =
                new FuncMethodMock<int, string>(new object(), "ClassName", "InterfaceName", "MemberName", "MockName", Strictness.VeryStrict);
            var nextStep = NextStepFor(methodMock, "5");
            methodMock.Clear();
            var ex = Assert.Throws<MockMissingException>(() => methodMock.Call(5));
            Assert.Equal(MockType.Method, ex.MemberType);
            Assert.Equal(0, nextStep.Count);
        }
    }
}
