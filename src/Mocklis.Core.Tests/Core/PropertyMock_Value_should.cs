// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PropertyMock_Value_should.cs">
//   SPDX-License-Identifier: MIT
//   Copyright © 2019-2020 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.Core.Tests.Core
{
    #region Using Directives

    using Mocklis.Core.Tests.Helpers;
    using Xunit;

    #endregion

    public class PropertyMock_Value_should
    {
        private static FakeNextPropertyStep<TValue> NextStepFor<TValue>(ICanHaveNextPropertyStep<TValue> mock, TValue value)
        {
            return new FakeNextPropertyStep<TValue>(mock, value);
        }

        [Fact]
        public void send_mock_information_to_step_and_get_value_on_getting()
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
        public void return_default_if_no_step_in_lenient_mode_on_getting()
        {
            var propertyMock = new PropertyMock<int>(new object(), "ClassName", "InterfaceName", "MemberName", "MockName", Strictness.Lenient);
            int result = propertyMock.Value;
            Assert.Equal(0, result);
        }

        [Fact]
        public void throw_if_no_step_in_strict_mode_on_getting()
        {
            var propertyMock = new PropertyMock<int>(new object(), "ClassName", "InterfaceName", "MemberName", "MockName", Strictness.Strict);
            var ex = Assert.Throws<MockMissingException>(() => propertyMock.Value);
            Assert.Equal(MockType.PropertyGet, ex.MemberType);
        }

        [Fact]
        public void throw_if_no_step_in_very_strict_mode_on_getting()
        {
            var propertyMock = new PropertyMock<int>(new object(), "ClassName", "InterfaceName", "MemberName", "MockName", Strictness.VeryStrict);
            var ex = Assert.Throws<MockMissingException>(() => propertyMock.Value);
            Assert.Equal(MockType.PropertyGet, ex.MemberType);
        }

        [Fact]
        public void return_default_if_cleared_in_lenient_mode_on_getting()
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
        public void throw_if_cleared_in_strict_mode_on_getting()
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
        public void throw_if_cleared_in_very_strict_mode_on_getting()
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
        public void send_mock_information_and_value_to_step_on_setting()
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
        public void do_nothing_if_no_step_in_lenient_mode_on_setting()
        {
            var propertyMock = new PropertyMock<int>(new object(), "ClassName", "InterfaceName", "MemberName", "MockName", Strictness.Lenient);
            propertyMock.Value = 5;
        }

        [Fact]
        public void throw_if_no_step_in_strict_mode_on_setting()
        {
            var propertyMock = new PropertyMock<int>(new object(), "ClassName", "InterfaceName", "MemberName", "MockName", Strictness.Strict);
            var ex = Assert.Throws<MockMissingException>(() => propertyMock.Value = 5);
            Assert.Equal(MockType.PropertySet, ex.MemberType);
        }

        [Fact]
        public void throw_if_no_step_in_very_strict_mode_on_setting()
        {
            var propertyMock = new PropertyMock<int>(new object(), "ClassName", "InterfaceName", "MemberName", "MockName", Strictness.VeryStrict);
            var ex = Assert.Throws<MockMissingException>(() => propertyMock.Value = 5);
            Assert.Equal(MockType.PropertySet, ex.MemberType);
        }

        [Fact]
        public void do_nothing_if_cleared_in_lenient_mode_on_setting()
        {
            var propertyMock = new PropertyMock<int>(new object(), "ClassName", "InterfaceName", "MemberName", "MockName", Strictness.Lenient);
            var nextStep = NextStepFor(propertyMock, 5);
            propertyMock.Clear();
            propertyMock.Value = 5;
            Assert.Equal(0, nextStep.GetCount);
            Assert.Equal(0, nextStep.SetCount);
        }

        [Fact]
        public void throw_if_cleared_in_strict_mode_on_setting()
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
        public void throw_if_cleared_in_very_strict_mode_on_setting()
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
