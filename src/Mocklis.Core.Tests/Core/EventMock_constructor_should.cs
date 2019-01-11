// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EventMock_constructor_should.cs">
//   Copyright © 2019 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.Core.Tests.Core
{
    #region Using Directives

    using System;
    using Xunit;

    #endregion

    public class EventMock_constructor_should
    {
        [Fact]
        public void require_mockInstance()
        {
            var exception = Assert.Throws<ArgumentNullException>(() =>
                new EventMock<EventHandler>(null, "MocklisClassName", "InterfaceName", "MemberName", "MemberMockName"));
            Assert.Equal("mockInstance", exception.ParamName);
        }

        [Fact]
        public void require_mocklisClassName()
        {
            var exception = Assert.Throws<ArgumentNullException>(() =>
                new EventMock<EventHandler>(new object(), null, "InterfaceName", "MemberName", "MemberMockName"));
            Assert.Equal("mocklisClassName", exception.ParamName);
        }

        [Fact]
        public void require_interfaceName()
        {
            var exception = Assert.Throws<ArgumentNullException>(() =>
                new EventMock<EventHandler>(new object(), "MocklisClassName", null, "MemberName", "MemberMockName"));
            Assert.Equal("interfaceName", exception.ParamName);
        }

        [Fact]
        public void require_memberName()
        {
            var exception = Assert.Throws<ArgumentNullException>(() =>
                new EventMock<EventHandler>(new object(), "MocklisClassName", "InterfaceName", null, "MemberMockName"));
            Assert.Equal("memberName", exception.ParamName);
        }

        [Fact]
        public void require_memberMockName()
        {
            var exception = Assert.Throws<ArgumentNullException>(() =>
                new EventMock<EventHandler>(new object(), "MocklisClassName", "InterfaceName", "MemberName", null));
            Assert.Equal("memberMockName", exception.ParamName);
        }

        [Fact]
        public void set_IMockInfo_properties()
        {
            var mockInstance = new object();
            var mockInfo = (IMockInfo)new EventMock<EventHandler>(mockInstance, "MocklisClassName", "InterfaceName", "MemberName", "MemberMockName");
            Assert.Equal(mockInstance, mockInfo.MockInstance);
            Assert.Equal("MocklisClassName", mockInfo.MocklisClassName);
            Assert.Equal("InterfaceName", mockInfo.InterfaceName);
            Assert.Equal("MemberName", mockInfo.MemberName);
            Assert.Equal("MemberMockName", mockInfo.MemberMockName);
        }
    }
}
