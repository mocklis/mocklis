// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ActionMethodMock_Call_should.cs">
//   SPDX-License-Identifier: MIT
//   Copyright © 2019-2020 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.Core
{
    #region Using Directives

    using System;
    using Mocklis.Helpers;
    using Xunit;

    #endregion


    public class ActionMethodMock_Call_should
    {
        private static FakeNextMethodStep<TParam, ValueTuple> NextStepFor<TParam>(ICanHaveNextMethodStep<TParam, ValueTuple> mock)
        {
            return new FakeNextMethodStep<TParam, ValueTuple>(mock, ValueTuple.Create());
        }

        [Fact]
        public void send_mock_information_to_step()
        {
            var methodMock = new ActionMethodMock(new object(), "ClassName", "InterfaceName", "MemberName", "MockName", Strictness.Lenient);
            var nextStep = NextStepFor(methodMock);

            methodMock.Call();

            Assert.Equal(1, nextStep.Count);
            Assert.Same(methodMock, nextStep.LastMockInfo);
        }

        [Fact]
        public void do_nothing_if_no_step_in_lenient_mode()
        {
            var methodMock = new ActionMethodMock(new object(), "ClassName", "InterfaceName", "MemberName", "MockName", Strictness.Lenient);
            methodMock.Call();
        }

        [Fact]
        public void throw_if_no_step_in_strict_mode()
        {
            var methodMock = new ActionMethodMock(new object(), "ClassName", "InterfaceName", "MemberName", "MockName", Strictness.Strict);
            var ex = Assert.Throws<MockMissingException>(() => methodMock.Call());
            Assert.Equal(MockType.Method, ex.MemberType);
        }

        [Fact]
        public void throw_if_no_step_in_very_strict_mode()
        {
            var methodMock = new ActionMethodMock(new object(), "ClassName", "InterfaceName", "MemberName", "MockName", Strictness.VeryStrict);
            var ex = Assert.Throws<MockMissingException>(() => methodMock.Call());
            Assert.Equal(MockType.Method, ex.MemberType);
        }

        [Fact]
        public void do_nothing_if_cleared_in_lenient_mode()
        {
            var methodMock = new ActionMethodMock(new object(), "ClassName", "InterfaceName", "MemberName", "MockName", Strictness.Lenient);
            var nextStep = NextStepFor(methodMock);
            methodMock.Clear();
            methodMock.Call();
            Assert.Equal(0, nextStep.Count);
        }

        [Fact]
        public void throw_if_cleared_in_strict_mode()
        {
            var methodMock = new ActionMethodMock(new object(), "ClassName", "InterfaceName", "MemberName", "MockName", Strictness.Strict);
            var nextStep = NextStepFor(methodMock);
            methodMock.Clear();
            var ex = Assert.Throws<MockMissingException>(() => methodMock.Call());
            Assert.Equal(MockType.Method, ex.MemberType);
            Assert.Equal(0, nextStep.Count);
        }

        [Fact]
        public void throw_if_cleared_in_very_strict_mode()
        {
            var methodMock = new ActionMethodMock(new object(), "ClassName", "InterfaceName", "MemberName", "MockName", Strictness.VeryStrict);
            var nextStep = NextStepFor(methodMock);
            methodMock.Clear();
            var ex = Assert.Throws<MockMissingException>(() => methodMock.Call());
            Assert.Equal(MockType.Method, ex.MemberType);
            Assert.Equal(0, nextStep.Count);
        }


        [Fact]
        public void send_mock_information_and_parameters_to_step()
        {
            var actionMock = new ActionMethodMock<int>(new object(), "ClassName", "InterfaceName", "MemberName", "MockName", Strictness.Lenient);
            var nextStep = NextStepFor(actionMock);

            actionMock.Call(5);

            Assert.Equal(1, nextStep.Count);
            Assert.Same(actionMock, nextStep.LastMockInfo);
            Assert.Equal(5, nextStep.LastParam);
        }

        [Fact]
        public void do_nothing_if_no_step_in_lenient_mode_parameter_case()
        {
            var methodMock = new ActionMethodMock<int>(new object(), "ClassName", "InterfaceName", "MemberName", "MockName", Strictness.Lenient);
            methodMock.Call(5);
        }

        [Fact]
        public void throw_if_no_step_in_strict_mode_parameter_case()
        {
            var methodMock = new ActionMethodMock<int>(new object(), "ClassName", "InterfaceName", "MemberName", "MockName", Strictness.Strict);
            var ex = Assert.Throws<MockMissingException>(() => methodMock.Call(5));
            Assert.Equal(MockType.Method, ex.MemberType);
        }

        [Fact]
        public void throw_if_no_step_in_very_strict_mode_parameter_case()
        {
            var methodMock = new ActionMethodMock<int>(new object(), "ClassName", "InterfaceName", "MemberName", "MockName", Strictness.VeryStrict);
            var ex = Assert.Throws<MockMissingException>(() => methodMock.Call(5));
            Assert.Equal(MockType.Method, ex.MemberType);
        }

        [Fact]
        public void do_nothing_if_cleared_in_lenient_mode_parameter_case()
        {
            var methodMock = new ActionMethodMock<int>(new object(), "ClassName", "InterfaceName", "MemberName", "MockName", Strictness.Lenient);
            var nextStep = NextStepFor(methodMock);
            methodMock.Clear();
            methodMock.Call(5);
            Assert.Equal(0, nextStep.Count);
        }

        [Fact]
        public void throw_if_cleared_in_strict_mode_parameter_case()
        {
            var methodMock = new ActionMethodMock<int>(new object(), "ClassName", "InterfaceName", "MemberName", "MockName", Strictness.Strict);
            var nextStep = NextStepFor(methodMock);
            methodMock.Clear();
            var ex = Assert.Throws<MockMissingException>(() => methodMock.Call(5));
            Assert.Equal(MockType.Method, ex.MemberType);
            Assert.Equal(0, nextStep.Count);
        }

        [Fact]
        public void throw_if_cleared_in_very_strict_mode_parameter_case()
        {
            var methodMock = new ActionMethodMock<int>(new object(), "ClassName", "InterfaceName", "MemberName", "MockName", Strictness.VeryStrict);
            var nextStep = NextStepFor(methodMock);
            methodMock.Clear();
            var ex = Assert.Throws<MockMissingException>(() => methodMock.Call(5));
            Assert.Equal(MockType.Method, ex.MemberType);
            Assert.Equal(0, nextStep.Count);
        }
    }
}
