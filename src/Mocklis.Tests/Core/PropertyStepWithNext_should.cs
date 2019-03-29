// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PropertyStepWithNext_should.cs">
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

    public class PropertyStepWithNext_should
    {
        private PropertyStepWithNext<int> PropertyStep { get; } = new PropertyStepWithNext<int>();

        [Fact]
        public void throw_when_null_passed_to_SetNextStep()
        {
            ICanHaveNextPropertyStep<int> step = PropertyStep;
            var exception = Assert.Throws<ArgumentNullException>(() => step.SetNextStep((IPropertyStep<int>)null));
            Assert.Equal("step", exception.ParamName);
        }

        [Fact]
        public void default_to_missing_for_Get()
        {
            Assert.Throws<MockMissingException>(() => PropertyStep.Get(MockInfo.Default));
        }

        [Fact]
        public void default_to_missing_for_Set()
        {
            Assert.Throws<MockMissingException>(() => PropertyStep.Set(MockInfo.Default, 1));
        }

        [Fact]
        public void forward_to_NextStep_for_Get()
        {
            var vg = new VerificationGroup();
            PropertyStep.ExpectedUsage(vg, null, 1, 0).Dummy();

            PropertyStep.Get(MockInfo.Default);

            vg.Assert();
        }

        [Fact]
        public void forward_to_NextStep_for_Remove()
        {
            var vg = new VerificationGroup();
            PropertyStep.ExpectedUsage(vg, null, 0, 1).Dummy();

            PropertyStep.Set(MockInfo.Default, 1);

            vg.Assert();
        }
    }
}
