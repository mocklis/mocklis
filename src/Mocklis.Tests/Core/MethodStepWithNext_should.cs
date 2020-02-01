// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MethodStepWithNext_should.cs">
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

    public class MethodStepWithNext_should
    {
        private MethodStepWithNext<int, int> MethodStep { get; } = new MethodStepWithNext<int, int>();

        [Fact]
        public void throw_when_null_passed_to_SetNextStep()
        {
            ICanHaveNextMethodStep<int, int> step = MethodStep;
            var exception = Assert.Throws<ArgumentNullException>(() => step.SetNextStep((IMethodStep<int, int>)null!));
            Assert.Equal("step", exception.ParamName);
        }

        [Fact]
        public void do_nothing_if_nextstep_missing_for_Call_lenient()
        {
            MethodStep.Call(MockInfo.Lenient, 1);
        }

        [Fact]
        public void do_nothing_if_nextstep_missing_for_Call_strict()
        {
            MethodStep.Call(MockInfo.Strict, 1);
        }

        [Fact]
        public void throw_if_nextstep_missing_for_Call_verystrict()
        {
            Assert.Throws<MockMissingException>(() => MethodStep.Call(MockInfo.VeryStrict, 1));
        }

        [Fact]
        public void Call()
        {
            var vg = new VerificationGroup();
            MethodStep.ExpectedUsage(vg, null, 1);

            MethodStep.Call(MockInfo.Lenient, 1);

            vg.Assert();
        }
    }
}
