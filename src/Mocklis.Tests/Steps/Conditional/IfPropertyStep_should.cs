// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IfPropertyStep_should.cs">
//   SPDX-License-Identifier: MIT
//   Copyright © 2019 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.Tests.Steps.Conditional
{
    #region Using Directives

    using System;
    using System.Collections.Generic;
    using Mocklis.Core;
    using Mocklis.Tests.Interfaces;
    using Mocklis.Tests.Mocks;
    using Mocklis.Verification;
    using Xunit;

    #endregion

    public class IfPropertyStep_should
    {
        public MockMembers MockMembers { get; } = new MockMembers();
        public IProperties Sut => MockMembers;

        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        public void check_get_conditions(bool value)
        {
            MockMembers.Flag
                .If(() => value, null, s => s.Return(true))
                .Return(false);

            Assert.Equal(value, Sut.Flag);
        }

        [Fact]
        public void check_set_conditions()
        {
            IReadOnlyList<string> ledger = null;
            MockMembers.Name
                .If(null, v => v.StartsWith("A"), s => s.RecordBeforeSet(out ledger, a => a).Dummy())
                .Dummy();

            Sut.Name = "Apple";
            Sut.Name = "Banana";
            Sut.Name = "Orange";
            Sut.Name = "Avocado";
            Sut.Name = "Pear";

            Assert.Equal(new[] { "Apple", "Avocado" }, ledger);
        }

        [Fact]
        public void use_ElseBranch_if_asked()
        {
            var vg = new VerificationGroup();
            MockMembers.Name
                .If(() => true, v => true, s => s.ExpectedUsage(vg, "IfBranch", 1, 1).Join(s.ElseBranch))
                .ExpectedUsage(vg, "ElseBranch", 1, 1).Dummy();

            Sut.Name = "one";
            var _ = Sut.Name;

            vg.Assert();
        }

        [Fact]
        public void throw_when_passed_null_as_NextStep()
        {
            Assert.Throws<ArgumentNullException>(() =>
                MockMembers.Name.If(
                    () => true,
                    v => true,
                    s => ((ICanHaveNextPropertyStep<string>)s).SetNextStep((IPropertyStep<string>)null)
                )
            );
        }
    }
}
