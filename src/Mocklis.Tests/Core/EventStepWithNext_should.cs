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
        public void do_nothing_if_nextstep_missing_for_Add_lenient()
        {
            EventStep.Add(MockInfo.Lenient, _eventHandler);
        }

        [Fact]
        public void do_nothing_if_nextstep_missing_for_Add_strict()
        {
            EventStep.Add(MockInfo.Strict, _eventHandler);
        }

        [Fact]
        public void throw_if_nextstep_missing_for_Add_verystrict()
        {
            Assert.Throws<MockMissingException>(() => EventStep.Add(MockInfo.VeryStrict, _eventHandler));
        }

        [Fact]
        public void do_nothing_if_nextstep_missing_for_Remove_lenient()
        {
            EventStep.Remove(MockInfo.Lenient, _eventHandler);
        }

        [Fact]
        public void do_nothing_if_nextstep_missing_for_Remove_strict()
        {
            EventStep.Remove(MockInfo.Strict, _eventHandler);
        }

        [Fact]
        public void throw_if_nextstep_missing_for_Remove_verystrict()
        {
            Assert.Throws<MockMissingException>(() => EventStep.Remove(MockInfo.VeryStrict, _eventHandler));
        }

        [Fact]
        public void forward_to_NextStep_for_Add()
        {
            var vg = new VerificationGroup();
            EventStep.ExpectedUsage(vg, null, 1, 0);

            EventStep.Add(MockInfo.Lenient, _eventHandler);

            vg.Assert();
        }

        [Fact]
        public void forward_to_NextStep_for_Remove()
        {
            var vg = new VerificationGroup();
            EventStep.ExpectedUsage(vg, null, 0, 1);

            EventStep.Remove(MockInfo.Lenient, _eventHandler);

            vg.Assert();
        }
    }
}
