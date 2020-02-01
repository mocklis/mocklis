// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ThrowIndexerStep_should.cs">
//   SPDX-License-Identifier: MIT
//   Copyright © 2019-2020 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.Tests.Steps.Throw
{
    #region Using Directives

    using System;
    using Mocklis.Tests.Helpers;
    using Mocklis.Tests.Interfaces;
    using Mocklis.Tests.Mocks;
    using Xunit;

    #endregion

    public class ThrowIndexerStep_should
    {
        private MockMembers MockMembers { get; } = new MockMembers();
        private IIndexers Sut => MockMembers;

        [Fact]
        public void RequireExceptionFactory()
        {
            var ex = Assert.Throws<ArgumentNullException>(() => MockMembers.Item.Throw(null!));
            Assert.Equal("exceptionFactory", ex.ParamName);
        }

        [Fact]
        public void ThrowOnGet()
        {
            MockMembers.Item.Throw(key => new SampleException<int>(key));
            var ex = Assert.Throws<SampleException<int>>(() => Sut[5]);
            Assert.Equal(5, ex.Payload);
        }

        [Fact]
        public void ThrowOnSet()
        {
            MockMembers.Item.Throw(key => new SampleException<int>(key));
            var ex = Assert.Throws<SampleException<int>>(() => Sut[5] = "Test");
            Assert.Equal(5, ex.Payload);
        }


        [Fact]
        public void RequireExceptionFactoryWithInstance()
        {
            var ex = Assert.Throws<ArgumentNullException>(() => MockMembers.Item.InstanceThrow(null!));
            Assert.Equal("exceptionFactory", ex.ParamName);
        }

        [Fact]
        public void ThrowOnGetWithInstance()
        {
            MockMembers.Item.InstanceThrow((i, key) => new SampleException<int>(key, i));
            var ex = Assert.Throws<SampleException<int>>(() => Sut[5]);
            Assert.Equal(5, ex.Payload);
            Assert.Same(MockMembers, ex.Instance);
        }

        [Fact]
        public void ThrowOnSetWithInstance()
        {
            MockMembers.Item.InstanceThrow((i, key) => new SampleException<int>(key, i));
            var ex = Assert.Throws<SampleException<int>>(() => Sut[5] = "Test");
            Assert.Equal(5, ex.Payload);
            Assert.Same(MockMembers, ex.Instance);
        }
    }
}
