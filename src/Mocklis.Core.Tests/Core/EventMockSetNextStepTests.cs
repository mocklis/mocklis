// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EventMockSetNextStepTests.cs">
//   SPDX-License-Identifier: MIT
//   Copyright © 2019-2023 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.Core
{
    #region Using Directives

    using System;
    using Mocklis.Mocks;
    using Xunit;

    #endregion

    public class EventMockSetNextStepTests
    {
        private readonly EventMock<EventHandler> _eventMock;

        public EventMockSetNextStepTests()
        {
            _eventMock = new EventMock<EventHandler>(new object(), "ClassName", "InterfaceName", "MemberName", "MockName", Strictness.Lenient);
        }

        [Fact]
        public void RequireStep()
        {
            var exception = Assert.Throws<ArgumentNullException>(() =>
                ((ICanHaveNextEventStep<EventHandler>)_eventMock).SetNextStep((IEventStep<EventHandler>)null!));
            Assert.Equal("step", exception.ParamName);
        }

        [Fact]
        public void ReturnNewStep()
        {
            var newStep = new MockEventStep<EventHandler>();
            var returnedStep = ((ICanHaveNextEventStep<EventHandler>)_eventMock).SetNextStep(newStep);
            Assert.Same(newStep, returnedStep);
        }

        [Fact]
        public void SetStepUsedByAdd()
        {
            bool called = false;
            var newStep = new MockEventStep<EventHandler>();
            newStep.Add.Action(_ => { called = true; });
            ((ICanHaveNextEventStep<EventHandler>)_eventMock).SetNextStep(newStep);
            _eventMock.Add((sender, e) => { });
            Assert.True(called);
        }

        [Fact]
        public void SetStepUsedByRemove()
        {
            bool called = false;
            var newStep = new MockEventStep<EventHandler>();
            newStep.Remove.Action(_ => { called = true; });
            ((ICanHaveNextEventStep<EventHandler>)_eventMock).SetNextStep(newStep);
            _eventMock.Remove((sender, e) => { });
            Assert.True(called);
        }
    }
}
