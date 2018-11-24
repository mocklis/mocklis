// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MockMissingException_constructor_should.cs">
//   Copyright © 2018 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.Core.Tests.Core
{
    #region Using Directives

    using System;
    using Xunit;

    #endregion

    public class MockMissingException_constructor_should
    {
        private readonly MemberMock _memberMock =
            new PropertyMock<int>(new object(), "MocklisClassName", "InterfaceName", "MemberName", "MemberMockName");

        [Fact(DisplayName = "require valid memberType (innerException)")]
        public void require_valid_memberType_X28innerExceptionX29()
        {
            var exception = Assert.Throws<ArgumentOutOfRangeException>(() => new MockMissingException((MockType)17, _memberMock, new Exception()));
            Assert.Equal("memberType", exception.ParamName);
        }

        [Fact]
        public void require_valid_memberType()
        {
            var exception = Assert.Throws<ArgumentOutOfRangeException>(() => new MockMissingException((MockType)17, _memberMock));
            Assert.Equal("memberType", exception.ParamName);
        }

        [Fact(DisplayName = "require memberMock (innerException)")]
        public void require_memberMock_X28innerExceptionX29()
        {
            var exception = Assert.Throws<ArgumentNullException>(() => new MockMissingException(MockType.PropertyGet, null, new Exception()));
            Assert.Equal("memberMock", exception.ParamName);
        }

        [Fact]
        public void require_memberMock()
        {
            var exception = Assert.Throws<ArgumentNullException>(() => new MockMissingException(MockType.PropertyGet, null));
            Assert.Equal("memberMock", exception.ParamName);
        }

        [Fact(DisplayName = "set properties (innerException)")]
        public void set_properties_X28innerExceptionX29()
        {
            var innerException = new Exception();
            var exception = new MockMissingException(MockType.PropertyGet, _memberMock, innerException);
            Assert.Equal(MockType.PropertyGet, exception.MemberType);
            Assert.Equal("MocklisClassName", exception.MocklisClassName);
            Assert.Equal("InterfaceName", exception.InterfaceName);
            Assert.Equal("MemberName", exception.MemberName);
            Assert.Equal("MemberMockName", exception.MemberMockName);
            Assert.Same(innerException, exception.InnerException);
        }

        [Fact]
        public void set_properties()
        {
            var exception = new MockMissingException(MockType.PropertyGet, _memberMock);
            Assert.Equal(MockType.PropertyGet, exception.MemberType);
            Assert.Equal("MocklisClassName", exception.MocklisClassName);
            Assert.Equal("InterfaceName", exception.InterfaceName);
            Assert.Equal("MemberName", exception.MemberName);
            Assert.Equal("MemberMockName", exception.MemberMockName);
        }
    }
}
