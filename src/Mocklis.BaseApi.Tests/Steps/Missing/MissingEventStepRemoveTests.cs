// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MissingEventStepRemoveTests.cs">
//   SPDX-License-Identifier: MIT
//   Copyright © 2019-2020 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.Steps.Missing
{
    #region Using Directives

    using System;
    using Mocklis.Core;
    using Xunit;

    #endregion

    public class MissingEventStepRemoveTests
    {
        private readonly EventMock<EventHandler> _eventMock;
        private readonly MissingEventStep<EventHandler> _missingEventStep;

        public MissingEventStepRemoveTests()
        {
            _eventMock = new EventMock<EventHandler>(new object(), "TestClass", "ITest", "Event", "Event1", Strictness.Lenient);
            _missingEventStep = MissingEventStep<EventHandler>.Instance;
        }

        [Fact]
        public void ThrowException()
        {
            static void NewHandler(object? sender, EventArgs e)
            {
            }

            var exception = Assert.Throws<MockMissingException>(() => _missingEventStep.Remove(_eventMock, NewHandler));
            Assert.Equal(MockType.EventRemove, exception.MemberType);
            Assert.Equal("TestClass", exception.MocklisClassName);
            Assert.Equal("ITest", exception.InterfaceName);
            Assert.Equal("Event", exception.MemberName);
            Assert.Equal("Event1", exception.MemberMockName);
        }
    }
}
