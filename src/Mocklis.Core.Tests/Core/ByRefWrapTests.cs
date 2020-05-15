// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ByRefWrapTests.cs">
//   SPDX-License-Identifier: MIT
//   Copyright © 2019-2020 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.Core
{
    #region Using Directives

    using Xunit;

    #endregion

    public class ByRefWrapTests
    {
        [Fact]
        public void CreateNewReferenceWithPassedValue()
        {
            ref var wrapped = ref ByRef<int>.Wrap(15);
            Assert.Equal(15, wrapped);
        }
    }
}
