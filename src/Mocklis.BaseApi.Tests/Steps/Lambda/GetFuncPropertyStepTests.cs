// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GetFuncPropertyStepTests.cs">
//   SPDX-License-Identifier: MIT
//   Copyright © 2019-2021 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.Steps.Lambda
{
    #region Using Directives

    using System;
    using System.Linq;
    using Mocklis.Interfaces;
    using Mocklis.Mocks;
    using Xunit;

    #endregion

    public class GetFuncPropertyStepTests
    {
        public MockMembers MockMembers { get; } = new MockMembers();
        public IProperties Sut => MockMembers;

        [Fact]
        public void RequireNonNullAction()
        {
            Assert.Throws<ArgumentNullException>(() => MockMembers.StringProperty.GetFunc(null!));
        }

        [Fact]
        public void EvaluateFuncOnGets()
        {
            MockMembers.StringProperty.GetFunc(() => "RESULT!!");

            var result = Sut.StringProperty;

            Assert.Equal("RESULT!!", result);
        }

        [Fact]
        public void ForwardSets()
        {
            MockMembers.StringProperty.GetFunc(() => throw new InvalidOperationException()).RecordBeforeSet(out var ledger);

            Sut.StringProperty = "Test";

            Assert.Equal(new[] { "Test" }, ledger);
            Assert.Equal(new[] { "Test" }, ledger.ToArray());
        }
    }
}
