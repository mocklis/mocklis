// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EventStepWithNext_should.cs">
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

    public class EventStepWithNext_should
    {
        private EventStepWithNext<EventHandler> EventStep { get; } = new EventStepWithNext<EventHandler>();

        private readonly EventHandler _eventHandler = (s, e) => { };

        [Fact]
        public void throw_when_null_passed_to_SetNextStep()
        {
            ICanHaveNextEventStep<EventHandler> step = EventStep;
            var exception = Assert.Throws<ArgumentNullException>(() => step.SetNextStep((IEventStep<EventHandler>)null));
            Assert.Equal("step", exception.ParamName);
        }

        [Fact]
        public void default_to_missing_for_Add()
        {
            Assert.Throws<MockMissingException>(() => EventStep.Add(MockInfo.Default, _eventHandler));
        }

        [Fact]
        public void default_to_missing_for_Remove()
        {
            Assert.Throws<MockMissingException>(() => EventStep.Remove(MockInfo.Default, _eventHandler));
        }

        [Fact]
        public void forward_to_NextStep_for_Add()
        {
            var vg = new VerificationGroup();
            EventStep.ExpectedUsage(vg, null, 1, 0).Dummy();

            EventStep.Add(MockInfo.Default, _eventHandler);

            vg.Assert();
        }

        [Fact]
        public void forward_to_NextStep_for_Remove()
        {
            var vg = new VerificationGroup();
            EventStep.ExpectedUsage(vg, null, 0, 1).Dummy();

            EventStep.Remove(MockInfo.Default, _eventHandler);

            vg.Assert();
        }
    }
}
