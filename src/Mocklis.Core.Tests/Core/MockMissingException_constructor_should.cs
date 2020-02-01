// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MockMissingException_constructor_should.cs">
//   SPDX-License-Identifier: MIT
//   Copyright © 2019-2020 Esbjörn Redmo and contributors. All rights reserved.
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
        private readonly IMockInfo _mockInfo =
            new PropertyMock<int>(new object(), "MocklisClassName", "InterfaceName", "MemberName", "MemberMockName", Strictness.Lenient);

        [Fact(DisplayName = "require valid memberType (innerException)")]
        public void require_valid_memberType_X28innerExceptionX29()
        {
            var exception = Assert.Throws<ArgumentOutOfRangeException>(() => new MockMissingException((MockType)17, _mockInfo, new Exception()));
            Assert.Equal("memberType", exception.ParamName);
        }

        [Fact]
        public void require_valid_memberType()
        {
            var exception = Assert.Throws<ArgumentOutOfRangeException>(() => new MockMissingException((MockType)17, _mockInfo));
            Assert.Equal("memberType", exception.ParamName);
        }

        [Fact(DisplayName = "require memberMock (innerException)")]
        public void require_memberMock_X28innerExceptionX29()
        {
            var exception = Assert.Throws<ArgumentNullException>(() => new MockMissingException(MockType.PropertyGet, null!, new Exception()));
            Assert.Equal("memberMock", exception.ParamName);
        }

        [Fact]
        public void require_memberMock()
        {
            var exception = Assert.Throws<ArgumentNullException>(() => new MockMissingException(MockType.PropertyGet, null!));
            Assert.Equal("memberMock", exception.ParamName);
        }

        [Fact(DisplayName = "set properties (innerException)")]
        public void set_properties_X28innerExceptionX29()
        {
            var innerException = new Exception();
            var exception = new MockMissingException(MockType.PropertyGet, _mockInfo, innerException);
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
            var exception = new MockMissingException(MockType.PropertyGet, _mockInfo);
            Assert.Equal(MockType.PropertyGet, exception.MemberType);
            Assert.Equal("MocklisClassName", exception.MocklisClassName);
            Assert.Equal("InterfaceName", exception.InterfaceName);
            Assert.Equal("MemberName", exception.MemberName);
            Assert.Equal("MemberMockName", exception.MemberMockName);
        }

        [Theory]
        [ClassData(typeof(SetMessageData))]
        public void set_message(MockType mockType, string expectedMessage)
        {
            var exception = new MockMissingException(mockType, "Class", "Interface", "Member", "Mock");
            Assert.Equal(expectedMessage, exception.Message);
        }

        public class SetMessageData : TheoryData<MockType, string>
        {
            public SetMessageData()
            {
                Add(MockType.Method, "No mock implementation found for Method 'Interface.Member'. Add one using 'Mock' on your 'Class' instance.");
                Add(MockType.PropertyGet,
                    "No mock implementation found for getting the value of Property 'Interface.Member'. Add one using 'Mock' on your 'Class' instance.");
                Add(MockType.PropertySet,
                    "No mock implementation found for setting the value of Property 'Interface.Member'. Add one using 'Mock' on your 'Class' instance.");
                Add(MockType.EventAdd,
                    "No mock implementation found for adding a handler to Event 'Interface.Member'. Add one using 'Mock' on your 'Class' instance.");
                Add(MockType.EventRemove,
                    "No mock implementation found for removing a handler from Event 'Interface.Member'. Add one using 'Mock' on your 'Class' instance.");
                Add(MockType.IndexerGet,
                    "No mock implementation found for getting a value via the Indexer on 'Interface'. Add one using 'Mock' on your 'Class' instance.");
                Add(MockType.IndexerSet,
                    "No mock implementation found for setting a value via the Indexer on 'Interface'. Add one using 'Mock' on your 'Class' instance.");
                Add(MockType.VirtualMethod,
                    "No mock implementation found for Method 'Interface.Member'. Add one by subclassing 'Class' and overriding the 'Mock' method.");
                Add(MockType.VirtualPropertyGet,
                    "No mock implementation found for getting the value of Property 'Interface.Member'. Add one by subclassing 'Class' and overriding the 'Mock' method (the one returning a value if more than one).");
                Add(MockType.VirtualPropertySet,
                    "No mock implementation found for setting the value of Property 'Interface.Member'. Add one by subclassing 'Class' and overriding the 'Mock' method (the one not returning a value if more than one).");
                Add(MockType.VirtualIndexerGet,
                    "No mock implementation found for getting a value via the Indexer on 'Interface'. Add one by subclassing 'Class' and overriding the 'Mock' method (the one returning a value if more than one).");
                Add(MockType.VirtualIndexerSet,
                    "No mock implementation found for setting a value via the Indexer on 'Interface'. Add one by subclassing 'Class' and overriding the 'Mock' method (the one not returning a value if more than one).");
            }
        }
    }
}
