// --------------------------------------------------------------------------------------------------------------------
// <copyright file="InstanceIfPropertyStep_should.cs">
//   SPDX-License-Identifier: MIT
//   Copyright © 2019 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.Tests.Steps.Conditional
{
    #region Using Directives

    using System.Collections.Generic;
    using Mocklis.Tests.Interfaces;
    using Mocklis.Tests.Mocks;
    using Mocklis.Verification;
    using Xunit;

    #endregion

    public class InstanceIfPropertyStep_should
    {
        public MockMembers MockMembers { get; } = new MockMembers();
        public IProperties Sut => MockMembers;

        [Fact]
        public void check_get_conditions()
        {
            MockMembers.Flag.Stored();
            MockMembers.Name.InstanceIf(inst => ((IProperties)inst).Flag, null, s => s.Return("Name")).Dummy();

            var v1 = Sut.Name;
            Sut.Flag = true;
            var v3 = Sut.Name;

            Assert.Null(v1);
            Assert.Equal("Name", v3);
        }

        [Fact]
        public void check_set_conditions()
        {
            MockMembers.Flag.Stored();
            IReadOnlyList<string> ledger = null;
            MockMembers.Name.InstanceIf(null, (inst, v) => ((IProperties)inst).Flag, s => s.RecordBeforeSet(out ledger, a => a).Dummy()).Dummy();

            Sut.Name = "off";
            Sut.Flag = true;
            Sut.Name = "on";

            Assert.Equal(new[] { "on" }, ledger);
        }

        [Fact]
        public void use_ElseBranch_if_asked()
        {
            var vg = new VerificationGroup();
            MockMembers.Name
                .InstanceIf(inst => true, (inst, v) => true, s => s.ExpectedUsage(vg, "IfBranch", 1, 1).Join(s.ElseBranch))
                .ExpectedUsage(vg, "ElseBranch", 1, 1).Dummy();

            Sut.Name = "one";
            var _ = Sut.Name;

            vg.Assert();
        }
    }
}
