// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EventMockRemoveTests.cs">
//   SPDX-License-Identifier: MIT
//   Copyright © 2019-2020 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.Core
{
    #region Using Directives

    using System;
    using Mocklis.Helpers;
    using Xunit;

    #endregion

    public class EventMockRemoveTests
    {
        private static FakeNextEventStep<THandler> NextStepFor<THandler>(ICanHaveNextEventStep<THandler> mock) where THandler : Delegate
        {
            return new FakeNextEventStep<THandler>(mock);
        }

        private readonly EventHandler _handler = (sender, e) => { };

        [Fact]
        public void SendMockInformationAndHandlerToStep()
        {
            var eventMock = new EventMock<EventHandler>(new object(), "ClassName", "InterfaceName", "MemberName", "MockName", Strictness.Lenient);
            var nextStep = NextStepFor(eventMock);

            eventMock.Remove(_handler);

            Assert.Equal(0, nextStep.AddCount);
            Assert.Equal(1, nextStep.RemoveCount);
            Assert.Same(eventMock, nextStep.LastRemoveMockInfo);
            Assert.Equal(_handler, nextStep.LastRemoveValue);
        }

        [Fact]
        public void DoNothingIfNoStepInLenientMode()
        {
            var eventMock = new EventMock<EventHandler>(new object(), "ClassName", "InterfaceName", "MemberName", "MockName", Strictness.Lenient);
            eventMock.Remove(_handler);
        }

        [Fact]
        public void ThrowIfNoStepInStrictMode()
        {
            var eventMock = new EventMock<EventHandler>(new object(), "ClassName", "InterfaceName", "MemberName", "MockName", Strictness.Strict);
            var ex = Assert.Throws<MockMissingException>(() => eventMock.Remove(_handler));
            Assert.Equal(MockType.EventRemove, ex.MemberType);
        }

        [Fact]
        public void ThrowIfNoStepInVeryStrictMode()
        {
            var eventMock = new EventMock<EventHandler>(new object(), "ClassName", "InterfaceName", "MemberName", "MockName", Strictness.VeryStrict);
            var ex = Assert.Throws<MockMissingException>(() => eventMock.Remove(_handler));
            Assert.Equal(MockType.EventRemove, ex.MemberType);
        }

        [Fact]
        public void DoNothingIfClearedInLenientMode()
        {
            var eventMock = new EventMock<EventHandler>(new object(), "ClassName", "InterfaceName", "MemberName", "MockName", Strictness.Lenient);
            var nextStep = NextStepFor(eventMock);
            eventMock.Clear();
            eventMock.Remove(_handler);
            Assert.Equal(0, nextStep.AddCount);
            Assert.Equal(0, nextStep.RemoveCount);
        }

        [Fact]
        public void ThrowIfClearedInStrictMode()
        {
            var eventMock = new EventMock<EventHandler>(new object(), "ClassName", "InterfaceName", "MemberName", "MockName", Strictness.Strict);
            var nextStep = NextStepFor(eventMock);
            eventMock.Clear();
            var ex = Assert.Throws<MockMissingException>(() => eventMock.Remove(_handler));
            Assert.Equal(MockType.EventRemove, ex.MemberType);
            Assert.Equal(0, nextStep.AddCount);
            Assert.Equal(0, nextStep.RemoveCount);
        }

        [Fact]
        public void ThrowIfClearedInVeryStrictMode()
        {
            var eventMock = new EventMock<EventHandler>(new object(), "ClassName", "InterfaceName", "MemberName", "MockName", Strictness.VeryStrict);
            var nextStep = NextStepFor(eventMock);
            eventMock.Clear();
            var ex = Assert.Throws<MockMissingException>(() => eventMock.Remove(_handler));
            Assert.Equal(MockType.EventRemove, ex.MemberType);
            Assert.Equal(0, nextStep.AddCount);
            Assert.Equal(0, nextStep.RemoveCount);
        }
    }
}
