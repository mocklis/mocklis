// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ActionMethodMock_constructor_should.cs">
//   SPDX-License-Identifier: MIT
//   Copyright © 2019 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.Core.Tests.Core
{
    #region Using Directives

    using System;
    using Xunit;

    #endregion

    public class ActionMethodMock_constructor_should
    {
        [Fact(DisplayName = "require mockInstance (parameterLess)")]
        public void require_mockInstance_X28parameterLessX29()
        {
            var exception = Assert.Throws<ArgumentNullException>(() =>
                new ActionMethodMock(null!, "MocklisClassName", "InterfaceName", "MemberName", "MemberMockName", Strictness.Lenient));
            Assert.Equal("mockInstance", exception.ParamName);
        }

        [Fact]
        public void require_mockInstance()
        {
            var exception = Assert.Throws<ArgumentNullException>(() =>
                new ActionMethodMock<int>(null!, "MocklisClassName", "InterfaceName", "MemberName", "MemberMockName", Strictness.Lenient));
            Assert.Equal("mockInstance", exception.ParamName);
        }

        [Fact(DisplayName = "require mocklisClassName (parameterLess)")]
        public void require_mocklisClassName_X28parameterLessX29()
        {
            var exception = Assert.Throws<ArgumentNullException>(() =>
                new ActionMethodMock(new object(), null!, "InterfaceName", "MemberName", "MemberMockName", Strictness.Lenient));
            Assert.Equal("mocklisClassName", exception.ParamName);
        }

        [Fact]
        public void require_mocklisClassName()
        {
            var exception = Assert.Throws<ArgumentNullException>(() =>
                new ActionMethodMock<int>(new object(), null!, "InterfaceName", "MemberName", "MemberMockName", Strictness.Lenient));
            Assert.Equal("mocklisClassName", exception.ParamName);
        }

        [Fact(DisplayName = "require interfaceName (parameterLess)")]
        public void require_interfaceName_X28parameterLessX29()
        {
            var exception = Assert.Throws<ArgumentNullException>(() =>
                new ActionMethodMock(new object(), "MocklisClassName", null!, "MemberName", "MemberMockName", Strictness.Lenient));
            Assert.Equal("interfaceName", exception.ParamName);
        }

        [Fact]
        public void require_interfaceName()
        {
            var exception = Assert.Throws<ArgumentNullException>(() =>
                new ActionMethodMock<int>(new object(), "MocklisClassName", null!, "MemberName", "MemberMockName", Strictness.Lenient));
            Assert.Equal("interfaceName", exception.ParamName);
        }

        [Fact(DisplayName = "require memberName (parameterLess)")]
        public void require_memberName_X28parameterLessX29()
        {
            var exception = Assert.Throws<ArgumentNullException>(() =>
                new ActionMethodMock(new object(), "MocklisClassName", "InterfaceName", null!, "MemberMockName", Strictness.Lenient));
            Assert.Equal("memberName", exception.ParamName);
        }

        [Fact]
        public void require_memberName()
        {
            var exception = Assert.Throws<ArgumentNullException>(() =>
                new ActionMethodMock<int>(new object(), "MocklisClassName", "InterfaceName", null!, "MemberMockName", Strictness.Lenient));
            Assert.Equal("memberName", exception.ParamName);
        }

        [Fact(DisplayName = "require MemberMockName (parameterLess)")]
        public void require_MemberMockName_X28parameterLessX29()
        {
            var exception = Assert.Throws<ArgumentNullException>(() =>
                new ActionMethodMock(new object(), "MocklisClassName", "InterfaceName", "MemberName", null!, Strictness.Lenient));
            Assert.Equal("memberMockName", exception.ParamName);
        }

        [Fact]
        public void require_MemberMockName()
        {
            var exception = Assert.Throws<ArgumentNullException>(() =>
                new ActionMethodMock<int>(new object(), "MocklisClassName", "InterfaceName", "MemberName", null!, Strictness.Lenient));
            Assert.Equal("memberMockName", exception.ParamName);
        }

        [Fact(DisplayName = "set IMockInfo properties (parameterLess)")]
        public void set_IMockInfo_properties_X28parameterLessX29()
        {
            var mockInstance = new object();
            var mockInfo = (IMockInfo)new ActionMethodMock(mockInstance, "MocklisClassName", "InterfaceName", "MemberName", "MemberMockName",
                Strictness.Lenient);
            Assert.Equal(mockInstance, mockInfo.MockInstance);
            Assert.Equal("MocklisClassName", mockInfo.MocklisClassName);
            Assert.Equal("InterfaceName", mockInfo.InterfaceName);
            Assert.Equal("MemberName", mockInfo.MemberName);
            Assert.Equal("MemberMockName", mockInfo.MemberMockName);
            Assert.Equal(Strictness.Lenient, mockInfo.Strictness);
        }

        [Fact]
        public void set_IMockInfo_properties()
        {
            var mockInstance = new object();
            var mockInfo = (IMockInfo)new ActionMethodMock<int>(mockInstance, "MocklisClassName", "InterfaceName", "MemberName", "MemberMockName",
                Strictness.Lenient);
            Assert.Equal(mockInstance, mockInfo.MockInstance);
            Assert.Equal("MocklisClassName", mockInfo.MocklisClassName);
            Assert.Equal("InterfaceName", mockInfo.InterfaceName);
            Assert.Equal("MemberName", mockInfo.MemberName);
            Assert.Equal("MemberMockName", mockInfo.MemberMockName);
            Assert.Equal(Strictness.Lenient, mockInfo.Strictness);
        }
    }
}
