// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GetFuncIndexerStep_should.cs">
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

    public class GetFuncIndexerStep_should
    {
        public MockMembers MockMembers { get; } = new MockMembers();
        public IIndexers Sut => MockMembers;

        [Fact]
        public void RequireNonNullAction()
        {
            Assert.Throws<ArgumentNullException>(() => MockMembers.Item.GetFunc(null!));
        }

        [Fact]
        public void evaluate_func_on_gets()
        {
            int usedIndex = 0;

            MockMembers.Item.GetFunc(i =>
            {
                usedIndex = i;
                return "RESULT!!";
            });

            var result = Sut[5];

            Assert.Equal(5, usedIndex);
            Assert.Equal("RESULT!!", result);
        }

        [Fact]
        public void forward_sets()
        {
            MockMembers.Item.GetFunc(i => throw new InvalidOperationException()).RecordBeforeSet(out var ledger);

            Sut[5] = "Test";

            Assert.Equal(new[] { (5, "Test") }, ledger);
        }
    }
}
