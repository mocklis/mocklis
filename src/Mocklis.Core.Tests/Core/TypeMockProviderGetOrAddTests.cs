// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TypeMockProviderGetOrAddTests.cs">
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

    public class TypeMockProviderGetOrAddTests
    {
        private class FakeMemberMock : MemberMock
        {
            public string KeyString { get; }

            public FakeMemberMock(string keyString) : base(new object(), "Class", "Interface", "Member" + keyString, "Mock", Strictness.Lenient)
            {
                KeyString = keyString;
            }

            public override void Clear()
            {
                throw new NotImplementedException();
            }
        }

        public TypedMockProvider Sut { get; } = new TypedMockProvider();

        [Fact]
        public void CallFactoryMethodWithKeystring()
        {
            var x = (FakeMemberMock)Sut.GetOrAdd(new[] { typeof(string), typeof(bool) }, ks => new FakeMemberMock(ks));

            Assert.Equal("<String,Boolean>", x.KeyString);
        }

        [Fact]
        public void ReturnSameMockIfCalledWithSameTypes()
        {
            var x1 = (FakeMemberMock)Sut.GetOrAdd(new[] { typeof(string), typeof(bool) }, ks => new FakeMemberMock(ks));
            var x2 = (FakeMemberMock)Sut.GetOrAdd(new[] { typeof(string), typeof(bool) }, ks => new FakeMemberMock(ks));
            Assert.Same(x1, x2);
        }

        [Fact]
        public void ReturnDifferentMocksIfCalledWithDifferentTypes()
        {
            var x1 = (FakeMemberMock)Sut.GetOrAdd(new[] { typeof(string), typeof(bool) }, ks => new FakeMemberMock(ks));
            var x2 = (FakeMemberMock)Sut.GetOrAdd(new[] { typeof(int), typeof(bool) }, ks => new FakeMemberMock(ks));
            Assert.NotSame(x1, x2);
        }

        [Fact]
        public void ReturnDifferentMocksIfCalledWithSameTypesInDifferentOrder()
        {
            var x1 = (FakeMemberMock)Sut.GetOrAdd(new[] { typeof(string), typeof(bool) }, ks => new FakeMemberMock(ks));
            var x2 = (FakeMemberMock)Sut.GetOrAdd(new[] { typeof(bool), typeof(string) }, ks => new FakeMemberMock(ks));
            Assert.NotSame(x1, x2);
        }

        [Fact]
        public void RequireTypeArray()
        {
            var ex = Assert.Throws<ArgumentNullException>(() => Sut.GetOrAdd(null!, a => new FakeMemberMock(string.Empty)));
            Assert.Equal("types", ex.ParamName);
        }

        [Fact]
        public void RequireFactory()
        {
            var ex = Assert.Throws<ArgumentNullException>(() => Sut.GetOrAdd(new[] { typeof(int) }, null!));
            Assert.Equal("factory", ex.ParamName);
        }
    }
}
