// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IndexerStepWithNext_should.cs">
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

    public class IndexerStepWithNext_should
    {
        private IndexerStepWithNext<int, string> IndexerStep { get; } = new IndexerStepWithNext<int, string>();

        [Fact]
        public void throw_when_null_passed_to_SetNextStep()
        {
            ICanHaveNextIndexerStep<int, string> step = IndexerStep;
            var exception = Assert.Throws<ArgumentNullException>(() => step.SetNextStep((IIndexerStep<int, string>)null));
            Assert.Equal("step", exception.ParamName);
        }

        [Fact]
        public void default_to_missing_for_Get()
        {
            Assert.Throws<MockMissingException>(() => IndexerStep.Get(MockInfo.Default, 1));
        }

        [Fact]
        public void default_to_missing_for_Set()
        {
            Assert.Throws<MockMissingException>(() => IndexerStep.Set(MockInfo.Default, 1, "one"));
        }

        [Fact]
        public void forward_to_NextStep_for_Get()
        {
            var vg = new VerificationGroup();
            IndexerStep.ExpectedUsage(vg, null, 1, 0).Dummy();

            IndexerStep.Get(MockInfo.Default, 1);

            vg.Assert();
        }

        [Fact]
        public void forward_to_NextStep_for_Remove()
        {
            var vg = new VerificationGroup();
            IndexerStep.ExpectedUsage(vg, null, 0, 1).Dummy();

            IndexerStep.Set(MockInfo.Default, 1, "one");

            vg.Assert();
        }
    }
}
