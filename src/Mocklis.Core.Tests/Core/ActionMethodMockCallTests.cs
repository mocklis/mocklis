// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ActionMethodMockCallTests.cs">
//   SPDX-License-Identifier: MIT
//   Copyright © 2019-2021 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.Core
{
    #region Using Directives

    using System;
    using Mocklis.Helpers;
    using Xunit;

    #endregion


    public class ActionMethodMockCallTests
    {
        private static FakeNextMethodStep<TParam, ValueTuple> NextStepFor<TParam>(ICanHaveNextMethodStep<TParam, ValueTuple> mock)
        {
            return new FakeNextMethodStep<TParam, ValueTuple>(mock, ValueTuple.Create());
        }

        [Fact]
        public void SendMockInformationToStep()
        {
            var methodMock = new ActionMethodMock(new object(), "ClassName", "InterfaceName", "MemberName", "MockName", Strictness.Lenient);
            var nextStep = NextStepFor(methodMock);

            methodMock.Call();

            Assert.Equal(1, nextStep.Count);
            Assert.Same(methodMock, nextStep.LastMockInfo);
        }

        [Fact]
        public void DoNothingIfNoStepInLenientMode()
        {
            var methodMock = new ActionMethodMock(new object(), "ClassName", "InterfaceName", "MemberName", "MockName", Strictness.Lenient);
            methodMock.Call();
        }

        [Fact]
        public void ThrowIfNoStepInStrictMode()
        {
            var methodMock = new ActionMethodMock(new object(), "ClassName", "InterfaceName", "MemberName", "MockName", Strictness.Strict);
            var ex = Assert.Throws<MockMissingException>(() => methodMock.Call());
            Assert.Equal(MockType.Method, ex.MemberType);
        }

        [Fact]
        public void ThrowIfNoStepInVeryStrictMode()
        {
            var methodMock = new ActionMethodMock(new object(), "ClassName", "InterfaceName", "MemberName", "MockName", Strictness.VeryStrict);
            var ex = Assert.Throws<MockMissingException>(() => methodMock.Call());
            Assert.Equal(MockType.Method, ex.MemberType);
        }

        [Fact]
        public void DoNothingIfClearedInLenientMode()
        {
            var methodMock = new ActionMethodMock(new object(), "ClassName", "InterfaceName", "MemberName", "MockName", Strictness.Lenient);
            var nextStep = NextStepFor(methodMock);
            methodMock.Clear();
            methodMock.Call();
            Assert.Equal(0, nextStep.Count);
        }

        [Fact]
        public void ThrowIfClearedInStrictMode()
        {
            var methodMock = new ActionMethodMock(new object(), "ClassName", "InterfaceName", "MemberName", "MockName", Strictness.Strict);
            var nextStep = NextStepFor(methodMock);
            methodMock.Clear();
            var ex = Assert.Throws<MockMissingException>(() => methodMock.Call());
            Assert.Equal(MockType.Method, ex.MemberType);
            Assert.Equal(0, nextStep.Count);
        }

        [Fact]
        public void ThrowIfClearedInVeryStrictMode()
        {
            var methodMock = new ActionMethodMock(new object(), "ClassName", "InterfaceName", "MemberName", "MockName", Strictness.VeryStrict);
            var nextStep = NextStepFor(methodMock);
            methodMock.Clear();
            var ex = Assert.Throws<MockMissingException>(() => methodMock.Call());
            Assert.Equal(MockType.Method, ex.MemberType);
            Assert.Equal(0, nextStep.Count);
        }


        [Fact]
        public void SendMockInformationAndParametersToStep()
        {
            var actionMock = new ActionMethodMock<int>(new object(), "ClassName", "InterfaceName", "MemberName", "MockName", Strictness.Lenient);
            var nextStep = NextStepFor(actionMock);

            actionMock.Call(5);

            Assert.Equal(1, nextStep.Count);
            Assert.Same(actionMock, nextStep.LastMockInfo);
            Assert.Equal(5, nextStep.LastParam);
        }

        [Fact]
        public void DoNothingIfNoStepInLenientModeParameterCase()
        {
            var methodMock = new ActionMethodMock<int>(new object(), "ClassName", "InterfaceName", "MemberName", "MockName", Strictness.Lenient);
            methodMock.Call(5);
        }

        [Fact]
        public void ThrowIfNoStepInStrictModeParameterCase()
        {
            var methodMock = new ActionMethodMock<int>(new object(), "ClassName", "InterfaceName", "MemberName", "MockName", Strictness.Strict);
            var ex = Assert.Throws<MockMissingException>(() => methodMock.Call(5));
            Assert.Equal(MockType.Method, ex.MemberType);
        }

        [Fact]
        public void ThrowIfNoStepInVeryStrictModeParameterCase()
        {
            var methodMock = new ActionMethodMock<int>(new object(), "ClassName", "InterfaceName", "MemberName", "MockName", Strictness.VeryStrict);
            var ex = Assert.Throws<MockMissingException>(() => methodMock.Call(5));
            Assert.Equal(MockType.Method, ex.MemberType);
        }

        [Fact]
        public void DoNothingIfClearedInLenientModeParameterCase()
        {
            var methodMock = new ActionMethodMock<int>(new object(), "ClassName", "InterfaceName", "MemberName", "MockName", Strictness.Lenient);
            var nextStep = NextStepFor(methodMock);
            methodMock.Clear();
            methodMock.Call(5);
            Assert.Equal(0, nextStep.Count);
        }

        [Fact]
        public void ThrowIfClearedInStrictModeParameterCase()
        {
            var methodMock = new ActionMethodMock<int>(new object(), "ClassName", "InterfaceName", "MemberName", "MockName", Strictness.Strict);
            var nextStep = NextStepFor(methodMock);
            methodMock.Clear();
            var ex = Assert.Throws<MockMissingException>(() => methodMock.Call(5));
            Assert.Equal(MockType.Method, ex.MemberType);
            Assert.Equal(0, nextStep.Count);
        }

        [Fact]
        public void ThrowIfClearedInVeryStrictModeParameterCase()
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
