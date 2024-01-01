// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LogStepExtensionsTests.cs">
//   SPDX-License-Identifier: MIT
//   Copyright © 2019-2024 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis
{
    #region Using Directives

    using System;
    using Mocklis.Mocks;
    using Mocklis.Steps.Log;
    using Xunit;

    #endregion

    public class LogStepExtensionsTests
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
