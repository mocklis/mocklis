// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PropertyMockValueTests.cs">
//   SPDX-License-Identifier: MIT
//   Copyright © 2019-2021 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.Core
{
    #region Using Directives

    using Mocklis.Helpers;
    using Xunit;

    #endregion

    public class PropertyMockValueTests
    {
        private static FakeNextPropertyStep<TValue> NextStepFor<TValue>(ICanHaveNextPropertyStep<TValue> mock, TValue value)
        {
            return new FakeNextPropertyStep<TValue>(mock, value);
        }

        [Fact]
        public void SendMockInformationToStepAndGetValueOnGetting()
        {
            var propertyMock = new PropertyMock<int>(new object(), "ClassName", "InterfaceName", "MemberName", "MockName", Strictness.Lenient);
            var nextStep = NextStepFor(propertyMock, 5);

            int value = propertyMock.Value;

            Assert.Equal(1, nextStep.GetCount);
            Assert.Equal(0, nextStep.SetCount);
            Assert.Same(propertyMock, nextStep.LastGetMockInfo);
            Assert.Equal(5, value);
        }

        [Fact]
        public void ReturnDefaultIfNoStepInLenientModeOnGetting()
        {
            var propertyMock = new PropertyMock<int>(new object(), "ClassName", "InterfaceName", "MemberName", "MockName", Strictness.Lenient);
            int result = propertyMock.Value;
            Assert.Equal(0, result);
        }

        [Fact]
        public void ThrowIfNoStepInStrictModeOnGetting()
        {
            var propertyMock = new PropertyMock<int>(new object(), "ClassName", "InterfaceName", "MemberName", "MockName", Strictness.Strict);
            var ex = Assert.Throws<MockMissingException>(() => propertyMock.Value);
            Assert.Equal(MockType.PropertyGet, ex.MemberType);
        }

        [Fact]
        public void ThrowIfNoStepInVeryStrictModeOnGetting()
        {
            var propertyMock = new PropertyMock<int>(new object(), "ClassName", "InterfaceName", "MemberName", "MockName", Strictness.VeryStrict);
            var ex = Assert.Throws<MockMissingException>(() => propertyMock.Value);
            Assert.Equal(MockType.PropertyGet, ex.MemberType);
        }

        [Fact]
        public void ReturnDefaultIfClearedInLenientModeOnGetting()
        {
            var propertyMock = new PropertyMock<int>(new object(), "ClassName", "InterfaceName", "MemberName", "MockName", Strictness.Lenient);
            var nextStep = NextStepFor(propertyMock, 5);
            propertyMock.Clear();
            int result = propertyMock.Value;
            Assert.Equal(0, nextStep.GetCount);
            Assert.Equal(0, nextStep.SetCount);
            Assert.Equal(0, result);
        }

        [Fact]
        public void ThrowIfClearedInStrictModeOnGetting()
        {
            var propertyMock = new PropertyMock<int>(new object(), "ClassName", "InterfaceName", "MemberName", "MockName", Strictness.Strict);
            var nextStep = NextStepFor(propertyMock, 5);
            propertyMock.Clear();
            var ex = Assert.Throws<MockMissingException>(() => propertyMock.Value);
            Assert.Equal(MockType.PropertyGet, ex.MemberType);
            Assert.Equal(0, nextStep.GetCount);
            Assert.Equal(0, nextStep.SetCount);
        }

        [Fact]
        public void ThrowIfClearedInVeryStrictModeOnGetting()
        {
            var propertyMock = new PropertyMock<int>(new object(), "ClassName", "InterfaceName", "MemberName", "MockName", Strictness.VeryStrict);
            var nextStep = NextStepFor(propertyMock, 5);
            propertyMock.Clear();
            var ex = Assert.Throws<MockMissingException>(() => propertyMock.Value);
            Assert.Equal(MockType.PropertyGet, ex.MemberType);
            Assert.Equal(0, nextStep.GetCount);
            Assert.Equal(0, nextStep.SetCount);
        }

        [Fact]
        public void SendMockInformationAndValueToStepOnSetting()
        {
            var propertyMock = new PropertyMock<int>(new object(), "ClassName", "InterfaceName", "MemberName", "MockName", Strictness.Lenient);
            var nextStep = NextStepFor(propertyMock, 5);

            propertyMock.Value = 5;

            Assert.Equal(0, nextStep.GetCount);
            Assert.Equal(1, nextStep.SetCount);
            Assert.Same(propertyMock, nextStep.LastSetMockInfo);
            Assert.Equal(5, nextStep.LastSetValue);
        }

        [Fact]
        public void DoNothingIfNoStepInLenientModeOnSetting()
        {
            var propertyMock = new PropertyMock<int>(new object(), "ClassName", "InterfaceName", "MemberName", "MockName", Strictness.Lenient);
            propertyMock.Value = 5;
        }

        [Fact]
        public void ThrowIfNoStepInStrictModeOnSetting()
        {
            var propertyMock = new PropertyMock<int>(new object(), "ClassName", "InterfaceName", "MemberName", "MockName", Strictness.Strict);
            var ex = Assert.Throws<MockMissingException>(() => propertyMock.Value = 5);
            Assert.Equal(MockType.PropertySet, ex.MemberType);
        }

        [Fact]
        public void ThrowIfNoStepInVeryStrictModeOnSetting()
        {
            var propertyMock = new PropertyMock<int>(new object(), "ClassName", "InterfaceName", "MemberName", "MockName", Strictness.VeryStrict);
            var ex = Assert.Throws<MockMissingException>(() => propertyMock.Value = 5);
            Assert.Equal(MockType.PropertySet, ex.MemberType);
        }

        [Fact]
        public void DoNothingIfClearedInLenientModeOnSetting()
        {
            var propertyMock = new PropertyMock<int>(new object(), "ClassName", "InterfaceName", "MemberName", "MockName", Strictness.Lenient);
            var nextStep = NextStepFor(propertyMock, 5);
            propertyMock.Clear();
            propertyMock.Value = 5;
            Assert.Equal(0, nextStep.GetCount);
            Assert.Equal(0, nextStep.SetCount);
        }

        [Fact]
        public void ThrowIfClearedInStrictModeOnSetting()
        {
            var propertyMock = new PropertyMock<int>(new object(), "ClassName", "InterfaceName", "MemberName", "MockName", Strictness.Strict);
            var nextStep = NextStepFor(propertyMock, 5);
            propertyMock.Clear();
            var ex = Assert.Throws<MockMissingException>(() => propertyMock.Value = 5);
            Assert.Equal(MockType.PropertySet, ex.MemberType);
            Assert.Equal(0, nextStep.GetCount);
            Assert.Equal(0, nextStep.SetCount);
        }

        [Fact]
        public void ThrowIfClearedInVeryStrictModeOnSetting()
        {
            var propertyMock = new PropertyMock<int>(new object(), "ClassName", "InterfaceName", "MemberName", "MockName", Strictness.VeryStrict);
            var nextStep = NextStepFor(propertyMock, 5);
            propertyMock.Clear();
            var ex = Assert.Throws<MockMissingException>(() => propertyMock.Value = 5);
            Assert.Equal(MockType.PropertySet, ex.MemberType);
            Assert.Equal(0, nextStep.GetCount);
            Assert.Equal(0, nextStep.SetCount);
        }
    }
}
