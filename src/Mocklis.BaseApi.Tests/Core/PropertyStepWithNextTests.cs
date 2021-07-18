// --------------------------------------------------------------------------------------------------------------------(ITestOutputHelper testOutputHelper)
// <copyright file="PropertyStepWithNextTests.cs">
//   SPDX-License-Identifier: MIT
//   Copyright © 2019-2021 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.Core
{
    #region Using Directives

    using System;
    using Mocklis.Helpers;
    using Mocklis.Verification;
    using Xunit;
    using Xunit.Abstractions;

    #endregion

    public class PropertyStepWithNextTests : XUnitTestClass
    {
        public PropertyStepWithNextTests(ITestOutputHelper testOutputHelper) : base(testOutputHelper)
        {
        }

        private PropertyStepWithNext<int> PropertyStep { get; } = new PropertyStepWithNext<int>();

        [Fact]
        public void SetNextStepRequiresStep()
        {
            ICanHaveNextPropertyStep<int> step = PropertyStep;
            var exception = Assert.Throws<ArgumentNullException>(() => step.SetNextStep((IPropertyStep<int>)null!));
            Assert.Equal("step", exception.ParamName);
        }

        [Fact]
        public void NextStepMissingForGetInLenientMode()
        {
            Log("In lenient mode we don't need a next step so a call to 'Get' should not throw.");
            PropertyStep.Get(MockInfo.Lenient);
        }

        [Fact]
        public void NextStepMissingForGetInStrictMode()
        {
            Log("In strict mode we don't need a next step so a call to 'Get' should not throw.");
            PropertyStep.Get(MockInfo.Strict);
        }

        [Fact]
        public void NextStepMissingForGetInVeryStrictMode()
        {
            Log("In very strict mode we need a next step so a call to 'Get' should throw a 'mock missing' exception.");
            Assert.Throws<MockMissingException>(() => PropertyStep.Get(MockInfo.VeryStrict));
        }

        [Fact]
        public void NextStepMissingForSetInLenientMode()
        {
            Log("In lenient mode we don't need a next step so a call to 'Set' should not throw.");
            PropertyStep.Set(MockInfo.Lenient, 1);
        }

        [Fact]
        public void NextStepMissingForSetInStrictMode()
        {
            Log("In strict mode we don't need a next step so a call to 'Set' should not throw.");
            PropertyStep.Set(MockInfo.Strict, 1);
        }

        [Fact]
        public void NextStepMissingForSetInVeryStrictMode()
        {
            Log("In very strict mode we need a next step so a call to 'Set' should throw a 'mock missing' exception.");
            Assert.Throws<MockMissingException>(() => PropertyStep.Set(MockInfo.VeryStrict, 1));
        }

        [Fact]
        public void GetForwardsToNextStep()
        {
            var vg = new VerificationGroup();
            PropertyStep.ExpectedUsage(vg, null, expectedNumberOfGets: 1, expectedNumberOfSets: 0);

            PropertyStep.Get(MockInfo.Lenient);

            vg.Assert();
        }

        [Fact]
        public void SetForwardsToNextStep()
        {
            var vg = new VerificationGroup();
            PropertyStep.ExpectedUsage(vg, null, expectedNumberOfGets: 0, expectedNumberOfSets: 1);

            PropertyStep.Set(MockInfo.Lenient, 1);

            vg.Assert();
        }
    }
}
