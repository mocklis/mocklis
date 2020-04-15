// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IndexerStepWithNext_should.cs">
//   SPDX-License-Identifier: MIT
//   Copyright © 2019-2020 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.Core
{
    #region Using Directives

    using System;
    using Mocklis.Helpers;
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
            var exception = Assert.Throws<ArgumentNullException>(() => step.SetNextStep((IIndexerStep<int, string>)null!));
            Assert.Equal("step", exception.ParamName);
        }

        [Fact]
        public void do_nothing_if_nextstep_missing_for_Get_lenient()
        {
            IndexerStep.Get(MockInfo.Lenient, 1);
        }

        [Fact]
        public void do_nothing_if_nextstep_missing_for_Get_strict()
        {
            IndexerStep.Get(MockInfo.Strict, 1);
        }

        [Fact]
        public void throw_if_nextstep_missing_for_Get_verystrict()
        {
            Assert.Throws<MockMissingException>(() => IndexerStep.Get(MockInfo.VeryStrict, 1));
        }

        [Fact]
        public void do_nothing_if_nextstep_missing_for_Set_lenient()
        {
            IndexerStep.Set(MockInfo.Lenient, 1, "one");
        }

        [Fact]
        public void do_nothing_if_nextstep_missing_for_Set_strict()
        {
            IndexerStep.Set(MockInfo.Strict, 1, "one");
        }

        [Fact]
        public void throw_if_nextstep_missing_for_Set_verystrict()
        {
            Assert.Throws<MockMissingException>(() => IndexerStep.Set(MockInfo.VeryStrict, 1, "one"));
        }

        [Fact]
        public void forward_to_NextStep_for_Get()
        {
            var vg = new VerificationGroup();
            IndexerStep.ExpectedUsage(vg, null, 1, 0);

            IndexerStep.Get(MockInfo.Lenient, 1);

            vg.Assert();
        }

        [Fact]
        public void forward_to_NextStep_for_Remove()
        {
            var vg = new VerificationGroup();
            IndexerStep.ExpectedUsage(vg, null, 0, 1);

            IndexerStep.Set(MockInfo.Lenient, 1, "one");

            vg.Assert();
        }
    }
}
