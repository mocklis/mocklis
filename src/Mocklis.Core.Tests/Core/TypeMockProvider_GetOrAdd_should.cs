// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TypeMockProvider_GetOrAdd_should.cs">
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

    public class TypeMockProvider_GetOrAdd_should
    {
        private class FakeMemberMock : MemberMock
        {
            public string KeyString { get; }

            public FakeMemberMock(string keyString) : base(new object(), "Class", "Interface", "Member" + keyString, "Mock")
            {
                KeyString = keyString;
            }
        }

        public TypedMockProvider Sut { get; } = new TypedMockProvider();

        [Fact]
        public void call_factory_method_with_keystring()
        {
            var x = (FakeMemberMock)Sut.GetOrAdd(new[] { typeof(string), typeof(bool) }, ks => new FakeMemberMock(ks));

            Assert.Equal("<String,Boolean>", x.KeyString);
        }

        [Fact]
        public void return_same_mock_if_called_with_same_types()
        {
            var x1 = (FakeMemberMock)Sut.GetOrAdd(new[] { typeof(string), typeof(bool) }, ks => new FakeMemberMock(ks));
            var x2 = (FakeMemberMock)Sut.GetOrAdd(new[] { typeof(string), typeof(bool) }, ks => new FakeMemberMock(ks));
            Assert.Same(x1, x2);
        }

        [Fact]
        public void return_different_mocks_if_called_with_different_types()
        {
            var x1 = (FakeMemberMock)Sut.GetOrAdd(new[] { typeof(string), typeof(bool) }, ks => new FakeMemberMock(ks));
            var x2 = (FakeMemberMock)Sut.GetOrAdd(new[] { typeof(int), typeof(bool) }, ks => new FakeMemberMock(ks));
            Assert.NotSame(x1, x2);
        }

        [Fact]
        public void return_different_mocks_if_called_with_same_types_in_different_order()
        {
            var x1 = (FakeMemberMock)Sut.GetOrAdd(new[] { typeof(string), typeof(bool) }, ks => new FakeMemberMock(ks));
            var x2 = (FakeMemberMock)Sut.GetOrAdd(new[] { typeof(bool), typeof(string) }, ks => new FakeMemberMock(ks));
            Assert.NotSame(x1, x2);
        }

        [Fact]
        public void require_type_array()
        {
            var ex = Assert.Throws<ArgumentNullException>(() => Sut.GetOrAdd(null, a => new FakeMemberMock(string.Empty)));
            Assert.Equal("types", ex.ParamName);
        }

        [Fact]
        public void require_factory()
        {
            var ex = Assert.Throws<ArgumentNullException>(() => Sut.GetOrAdd(new[] { typeof(int) }, null));
            Assert.Equal("factory", ex.ParamName);
        }
    }
}
