// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EventMock_SetNextStep_should.cs">
//   SPDX-License-Identifier: MIT
//   Copyright © 2019-2020 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.Core
{
    #region Using Directives

    using System;
    using Mocklis.Mocks;
    using Xunit;

    #endregion

    public class EventMock_SetNextStep_should
    {
        private readonly EventMock<EventHandler> _eventMock;

        public EventMock_SetNextStep_should()
        {
            _eventMock = new EventMock<EventHandler>(new object(), "ClassName", "InterfaceName", "MemberName", "MockName", Strictness.Lenient);
        }

        [Fact]
        public void require_step()
        {
            var exception = Assert.Throws<ArgumentNullException>(() =>
                ((ICanHaveNextEventStep<EventHandler>)_eventMock).SetNextStep((IEventStep<EventHandler>)null!));
            Assert.Equal("step", exception.ParamName);
        }

        [Fact]
        public void return_new_step()
        {
            var newStep = new MockEventStep<EventHandler>();
            var returnedStep = ((ICanHaveNextEventStep<EventHandler>)_eventMock).SetNextStep(newStep);
            Assert.Same(newStep, returnedStep);
        }

        [Fact]
        public void set_step_used_by_Add()
        {
            bool called = false;
            var newStep = new MockEventStep<EventHandler>();
            newStep.Add.Action(_ => { called = true; });
            ((ICanHaveNextEventStep<EventHandler>)_eventMock).SetNextStep(newStep);
            _eventMock.Add((sender, e) => { });
            Assert.True(called);
        }

        [Fact]
        public void set_step_used_by_Remove()
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
