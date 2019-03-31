// --------------------------------------------------------------------------------------------------------------------
// <copyright file="InstanceIfMethodStep_should.cs">
//   SPDX-License-Identifier: MIT
//   Copyright © 2019 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.Tests.Steps.Conditional
{
    #region Using Directives

    using Mocklis.Tests.Interfaces;
    using Mocklis.Tests.Mocks;
    using Mocklis.Verification;
    using Xunit;

    #endregion

    public class InstanceIfMethodStep_should
    {
        public MockMembers MockMembers { get; } = new MockMembers();
        public IMethods Sut => MockMembers;
        public IProperties Props => MockMembers;

        [Fact]
        public void check_condition()
        {
            MockMembers.Flag.Stored();
            MockMembers.DoStuff.InstanceIf((inst, i) => ((IProperties)inst).Flag, s => s.Func(a => a * 2)).Func(a => a * 5);

            var v1 = Sut.DoStuff(4);
            Props.Flag = true;
            var v2 = Sut.DoStuff(4);

            Assert.Equal(20, v1);
            Assert.Equal(8, v2);
        }

        [Fact]
        public void use_ElseBranch_if_asked()
        {
            var vg = new VerificationGroup();
            MockMembers.DoStuff
                .InstanceIf((inst, i) => true, s => s.ExpectedUsage(vg, "IfBranch", 1).Join(s.ElseBranch))
                .ExpectedUsage(vg, "ElseBranch", 1).Dummy();

            Sut.DoStuff(42);

            vg.Assert();
        }
    }
}
