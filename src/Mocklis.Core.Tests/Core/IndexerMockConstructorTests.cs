// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IndexerMockConstructorTests.cs">
//   SPDX-License-Identifier: MIT
//   Copyright © 2019-2020 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.Core
{
    #region Using Directives

    using System;
    using Xunit;

    #endregion

    public class IndexerMockConstructorTests
    {
        [Fact]
        public void RequireMockInstance()
        {
            var exception = Assert.Throws<ArgumentNullException>(() =>
                new IndexerMock<int, string>(null!, "MocklisClassName", "InterfaceName", "MemberName", "MemberMockName", Strictness.Lenient));
            Assert.Equal("mockInstance", exception.ParamName);
        }

        [Fact]
        public void RequireMocklisClassName()
        {
            var exception = Assert.Throws<ArgumentNullException>(() =>
                new IndexerMock<int, string>(new object(), null!, "InterfaceName", "MemberName", "MemberMockName", Strictness.Lenient));
            Assert.Equal("mocklisClassName", exception.ParamName);
        }

        [Fact]
        public void RequireInterfaceName()
        {
            var exception = Assert.Throws<ArgumentNullException>(() =>
                new IndexerMock<int, string>(new object(), "MocklisClassName", null!, "MemberName", "MemberMockName", Strictness.Lenient));
            Assert.Equal("interfaceName", exception.ParamName);
        }

        [Fact]
        public void RequireMemberName()
        {
            var exception = Assert.Throws<ArgumentNullException>(() =>
                new IndexerMock<int, string>(new object(), "MocklisClassName", "InterfaceName", null!, "MemberMockName", Strictness.Lenient));
            Assert.Equal("memberName", exception.ParamName);
        }

        [Fact]
        public void RequireMemberMockName()
        {
            var exception = Assert.Throws<ArgumentNullException>(() =>
                new IndexerMock<int, string>(new object(), "MocklisClassName", "InterfaceName", "MemberName", null!, Strictness.Lenient));
            Assert.Equal("memberMockName", exception.ParamName);
        }

        [Fact]
        public void SetIMockInfoProperties()
        {
            var mockInstance = new object();
            var mockInfo = (IMockInfo)new IndexerMock<int, string>(mockInstance, "MocklisClassName", "InterfaceName", "MemberName", "MemberMockName",
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
