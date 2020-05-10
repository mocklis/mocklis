// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IndexerStepWithNextTests.cs">
//   SPDX-License-Identifier: MIT
//   Copyright © 2019-2020 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.Core
{
    #region Using Directives

    using System;
    using JetBrains.Annotations;
    using Mocklis.Helpers;
    using Mocklis.Verification;
    using Xunit;
    using Xunit.Abstractions;

    #endregion

    public class IndexerStepWithNextTests : XUnitTestClass
    {
        public IndexerStepWithNextTests([NotNull] ITestOutputHelper testOutputHelper) : base(testOutputHelper)
        {
        }

        private IndexerStepWithNext<int, string> IndexerStep { get; } = new IndexerStepWithNext<int, string>();

        [Fact]
        public void SetNextStepRequiresStep()
        {
            ICanHaveNextIndexerStep<int, string> step = IndexerStep;
            var exception = Assert.Throws<ArgumentNullException>(() => step.SetNextStep((IIndexerStep<int, string>)null!));
            Assert.Equal("step", exception.ParamName);
        }

        [Fact]
        public void NextStepMissingForGetInLenientMode()
        {
            Log("In lenient mode we don't need a next step so a call to 'Get' should not throw.");
            IndexerStep.Get(MockInfo.Lenient, 1);
        }

        [Fact]
        public void NextStepMissingForGetInStrictMode()
        {
            Log("In strict mode we don't need a next step so a call to 'Get' should not throw.");
            IndexerStep.Get(MockInfo.Strict, 1);
        }

        [Fact]
        public void NextStepMissingForGetInVeryStrictMode()
        {
            Log("In very strict mode we need a next step so a call to 'Get' should throw a 'mock missing' exception.");
            Assert.Throws<MockMissingException>(() => IndexerStep.Get(MockInfo.VeryStrict, 1));
        }

        [Fact]
        public void NextStepMissingForSetInLenientMode()
        {
            Log("In lenient mode we don't need a next step so a call to 'Set' should not throw.");
            IndexerStep.Set(MockInfo.Lenient, 1, "one");
        }

        [Fact]
        public void NextStepMissingForSetInStrictMode()
        {
            Log("In strict mode we don't need a next step so a call to 'Set' should not throw.");
            IndexerStep.Set(MockInfo.Strict, 1, "one");
        }

        [Fact]
        public void NextStepMissingForSetInVeryStrictMode()
        {
            Log("In very strict mode we need a next step so a call to 'Set' should throw a 'mock missing' exception.");
            Assert.Throws<MockMissingException>(() => IndexerStep.Set(MockInfo.VeryStrict, 1, "one"));
        }

        [Fact]
        public void GetForwardsToNextStep()
        {
            var vg = new VerificationGroup();
            IndexerStep.ExpectedUsage(vg, null, expectedNumberOfGets:1, expectedNumberOfSets:0);

            IndexerStep.Get(MockInfo.Lenient, 1);

            vg.Assert();
        }

        [Fact]
        public void SetForwardsToNextStep()
        {
            var vg = new VerificationGroup();
            IndexerStep.ExpectedUsage(vg, null, expectedNumberOfGets:0, expectedNumberOfSets:1);

            IndexerStep.Set(MockInfo.Lenient, 1, "one");

            vg.Assert();
        }


    }
}
