// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EventStepWithNextTests.cs">
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
    using Xunit.Abstractions;

    #endregion

    public class EventStepWithNextTests : XUnitTestClass
    {
        public EventStepWithNextTests(ITestOutputHelper testOutputHelper) : base(testOutputHelper)
        {
        }

        private EventStepWithNext<EventHandler> EventStep { get; } = new EventStepWithNext<EventHandler>();

        private readonly EventHandler _eventHandler = (s, e) => { };

        [Fact]
        public void SetNextStepRequiresStep()
        {
            ICanHaveNextEventStep<EventHandler> step = EventStep;
            var exception = Assert.Throws<ArgumentNullException>(() => step.SetNextStep((IEventStep<EventHandler>)null!));
            Assert.Equal("step", exception.ParamName);
        }

        [Fact]
        public void NextStepMissingForAddInLenientMode()
        {
            Log("In lenient mode we don't need a next step so a call to 'Add' should not throw.");
            EventStep.Add(MockInfo.Lenient, _eventHandler);
        }

        [Fact]
        public void NextStepMissingForAddInStrictMode()
        {
            Log("In strict mode we don't need a next step so a call to 'Add' should not throw.");
            EventStep.Add(MockInfo.Strict, _eventHandler);
        }

        [Fact]
        public void NextStepMissingForAddInVeryStrictMode()
        {
            Log("In very strict mode we need a next step so a call to 'Add' should throw a 'mock missing' exception.");
            Assert.Throws<MockMissingException>(() => EventStep.Add(MockInfo.VeryStrict, _eventHandler));
        }

        [Fact]
        public void NextStepMissingForRemoveInLenientMode()
        {
            Log("In lenient mode we don't need a next step so a call to 'Remove' should not throw.");
            EventStep.Remove(MockInfo.Lenient, _eventHandler);
        }

        [Fact]
        public void NextStepMissingForRemoveInStrictMode()
        {
            Log("In strict mode we don't need a next step so a call to 'Remove' should not throw.");
            EventStep.Remove(MockInfo.Strict, _eventHandler);
        }

        [Fact]
        public void NextStepMissingForRemoveInVeryStrictMode()
        {
            Log("In very strict mode we need a next step so a call to 'Remove' should throw a 'mock missing' exception.");
            Assert.Throws<MockMissingException>(() => EventStep.Remove(MockInfo.VeryStrict, _eventHandler));
        }

        [Fact]
        public void AddForwardsToNextStep()
        {
            var vg = new VerificationGroup();
            EventStep.ExpectedUsage(vg, null, expectedNumberOfAdds: 1, expectedNumberOfRemoves: 0);

            EventStep.Add(MockInfo.Lenient, _eventHandler);

            vg.Assert();
        }

        [Fact]
        public void RemoveForwardsToNextStep()
        {
            var vg = new VerificationGroup();
            EventStep.ExpectedUsage(vg, null, expectedNumberOfAdds: 0, expectedNumberOfRemoves: 1);

            EventStep.Remove(MockInfo.Lenient, _eventHandler);

            vg.Assert();
        }
    }
}
