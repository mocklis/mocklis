// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FuncMethodMockCallTests.cs">
//   SPDX-License-Identifier: MIT
//   Copyright © 2019-2023 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.Core
{
    #region Using Directives

    using Mocklis.Helpers;
    using Xunit;

    #endregion

    public class FuncMethodMockCallTests
    {
        private static FakeNextMethodStep<TParam, TResult> NextStepFor<TParam, TResult>(ICanHaveNextMethodStep<TParam, TResult> mock, TResult result)
        {
            return new FakeNextMethodStep<TParam, TResult>(mock, result);
        }

        [Fact]
        public void SendMockInformationToStepAndGetResultBack()
        {
            var methodMock = new FuncMethodMock<string>(new object(), "ClassName", "InterfaceName", "MemberName", "MockName", Strictness.Lenient);
            var nextStep = NextStepFor(methodMock, "5");

            string result = methodMock.Call();

            Assert.Equal(1, nextStep.Count);
            Assert.Same(methodMock, nextStep.LastMockInfo);
            Assert.Equal("5", result);
        }

        [Fact]
        public void ReturnDefaultIfNoStepInLenientMode()
        {
            var methodMock = new FuncMethodMock<string>(new object(), "ClassName", "InterfaceName", "MemberName", "MockName", Strictness.Lenient);
            string result = methodMock.Call();
            Assert.Null(result);
        }

        [Fact]
        public void ThrowIfNoStepInStrictMode()
        {
            var methodMock = new FuncMethodMock<string>(new object(), "ClassName", "InterfaceName", "MemberName", "MockName", Strictness.Strict);
            var ex = Assert.Throws<MockMissingException>(() => methodMock.Call());
            Assert.Equal(MockType.Method, ex.MemberType);
        }

        [Fact]
        public void ThrowIfNoStepInVeryStrictMode()
        {
            var methodMock = new FuncMethodMock<string>(new object(), "ClassName", "InterfaceName", "MemberName", "MockName", Strictness.VeryStrict);
            var ex = Assert.Throws<MockMissingException>(() => methodMock.Call());
            Assert.Equal(MockType.Method, ex.MemberType);
        }

        [Fact]
        public void ReturnDefaultIfClearedInLenientMode()
        {
            var methodMock = new FuncMethodMock<string>(new object(), "ClassName", "InterfaceName", "MemberName", "MockName", Strictness.Lenient);
            var nextStep = NextStepFor(methodMock, "5");
            methodMock.Clear();
            string result = methodMock.Call();
            Assert.Equal(0, nextStep.Count);
            Assert.Null(result);
        }

        [Fact]
        public void ThrowIfClearedInStrictMode()
        {
            var methodMock = new FuncMethodMock<string>(new object(), "ClassName", "InterfaceName", "MemberName", "MockName", Strictness.Strict);
            var nextStep = NextStepFor(methodMock, "5");
            methodMock.Clear();
            var ex = Assert.Throws<MockMissingException>(() => methodMock.Call());
            Assert.Equal(MockType.Method, ex.MemberType);
            Assert.Equal(0, nextStep.Count);
        }

        [Fact]
        public void ThrowIfClearedInVeryStrictMode()
        {
            var methodMock = new FuncMethodMock<string>(new object(), "ClassName", "InterfaceName", "MemberName", "MockName", Strictness.VeryStrict);
            var nextStep = NextStepFor(methodMock, "5");
            methodMock.Clear();
            var ex = Assert.Throws<MockMissingException>(() => methodMock.Call());
            Assert.Equal(MockType.Method, ex.MemberType);
            Assert.Equal(0, nextStep.Count);
        }

        [Fact]
        public void SendMockInformationAndParametersToStepAndGetResultBack()
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
        public void ReturnDefaultIfNoStepInLenientModeParameterCase()
        {
            var methodMock =
                new FuncMethodMock<int, string>(new object(), "ClassName", "InterfaceName", "MemberName", "MockName", Strictness.Lenient);
            string result = methodMock.Call(5);
            Assert.Null(result);
        }

        [Fact]
        public void ThrowIfNoStepInStrictModeParameterCase()
        {
            var methodMock = new FuncMethodMock<int, string>(new object(), "ClassName", "InterfaceName", "MemberName", "MockName", Strictness.Strict);
            var ex = Assert.Throws<MockMissingException>(() => methodMock.Call(5));
            Assert.Equal(MockType.Method, ex.MemberType);
        }

        [Fact]
        public void ThrowIfNoStepInVeryStrictModeParameterCase()
        {
            var methodMock =
                new FuncMethodMock<int, string>(new object(), "ClassName", "InterfaceName", "MemberName", "MockName", Strictness.VeryStrict);
            var ex = Assert.Throws<MockMissingException>(() => methodMock.Call(5));
            Assert.Equal(MockType.Method, ex.MemberType);
        }

        [Fact]
        public void ReturnDefaultIfClearedInLenientModeParameterCase()
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
        public void ThrowIfClearedInStrictModeParameterCase()
        {
            var methodMock = new FuncMethodMock<int, string>(new object(), "ClassName", "InterfaceName", "MemberName", "MockName", Strictness.Strict);
            var nextStep = NextStepFor(methodMock, "5");
            methodMock.Clear();
            var ex = Assert.Throws<MockMissingException>(() => methodMock.Call(5));
            Assert.Equal(MockType.Method, ex.MemberType);
            Assert.Equal(0, nextStep.Count);
        }

        [Fact]
        public void ThrowIfClearedInVeryStrictModeParameterCase()
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
