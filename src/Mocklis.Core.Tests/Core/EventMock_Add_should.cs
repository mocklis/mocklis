// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EventMock_Add_should.cs">
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

    public class EventMock_Add_should
    {
        private readonly EventMock<EventHandler> _eventMock;

        public EventMock_Add_should()
        {
            _eventMock = new EventMock<EventHandler>(new object(), "ClassName", "InterfaceName", "MemberName", "MockName");
        }

        [Fact]
        public void send_mock_information_and_handler_to_step()
        {
            IMockInfo sentMockInfo = null;
            EventHandler sentEventHandler = null;

            var newStep = new MockEventStep<EventHandler>();
            newStep.Add.Action(p =>
            {
                sentMockInfo = p.mockInfo;
                sentEventHandler = p.value;
            });
            ((IEventStepCaller<EventHandler>)_eventMock).SetNextStep(newStep);
            EventHandler handler = (sender, e) => { };

            _eventMock.Add(handler);

            Assert.Same(_eventMock, sentMockInfo);
            Assert.Equal(handler, sentEventHandler);
        }
    }
}
