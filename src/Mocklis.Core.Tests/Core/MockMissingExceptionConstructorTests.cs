// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MockMissingExceptionConstructorTests.cs">
//   SPDX-License-Identifier: MIT
//   Copyright © 2019-2024 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.Core
{
    #region Using Directives

    using System;
    using Xunit;

    #endregion

    public class MockMissingExceptionConstructorTests
    {
        private readonly IMockInfo _mockInfo =
            new PropertyMock<int>(new object(), "MocklisClassName", "InterfaceName", "MemberName", "MemberMockName", Strictness.Lenient);

        [Fact]
        public void InnerExceptionConstructorRequiresValidMemberType()
        {
            var exception = Assert.Throws<ArgumentOutOfRangeException>(() => new MockMissingException((MockType)17, _mockInfo, new Exception()));
            Assert.Equal("memberType", exception.ParamName);
        }

        [Fact]
        public void ConstructorRequiresValidMemberType()
        {
            var exception = Assert.Throws<ArgumentOutOfRangeException>(() => new MockMissingException((MockType)17, _mockInfo));
            Assert.Equal("memberType", exception.ParamName);
        }

        [Fact]
        public void InnerExceptionConstructorRequiresMemberMock()
        {
            var exception = Assert.Throws<ArgumentNullException>(() => new MockMissingException(MockType.PropertyGet, null!, new Exception()));
            Assert.Equal("memberMock", exception.ParamName);
        }

        [Fact]
        public void ConstructorRequiresMemberMock()
        {
            var exception = Assert.Throws<ArgumentNullException>(() => new MockMissingException(MockType.PropertyGet, null!));
            Assert.Equal("memberMock", exception.ParamName);
        }

        [Fact]
        public void InnerExceptionConstructorSetsProperties()
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
        public void ConstructorSetsProperties()
        {
            var exception = new MockMissingException(MockType.PropertyGet, _mockInfo);
            Assert.Equal(MockType.PropertyGet, exception.MemberType);
            Assert.Equal("MocklisClassName", exception.MocklisClassName);
            Assert.Equal("InterfaceName", exception.InterfaceName);
            Assert.Equal("MemberName", exception.MemberName);
            Assert.Equal("MemberMockName", exception.MemberMockName);
        }

        [Theory]
        [InlineData(MockType.Method)]
        [InlineData(MockType.PropertyGet)]
        [InlineData(MockType.PropertySet)]
        [InlineData(MockType.EventAdd)]
        [InlineData(MockType.EventRemove)]
        [InlineData(MockType.IndexerGet)]
        [InlineData(MockType.IndexerSet)]
        [InlineData(MockType.VirtualMethod)]
        [InlineData(MockType.VirtualPropertyGet)]
        [InlineData(MockType.VirtualPropertySet)]
        [InlineData(MockType.VirtualIndexerGet)]
        [InlineData(MockType.VirtualIndexerSet)]
        public void SetMessage(MockType mockType)
        {
            var expectedMessage = GetExpectedMessage(mockType);
            var exception = new MockMissingException(mockType, "Class", "Interface", "Member", "Mock");
            Assert.Equal(expectedMessage, exception.Message);
        }

        private static string GetExpectedMessage(MockType mockType)
        {
            return mockType switch
            {
                MockType.Method => "No mock implementation found for Method 'Interface.Member'. Add one using 'Mock' on your 'Class' instance.",
                MockType.PropertyGet =>
                    "No mock implementation found for getting the value of Property 'Interface.Member'. Add one using 'Mock' on your 'Class' instance.",
                MockType.PropertySet =>
                    "No mock implementation found for setting the value of Property 'Interface.Member'. Add one using 'Mock' on your 'Class' instance.",
                MockType.EventAdd =>
                    "No mock implementation found for adding a handler to Event 'Interface.Member'. Add one using 'Mock' on your 'Class' instance.",
                MockType.EventRemove =>
                    "No mock implementation found for removing a handler from Event 'Interface.Member'. Add one using 'Mock' on your 'Class' instance.",
                MockType.IndexerGet =>
                    "No mock implementation found for getting a value via the Indexer on 'Interface'. Add one using 'Mock' on your 'Class' instance.",
                MockType.IndexerSet =>
                    "No mock implementation found for setting a value via the Indexer on 'Interface'. Add one using 'Mock' on your 'Class' instance.",
                MockType.VirtualMethod =>
                    "No mock implementation found for Method 'Interface.Member'. Add one by subclassing 'Class' and overriding the 'Mock' method.",
                MockType.VirtualPropertyGet =>
                    "No mock implementation found for getting the value of Property 'Interface.Member'. Add one by subclassing 'Class' and overriding the 'Mock' method (the one returning a value if more than one).",
                MockType.VirtualPropertySet =>
                    "No mock implementation found for setting the value of Property 'Interface.Member'. Add one by subclassing 'Class' and overriding the 'Mock' method (the one not returning a value if more than one).",
                MockType.VirtualIndexerGet =>
                    "No mock implementation found for getting a value via the Indexer on 'Interface'. Add one by subclassing 'Class' and overriding the 'Mock' method (the one returning a value if more than one).",
                MockType.VirtualIndexerSet =>
                    "No mock implementation found for setting a value via the Indexer on 'Interface'. Add one by subclassing 'Class' and overriding the 'Mock' method (the one not returning a value if more than one).",
                _ => throw new ArgumentOutOfRangeException(nameof(mockType))
            };
        }
    }
}
