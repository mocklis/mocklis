// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LogStepExtensions_should.cs">
//   SPDX-License-Identifier: MIT
//   Copyright © 2019 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.Tests
{
    #region Using Directives

    using System;
    using Mocklis.Steps.Log;
    using Mocklis.Tests.Mocks;
    using Xunit;

    #endregion

    public class LogStepExtensions_should
    {
        public MockMembers MockMembers { get; } = new MockMembers();

        [Fact]
        public void RequireLogContextProviderForEvents()
        {
            var ex = Assert.Throws<ArgumentNullException>(() => MockMembers.MyEvent.Log((ILogContextProvider)null!));
            Assert.Equal("logContextProvider", ex.ParamName);
        }

        [Fact]
        public void RequireLogContextProviderForIndexers()
        {
            var ex = Assert.Throws<ArgumentNullException>(() => MockMembers.Item.Log((ILogContextProvider)null!));
            Assert.Equal("logContextProvider", ex.ParamName);
        }

        [Fact]
        public void RequireLogContextProviderForMethods()
        {
            var ex = Assert.Throws<ArgumentNullException>(() => MockMembers.SimpleAction.Log((ILogContextProvider)null!));
            Assert.Equal("logContextProvider", ex.ParamName);
        }

        [Fact]
        public void RequireLogContextProviderForProperties()
        {
            var ex = Assert.Throws<ArgumentNullException>(() => MockMembers.StringProperty.Log((ILogContextProvider)null!));
            Assert.Equal("logContextProvider", ex.ParamName);
        }
    }
}
