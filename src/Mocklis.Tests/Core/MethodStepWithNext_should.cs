// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MethodStepWithNext_should.cs">
//   SPDX-License-Identifier: MIT
//   Copyright © 2019 Esbjörn Redmo and contributors. All rights reserved.
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
            var exception = Assert.Throws<ArgumentNullException>(() => step.SetNextStep((IMethodStep<int, int>)null));
            Assert.Equal("step", exception.ParamName);
        }

        [Fact]
        public void default_to_missing_for_Call()
        {
            Assert.Throws<MockMissingException>(() => MethodStep.Call(MockInfo.Default, 1));
        }

        [Fact]
        public void Call()
        {
            var vg = new VerificationGroup();
            MethodStep.ExpectedUsage(vg, null, 1).Dummy();

            MethodStep.Call(MockInfo.Default, 1);

            vg.Assert();
        }
    }
}
