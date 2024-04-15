// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MethodStepWithNextTests.cs">
//   SPDX-License-Identifier: MIT
//   Copyright © 2019-2024 Esbjörn Redmo and contributors. All rights reserved.
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

    public class MethodStepWithNextTests : XUnitTestClass
    {
        public MethodStepWithNextTests(ITestOutputHelper testOutputHelper) : base(testOutputHelper)
        {
        }

        private MethodStepWithNext<int, int> MethodStep { get; } = new MethodStepWithNext<int, int>();

        [Fact]
        public void SetNextStepRequiresStep()
        {
            ICanHaveNextMethodStep<int, int> step = MethodStep;
            var exception = Assert.Throws<ArgumentNullException>(() => step.SetNextStep((IMethodStep<int, int>)null!));
            Assert.Equal("step", exception.ParamName);
        }

        [Fact]
        public void NextStepMissingForCallInLenientMode()
        {
            Log("In lenient mode we don't need a next step so a call to 'Call' should not throw.");
            MethodStep.Call(MockInfo.Lenient, 1);
        }

        [Fact]
        public void NextStepMissingForCallInStrictMode()
        {
            Log("In strict mode we don't need a next step so a call to 'Call' should not throw.");
            MethodStep.Call(MockInfo.Strict, 1);
        }

        [Fact]
        public void NextStepMissingForCallInVeryStrictMode()
        {
            Log("In very strict mode we need a next step so a call to 'Call' should throw a 'mock missing' exception.");
            Assert.Throws<MockMissingException>(() => MethodStep.Call(MockInfo.VeryStrict, 1));
        }

        [Fact]
        public void CallForwardsToNextStep()
        {
            var vg = new VerificationGroup();
            MethodStep.ExpectedUsage(vg, null, expectedNumberOfCalls: 1);

            MethodStep.Call(MockInfo.Lenient, 1);

            vg.Assert();
        }

        [Fact]
        public void ClearRemovesAnyProgramming()
        {
            var vg = new VerificationGroup();
            MethodStep.ExpectedUsage(vg, null, expectedNumberOfCalls: 0);

            MethodStep.Clear();

            MethodStep.Call(MockInfo.Lenient, 5);

            vg.Assert();
        }
    }
}
