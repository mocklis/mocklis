// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EventMock_Add_should.cs">
//   SPDX-License-Identifier: MIT
//   Copyright © 2019 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.Core.Tests.Core
{
    #region Using Directives

    using System;
    using Mocklis.Core.Tests.Helpers;
    using Xunit;

    #endregion

    public class EventMock_Add_should
    {
        private static FakeNextEventStep<THandler> NextStepFor<THandler>(ICanHaveNextEventStep<THandler> mock) where THandler : Delegate
        {
            return new FakeNextEventStep<THandler>(mock);
        }

        private readonly EventHandler _handler = (sender, e) => { };

        [Fact]
        public void send_mock_information_and_handler_to_step()
        {
            var eventMock = new EventMock<EventHandler>(new object(), "ClassName", "InterfaceName", "MemberName", "MockName", Strictness.Lenient);
            var nextStep = NextStepFor(eventMock);

            eventMock.Add(_handler);

            Assert.Equal(1, nextStep.AddCount);
            Assert.Equal(0, nextStep.RemoveCount);
            Assert.Same(eventMock, nextStep.LastAddMockInfo);
            Assert.Equal(_handler, nextStep.LastAddValue);
        }

        [Fact]
        public void do_nothing_if_no_step_in_lenient_mode()
        {
            var eventMock = new EventMock<EventHandler>(new object(), "ClassName", "InterfaceName", "MemberName", "MockName", Strictness.Lenient);
            eventMock.Add(_handler);
        }

        [Fact]
        public void throw_if_no_step_in_strict_mode()
        {
            var eventMock = new EventMock<EventHandler>(new object(), "ClassName", "InterfaceName", "MemberName", "MockName", Strictness.Strict);
            var ex = Assert.Throws<MockMissingException>(() => eventMock.Add(_handler));
            Assert.Equal(MockType.EventAdd, ex.MemberType);
        }

        [Fact]
        public void throw_if_no_step_in_very_strict_mode()
        {
            var eventMock = new EventMock<EventHandler>(new object(), "ClassName", "InterfaceName", "MemberName", "MockName", Strictness.VeryStrict);
            var ex = Assert.Throws<MockMissingException>(() => eventMock.Add(_handler));
            Assert.Equal(MockType.EventAdd, ex.MemberType);
        }

        [Fact]
        public void do_nothing_if_cleared_in_lenient_mode()
        {
            var eventMock = new EventMock<EventHandler>(new object(), "ClassName", "InterfaceName", "MemberName", "MockName", Strictness.Lenient);
            var nextStep = NextStepFor(eventMock);
            eventMock.Clear();
            eventMock.Add(_handler);
            Assert.Equal(0, nextStep.AddCount);
            Assert.Equal(0, nextStep.RemoveCount);
        }

        [Fact]
        public void throw_if_cleared_in_strict_mode()
        {
            var eventMock = new EventMock<EventHandler>(new object(), "ClassName", "InterfaceName", "MemberName", "MockName", Strictness.Strict);
            var nextStep = NextStepFor(eventMock);
            eventMock.Clear();
            var ex = Assert.Throws<MockMissingException>(() => eventMock.Add(_handler));
            Assert.Equal(MockType.EventAdd, ex.MemberType);
            Assert.Equal(0, nextStep.AddCount);
            Assert.Equal(0, nextStep.RemoveCount);
        }

        [Fact]
        public void throw_if_cleared_in_very_strict_mode()
        {
            var eventMock = new EventMock<EventHandler>(new object(), "ClassName", "InterfaceName", "MemberName", "MockName", Strictness.VeryStrict);
            var nextStep = NextStepFor(eventMock);
            eventMock.Clear();
            var ex = Assert.Throws<MockMissingException>(() => eventMock.Add(_handler));
            Assert.Equal(MockType.EventAdd, ex.MemberType);
            Assert.Equal(0, nextStep.AddCount);
            Assert.Equal(0, nextStep.RemoveCount);
        }
    }
}
