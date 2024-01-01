// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IfIndexerStepTests.cs">
//   SPDX-License-Identifier: MIT
//   Copyright © 2019-2024 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.Steps.Conditional
{
    #region Using Directives

    using System;
    using Mocklis.Core;
    using Mocklis.Helpers;
    using Mocklis.Interfaces;
    using Mocklis.Mocks;
    using Mocklis.Steps.Stored;
    using Mocklis.Verification;
    using Xunit;

    #endregion

    public class IfIndexerStepTests
    {
        public MockMembers MockMembers { get; } = new MockMembers();
        public IIndexers Sut => MockMembers;

        [Fact]
        public void RequireBranch()
        {
            Assert.Throws<ArgumentNullException>(() => MockMembers.Item.If(_ => true, null, null!));
        }

        [Fact]
        public void CheckGetConditions()
        {
            StoredAsDictionaryIndexerStep<int, string>? indexerStore = null;
            MockMembers.Item.If(i => i == 3, null, s => s.StoredAsDictionary(out indexerStore));

            indexerStore![1] = "one";
            indexerStore[3] = "three";

            var v1 = Sut[1];
            var v3 = Sut[3];

            Assert.Null(v1);
            Assert.Equal("three", v3);
        }

        [Fact]
        public void CheckSetConditions()
        {
            StoredAsDictionaryIndexerStep<int, string>? tmpIndexerStore = null;
            MockMembers.Item.If(null, (i, v) => i == 3 || v == "two", s => s.StoredAsDictionary(out tmpIndexerStore));

            var indexerStore = tmpIndexerStore.AssertNotNull();

            Sut[1] = "one";
            Sut[2] = "two";
            Sut[3] = "three";

            Assert.Null(indexerStore[1]);
            Assert.Equal("two", indexerStore[2]);
            Assert.Equal("three", indexerStore[3]);
        }

        [Fact]
        public void UseElseBranchIfAsked()
        {
            var vg = new VerificationGroup();
            MockMembers.Item
                .If(i => true, (i, v) => true, s => s.ExpectedUsage(vg, "IfBranch", 1, 1).Join(s.ElseBranch))
                .ExpectedUsage(vg, "ElseBranch", 1, 1);

            Sut[1] = "one";
            var _ = Sut[1];

            vg.Assert();
        }

        [Fact]
        public void ThrowWhenPassedNullAsNextStep()
        {
            Assert.Throws<ArgumentNullException>(() =>
                MockMembers.Item.If(
                    i => true,
                    (i, v) => true,
                    s => ((ICanHaveNextIndexerStep<int, string>)s).SetNextStep((IIndexerStep<int, string>)null!)
                )
            );
        }
    }
}
