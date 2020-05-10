// --------------------------------------------------------------------------------------------------------------------
// <copyright file="InstanceGetFuncIndexerStepTests.cs">
//   SPDX-License-Identifier: MIT
//   Copyright © 2019-2020 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.Steps.Lambda
{
    #region Using Directives

    using System;
    using Mocklis.Interfaces;
    using Mocklis.Mocks;
    using Xunit;

    #endregion

    public class InstanceGetFuncIndexerStepTests
    {
        public MockMembers MockMembers { get; } = new MockMembers();
        public IIndexers Sut => MockMembers;

        [Fact]
        public void RequireNonNullAction()
        {
            Assert.Throws<ArgumentNullException>(() => MockMembers.Item.InstanceGetFunc(null!));
        }

        [Fact]
        public void EvaluateFuncOnGets()
        {
            object? callInstance = null;
            int usedIndex = 0;

            MockMembers.Item.InstanceGetFunc((obj, i) =>
            {
                callInstance = obj;
                usedIndex = i;
                return "RESULT!!";
            });

            var result = Sut[5];

            Assert.Same(Sut, callInstance);
            Assert.Equal(5, usedIndex);
            Assert.Equal("RESULT!!", result);
        }

        [Fact]
        public void ForwardSets()
        {
            MockMembers.Item.InstanceGetFunc((o, i) => throw new InvalidOperationException()).RecordBeforeSet(out var ledger);

            Sut[5] = "Test";

            Assert.Equal(new[] { (5, "Test") }, ledger);
        }
    }
}
