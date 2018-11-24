// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EventMock_SetNextStep_should.cs">
//   Copyright © 2018 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.Core.Tests.Core
{
    #region Using Directives

    using System;
    using Mocklis.Core.Tests.Mocks;
    using Xunit;

    #endregion

    public class EventMock_SetNextStep_should
    {
        private readonly EventMock<EventHandler> _propertyMock;

        public EventMock_SetNextStep_should()
        {
            _propertyMock = new EventMock<EventHandler>(new object(), "ClassName", "InterfaceName", "MemberName", "MockName");
        }

        [Fact]
        public void require_step()
        {
            var exception = Assert.Throws<ArgumentNullException>(() =>
                _propertyMock.SetNextStep((IEventStep<EventHandler>)null));
            Assert.Equal("step", exception.ParamName);
        }

        [Fact]
        public void return_new_step()
        {
            var newStep = new MockEventStep<EventHandler>();
            var returnedStep = _propertyMock.SetNextStep(newStep);
            Assert.Same(newStep, returnedStep);
        }

        [Fact]
        public void set_step_used_by_Add()
        {
            bool called = false;
            var newStep = new MockEventStep<EventHandler>();
            newStep.Add.Action(_ => { called = true; });
            _propertyMock.SetNextStep(newStep);
            _propertyMock.Add((sender, e) => { });
            Assert.True(called);
        }

        [Fact]
        public void set_step_used_by_Remove()
        {
            bool called = false;
            var newStep = new MockEventStep<EventHandler>();
            newStep.Remove.Action(_ => { called = true; });
            _propertyMock.SetNextStep(newStep);
            _propertyMock.Remove((sender, e) => { });
            Assert.True(called);
        }
    }
}
