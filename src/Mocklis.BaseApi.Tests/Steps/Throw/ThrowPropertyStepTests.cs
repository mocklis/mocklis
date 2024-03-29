// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ThrowPropertyStepTests.cs">
//   SPDX-License-Identifier: MIT
//   Copyright © 2019-2024 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.Steps.Throw
{
    #region Using Directives

    using System;
    using Mocklis.Helpers;
    using Mocklis.Interfaces;
    using Mocklis.Mocks;
    using Xunit;

    #endregion

    public class ThrowPropertyStepTests
    {
        private MockMembers MockMembers { get; } = new MockMembers();
        private IProperties Sut => MockMembers;

        [Fact]
        public void RequireExceptionFactory()
        {
            var ex = Assert.Throws<ArgumentNullException>(() => MockMembers.IntProperty.Throw(null!));
            Assert.Equal("exceptionFactory", ex.ParamName);
        }

        [Fact]
        public void ThrowOnGet()
        {
            MockMembers.IntProperty.Throw(() => new SampleException());
            Assert.Throws<SampleException>(() => Sut.IntProperty);
        }

        [Fact]
        public void ThrowOnSet()
        {
            MockMembers.IntProperty.Throw(() => new SampleException());
            Assert.Throws<SampleException>(() => Sut.IntProperty = 5);
        }

        [Fact]
        public void RequireExceptionFactoryWithInstance()
        {
            var ex = Assert.Throws<ArgumentNullException>(() => MockMembers.IntProperty.InstanceThrow(null!));
            Assert.Equal("exceptionFactory", ex.ParamName);
        }

        [Fact]
        public void ThrowOnGetWithInstance()
        {
            MockMembers.IntProperty.InstanceThrow(i => new SampleException(i));
            var ex = Assert.Throws<SampleException>(() => Sut.IntProperty);
            Assert.Same(MockMembers, ex.Instance);
        }

        [Fact]
        public void ThrowOnSetWithInstance()
        {
            MockMembers.IntProperty.InstanceThrow(i => new SampleException(i));
            var ex = Assert.Throws<SampleException>(() => Sut.IntProperty = 5);
            Assert.Same(MockMembers, ex.Instance);
        }
    }
}
