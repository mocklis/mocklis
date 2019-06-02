// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GetFuncPropertyStep_should.cs">
//   SPDX-License-Identifier: MIT
//   Copyright © 2019 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.Tests.Steps.Lambda
{
    #region Using Directives

    using System;
    using System.Linq;
    using Mocklis.Tests.Interfaces;
    using Mocklis.Tests.Mocks;
    using Xunit;

    #endregion

    public class GetFuncPropertyStep_should
    {
        public MockMembers MockMembers { get; } = new MockMembers();
        public IProperties Sut => MockMembers;

        [Fact]
        public void RequireNonNullAction()
        {
            Assert.Throws<ArgumentNullException>(() => MockMembers.StringProperty.GetFunc(null));
        }

        [Fact]
        public void evaluate_func_on_gets()
        {
            MockMembers.StringProperty.GetFunc(() => "RESULT!!");

            var result = Sut.StringProperty;

            Assert.Equal("RESULT!!", result);
        }

        [Fact]
        public void forward_sets()
        {
            MockMembers.StringProperty.GetFunc(() => throw new InvalidOperationException()).RecordBeforeSet(out var ledger);

            Sut.StringProperty = "Test";

            Assert.Equal(new[] { "Test" }, ledger);
            Assert.Equal(new[] { "Test" }, ledger.ToArray());
        }
    }
}
