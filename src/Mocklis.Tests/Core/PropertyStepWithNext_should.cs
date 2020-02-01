// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PropertyStepWithNext_should.cs">
//   SPDX-License-Identifier: MIT
//   Copyright © 2019-2020 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.Tests.Core
{
    #region Using Directives

    using System;
    using Mocklis.Core;
    using Mocklis.Tests.Helpers;
    using Mocklis.Verification;
    using Xunit;

    #endregion

    public class PropertyStepWithNext_should
    {
        private PropertyStepWithNext<int> PropertyStep { get; } = new PropertyStepWithNext<int>();

        [Fact]
        public void throw_when_null_passed_to_SetNextStep()
        {
            ICanHaveNextPropertyStep<int> step = PropertyStep;
            var exception = Assert.Throws<ArgumentNullException>(() => step.SetNextStep((IPropertyStep<int>)null!));
            Assert.Equal("step", exception.ParamName);
        }

        [Fact]
        public void do_nothing_if_nextstep_missing_for_Get_lenient()
        {
            PropertyStep.Get(MockInfo.Lenient);
        }

        [Fact]
        public void do_nothing_if_nextstep_missing_for_Get_strict()
        {
            PropertyStep.Get(MockInfo.Strict);
        }

        [Fact]
        public void throw_if_nextstep_missing_for_Get_verystrict()
        {
            Assert.Throws<MockMissingException>(() => PropertyStep.Get(MockInfo.VeryStrict));
        }

        [Fact]
        public void do_nothing_if_nextstep_missing_for_Set_lenient()
        {
            PropertyStep.Set(MockInfo.Lenient, 1);
        }

        [Fact]
        public void do_nothing_if_nextstep_missing_for_Set_strict()
        {
            PropertyStep.Set(MockInfo.Strict, 1);
        }

        [Fact]
        public void throw_if_nextstep_missing_for_Set_verystrict()
        {
            Assert.Throws<MockMissingException>(() => PropertyStep.Set(MockInfo.VeryStrict, 1));
        }

        [Fact]
        public void forward_to_NextStep_for_Get()
        {
            var vg = new VerificationGroup();
            PropertyStep.ExpectedUsage(vg, null, 1, 0);

            PropertyStep.Get(MockInfo.Lenient);

            vg.Assert();
        }

        [Fact]
        public void forward_to_NextStep_for_Remove()
        {
            var vg = new VerificationGroup();
            PropertyStep.ExpectedUsage(vg, null, 0, 1);

            PropertyStep.Set(MockInfo.Lenient, 1);

            vg.Assert();
        }
    }
}
