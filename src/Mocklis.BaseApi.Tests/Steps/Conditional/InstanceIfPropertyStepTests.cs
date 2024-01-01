// --------------------------------------------------------------------------------------------------------------------
// <copyright file="InstanceIfPropertyStepTests.cs">
//   SPDX-License-Identifier: MIT
//   Copyright © 2019-2024 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.Steps.Conditional
{
    #region Using Directives

    using System.Collections.Generic;
    using Mocklis.Interfaces;
    using Mocklis.Mocks;
    using Mocklis.Verification;
    using Xunit;

    #endregion

    public class InstanceIfPropertyStepTests
    {
        public MockMembers MockMembers { get; } = new MockMembers();
        public IProperties Sut => MockMembers;

        [Fact]
        public void CheckGetConditions()
        {
            MockMembers.BoolProperty.Stored();
            MockMembers.StringProperty.InstanceIf(inst => ((IProperties)inst).BoolProperty, null, s => s.Return("Name"));

            var v1 = Sut.StringProperty;
            Sut.BoolProperty = true;
            var v3 = Sut.StringProperty;

            Assert.Null(v1);
            Assert.Equal("Name", v3);
        }

        [Fact]
        public void CheckSetConditions()
        {
            MockMembers.BoolProperty.Stored();
            IReadOnlyList<string>? ledger = null;
            MockMembers.StringProperty
                .InstanceIf(null, (inst, v) => ((IProperties)inst).BoolProperty, s => s.RecordBeforeSet(out ledger));

            Sut.StringProperty = "off";
            Sut.BoolProperty = true;
            Sut.StringProperty = "on";

            Assert.Equal(new[] { "on" }, ledger);
        }

        [Fact]
        public void UseElseBranchIfAsked()
        {
            var vg = new VerificationGroup();
            MockMembers.StringProperty
                .InstanceIf(inst => true, (inst, v) => true, s => s.ExpectedUsage(vg, "IfBranch", 1, 1).Join(s.ElseBranch))
                .ExpectedUsage(vg, "ElseBranch", 1, 1);

            Sut.StringProperty = "one";
            var _ = Sut.StringProperty;

            vg.Assert();
        }
    }
}
