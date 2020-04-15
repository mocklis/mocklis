// --------------------------------------------------------------------------------------------------------------------
// <copyright file="InstanceIfMethodStep_should.cs">
//   SPDX-License-Identifier: MIT
//   Copyright © 2019-2020 Esbjörn Redmo and contributors. All rights reserved.
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

    public class InstanceIfMethodStep_should
    {
        public MockMembers MockMembers { get; } = new MockMembers();
        public IMethods Sut => MockMembers;
        public IProperties Props => MockMembers;

        [Fact]
        public void check_condition()
        {
            MockMembers.BoolProperty.Stored();
            MockMembers.FuncWithParameter.InstanceIf((inst, i) => ((IProperties)inst).BoolProperty, s => s.Func(a => a * 2)).Func(a => a * 5);

            var v1 = Sut.FuncWithParameter(4);
            Props.BoolProperty = true;
            var v2 = Sut.FuncWithParameter(4);

            Assert.Equal(20, v1);
            Assert.Equal(8, v2);
        }

        [Fact]
        public void use_ElseBranch_if_asked()
        {
            var vg = new VerificationGroup();
            MockMembers.FuncWithParameter
                .InstanceIf((inst, i) => true, s => s.ExpectedUsage(vg, "IfBranch", 1).Join(s.ElseBranch))
                .ExpectedUsage(vg, "ElseBranch", 1);

            Sut.FuncWithParameter(42);

            vg.Assert();
        }

        [Fact]
        public void check_condition_in_no_parameter_case()
        {
            MockMembers.BoolProperty.Stored();
            MockMembers.SimpleFunc.InstanceIf((inst, i) => ((IProperties)inst).BoolProperty, s => s.Return(99)).Return(10);

            var v1 = Sut.SimpleFunc();
            Props.BoolProperty = true;
            var v2 = Sut.SimpleFunc();

            Assert.Equal(10, v1);
            Assert.Equal(99, v2);
        }

        [Fact]
        public void check_condition_in_no_return_value_case()
        {
            MockMembers.BoolProperty.Stored();
            IReadOnlyList<int>? ifBranchLedger = null;

            MockMembers.ActionWithParameter
                .InstanceIf((inst, i) => ((IProperties)inst).BoolProperty, s => s.RecordBeforeCall(out ifBranchLedger))
                .RecordBeforeCall(out var elseBranchLedger);

            Sut.ActionWithParameter(1);
            Sut.ActionWithParameter(6);
            Props.BoolProperty = true;
            Sut.ActionWithParameter(-3);

            Assert.Equal(new[] { 1, 6 }, elseBranchLedger);
            Assert.Equal(new[] { -3 }, ifBranchLedger);
        }

        [Fact]
        public void use_ElseBranch_if_asked_in_no_parameter_case()
        {
            var vg = new VerificationGroup();
            MockMembers.SimpleFunc
                .InstanceIf(inst => true, s => s.ExpectedUsage(vg, "IfBranch", 1).Join(s.ElseBranch))
                .ExpectedUsage(vg, "ElseBranch", 1);

            Sut.SimpleFunc();

            vg.Assert();
        }

        [Fact]
        public void CallBaseIfConditionsAreNotMet()
        {
            var group = new VerificationGroup();
            MockMembers.SimpleAction
                .InstanceIf(i => false, b => b.ExpectedUsage(group, null, 0))
                .ExpectedUsage(group, null, 1);

            Sut.SimpleAction();

            group.Assert();
        }
    }
}
