// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IfPropertyStepTests.cs">
//   SPDX-License-Identifier: MIT
//   Copyright © 2019-2020 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.Steps.Conditional
{
    #region Using Directives

    using System;
    using System.Collections.Generic;
    using Mocklis.Core;
    using Mocklis.Interfaces;
    using Mocklis.Mocks;
    using Mocklis.Verification;
    using Xunit;

    #endregion

    public class IfPropertyStepTests
    {
        public MockMembers MockMembers { get; } = new MockMembers();
        public IProperties Sut => MockMembers;

        [Fact]
        public void RequireBranch()
        {
            Assert.Throws<ArgumentNullException>(() => MockMembers.BoolProperty.If(() => true, null, null!));
        }

        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        public void CheckGetConditions(bool value)
        {
            MockMembers.BoolProperty
                .If(() => value, null, s => s.Return(true))
                .Return(false);

            Assert.Equal(value, Sut.BoolProperty);
        }

        [Fact]
        public void CheckSetConditions()
        {
            IReadOnlyList<string>? ledger = null;
            MockMembers.StringProperty.If(null, v => v.StartsWith("A"), s => s.RecordBeforeSet(out ledger));

            Sut.StringProperty = "Apple";
            Sut.StringProperty = "Banana";
            Sut.StringProperty = "Orange";
            Sut.StringProperty = "Avocado";
            Sut.StringProperty = "Pear";

            Assert.Equal(new[] { "Apple", "Avocado" }, ledger);
        }

        [Fact]
        public void UseElseBranchIfAsked()
        {
            var vg = new VerificationGroup();
            MockMembers.StringProperty
                .If(() => true, v => true, s => s.ExpectedUsage(vg, "IfBranch", 1, 1).Join(s.ElseBranch))
                .ExpectedUsage(vg, "ElseBranch", 1, 1);

            Sut.StringProperty = "one";
            var _ = Sut.StringProperty;

            vg.Assert();
        }

        [Fact]
        public void ThrowWhenPassedNullAsNextStep()
        {
            Assert.Throws<ArgumentNullException>(() =>
                MockMembers.StringProperty.If(
                    () => true,
                    v => true,
                    s => ((ICanHaveNextPropertyStep<string>)s).SetNextStep((IPropertyStep<string>)null!)
                )
            );
        }
    }
}
