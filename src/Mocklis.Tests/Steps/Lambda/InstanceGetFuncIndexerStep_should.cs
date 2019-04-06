// --------------------------------------------------------------------------------------------------------------------
// <copyright file="InstanceGetFuncIndexerStep_should.cs">
//   SPDX-License-Identifier: MIT
//   Copyright © 2019 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.Tests.Steps.Lambda
{
    #region Using Directives

    using System;
    using Mocklis.Tests.Interfaces;
    using Mocklis.Tests.Mocks;
    using Xunit;

    #endregion

    public class InstanceGetFuncIndexerStep_should
    {
        public MockMembers MockMembers { get; } = new MockMembers();
        public IIndexers Sut => MockMembers;

        [Fact]
        public void RequireNonNullAction()
        {
            Assert.Throws<ArgumentNullException>(() => MockMembers.Item.InstanceGetFunc(null));
        }

        [Fact]
        public void evaluate_func_on_gets()
        {
            object callInstance = null;
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
        public void forward_sets()
        {
            MockMembers.Item.InstanceGetFunc((o, i) => throw new InvalidOperationException()).RecordBeforeSet(out var ledger, (i, v) => (i, v))
                .Dummy();

            Sut[5] = "Test";

            Assert.Equal(new[] { (5, "Test") }, ledger);
        }
    }
}
