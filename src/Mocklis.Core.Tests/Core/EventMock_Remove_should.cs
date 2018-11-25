// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EventMock_Remove_should.cs">
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

    public class EventMock_Remove_should
    {
        private readonly EventMock<EventHandler> _eventMock;

        public EventMock_Remove_should()
        {
            _eventMock = new EventMock<EventHandler>(new object(), "ClassName", "InterfaceName", "MemberName", "MockName");
        }

        [Fact]
        public void send_mock_information_and_handler_to_step()
        {
            IMockInfo sentMockInfo = null;
            EventHandler sentEventHandler = null;

            var newStep = new MockEventStep<EventHandler>();
            newStep.Remove.Action(p =>
            {
                sentMockInfo = p.mockInfo;
                sentEventHandler = p.value;
            });
            _eventMock.SetNextStep(newStep);
            EventHandler handler = (sender, e) => { };

            _eventMock.Remove(handler);

            Assert.Same(_eventMock, sentMockInfo);
            Assert.Equal(handler, sentEventHandler);
        }
    }
}
