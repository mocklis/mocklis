// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FuncMethodMock_constructor_should.cs">
//   Copyright © 2018 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.Core.Tests.Core
{
    #region Using Directives

    using System;
    using Xunit;

    #endregion

    public class FuncMethodMock_constructor_should
    {
        [Fact(DisplayName = "require mockInstance (parameterLess)")]
        public void require_mockInstance_X28parameterLessX29()
        {
            var exception = Assert.Throws<ArgumentNullException>(() =>
                new FuncMethodMock<string>(null, "MocklisClassName", "InterfaceName", "MemberName", "MemberMockName"));
            Assert.Equal("mockInstance", exception.ParamName);
        }

        [Fact]
        public void require_mockInstance()
        {
            var exception = Assert.Throws<ArgumentNullException>(() =>
                new FuncMethodMock<int, string>(null, "MocklisClassName", "InterfaceName", "MemberName", "MemberMockName"));
            Assert.Equal("mockInstance", exception.ParamName);
        }

        [Fact(DisplayName = "require mocklisClassName (parameterLess)")]
        public void require_mocklisClassName_X28parameterLessX29()
        {
            var exception = Assert.Throws<ArgumentNullException>(() =>
                new FuncMethodMock<string>(new object(), null, "InterfaceName", "MemberName", "MemberMockName"));
            Assert.Equal("mocklisClassName", exception.ParamName);
        }

        [Fact]
        public void require_mocklisClassName()
        {
            var exception = Assert.Throws<ArgumentNullException>(() =>
                new FuncMethodMock<int, string>(new object(), null, "InterfaceName", "MemberName", "MemberMockName"));
            Assert.Equal("mocklisClassName", exception.ParamName);
        }

        [Fact(DisplayName = "require interfaceName (parameterLess)")]
        public void require_interfaceName_X28parameterLessX29()
        {
            var exception = Assert.Throws<ArgumentNullException>(() =>
                new FuncMethodMock<string>(new object(), "MocklisClassName", null, "MemberName", "MemberMockName"));
            Assert.Equal("interfaceName", exception.ParamName);
        }

        [Fact]
        public void require_interfaceName()
        {
            var exception = Assert.Throws<ArgumentNullException>(() =>
                new FuncMethodMock<int, string>(new object(), "MocklisClassName", null, "MemberName", "MemberMockName"));
            Assert.Equal("interfaceName", exception.ParamName);
        }

        [Fact(DisplayName = "require memberName (parameterLess)")]
        public void require_memberName_X28parameterLessX29()
        {
            var exception = Assert.Throws<ArgumentNullException>(() =>
                new FuncMethodMock<string>(new object(), "MocklisClassName", "InterfaceName", null, "MemberMockName"));
            Assert.Equal("memberName", exception.ParamName);
        }

        [Fact]
        public void require_memberName()
        {
            var exception = Assert.Throws<ArgumentNullException>(() =>
                new FuncMethodMock<int, string>(new object(), "MocklisClassName", "InterfaceName", null, "MemberMockName"));
            Assert.Equal("memberName", exception.ParamName);
        }

        [Fact(DisplayName = "require MemberMockName (parameterLess)")]
        public void require_MemberMockName_X28parameterLessX29()
        {
            var exception = Assert.Throws<ArgumentNullException>(() =>
                new FuncMethodMock<string>(new object(), "MocklisClassName", "InterfaceName", "MemberName", null));
            Assert.Equal("memberMockName", exception.ParamName);
        }

        [Fact]
        public void require_MemberMockName()
        {
            var exception = Assert.Throws<ArgumentNullException>(() =>
                new FuncMethodMock<int, string>(new object(), "MocklisClassName", "InterfaceName", "MemberName", null));
            Assert.Equal("memberMockName", exception.ParamName);
        }

        [Fact(DisplayName = "set properties (parameterLess)")]
        public void set_IMockInfo_properties_X28parameterLessX29()
        {
            var mockInstance = new object();
            var mockInfo = (IMockInfo)new FuncMethodMock<string>(mockInstance, "MocklisClassName", "InterfaceName", "MemberName", "MemberMockName");
            Assert.Equal(mockInstance, mockInfo.MockInstance);
            Assert.Equal("MocklisClassName", mockInfo.MocklisClassName);
            Assert.Equal("InterfaceName", mockInfo.InterfaceName);
            Assert.Equal("MemberName", mockInfo.MemberName);
            Assert.Equal("MemberMockName", mockInfo.MemberMockName);
        }

        [Fact]
        public void set_IMockInfo_properties()
        {
            var mockInstance = new object();
            var mockInfo = (IMockInfo)new FuncMethodMock<int, string>(mockInstance, "MocklisClassName", "InterfaceName", "MemberName",
                "MemberMockName");
            Assert.Equal(mockInstance, mockInfo.MockInstance);
            Assert.Equal("MocklisClassName", mockInfo.MocklisClassName);
            Assert.Equal("InterfaceName", mockInfo.InterfaceName);
            Assert.Equal("MemberName", mockInfo.MemberName);
            Assert.Equal("MemberMockName", mockInfo.MemberMockName);
        }
    }
}
