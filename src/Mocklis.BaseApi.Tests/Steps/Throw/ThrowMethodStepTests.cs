// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ThrowMethodStepTests.cs">
//   SPDX-License-Identifier: MIT
//   Copyright © 2019-2020 Esbjörn Redmo and contributors. All rights reserved.
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

    public class ThrowMethodStepTests
    {
        private MockMembers MockMembers { get; } = new MockMembers();
        private IMethods Sut => MockMembers;

        [Fact]
        public void RequireExceptionFactory()
        {
            var ex = Assert.Throws<ArgumentNullException>(() => MockMembers.ActionWithParameter.Throw(null!));
            Assert.Equal("exceptionFactory", ex.ParamName);
        }

        [Fact]
        public void RequireExceptionFactoryInNoParameterCase()
        {
            var ex = Assert.Throws<ArgumentNullException>(() => MockMembers.SimpleAction.Throw((Func<Exception>)null!));
            Assert.Equal("exceptionFactory", ex.ParamName);
        }

        [Fact]
        public void ThrowOnCall()
        {
            MockMembers.ActionWithParameter.Throw(param => new SampleException<int>(param));
            var ex = Assert.Throws<SampleException<int>>(() => Sut.ActionWithParameter(5));
            Assert.Equal(5, ex.Payload);
        }

        [Fact]
        public void ThrowOnCallInNoParameterCase()
        {
            MockMembers.SimpleAction.Throw(() => new SampleException());
            Assert.Throws<SampleException>(() => Sut.SimpleAction());
        }

        [Fact]
        public void RequireExceptionFactoryWithInstance()
        {
            var ex = Assert.Throws<ArgumentNullException>(() => MockMembers.ActionWithParameter.InstanceThrow(null!));
            Assert.Equal("exceptionFactory", ex.ParamName);
        }

        [Fact]
        public void RequireExceptionFactoryInNoParameterCaseWithInstance()
        {
            var ex = Assert.Throws<ArgumentNullException>(() => MockMembers.SimpleAction.InstanceThrow((Func<object, Exception>)null!));
            Assert.Equal("exceptionFactory", ex.ParamName);
        }

        [Fact]
        public void ThrowOnCallWithInstance()
        {
            MockMembers.ActionWithParameter.InstanceThrow((i, param) => new SampleException<int>(param, i));
            var ex = Assert.Throws<SampleException<int>>(() => Sut.ActionWithParameter(5));
            Assert.Equal(5, ex.Payload);
            Assert.Same(MockMembers, ex.Instance);
        }

        [Fact]
        public void ThrowOnCallInNoParameterCaseWithInstance()
        {
            MockMembers.SimpleAction.InstanceThrow(i => new SampleException(i));
            var ex = Assert.Throws<SampleException>(() => Sut.SimpleAction());
            Assert.Same(MockMembers, ex.Instance);
        }
    }
}
