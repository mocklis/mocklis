// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MissingEventStepAddTests.cs">
//   SPDX-License-Identifier: MIT
//   Copyright © 2019-2024 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.Steps.Missing
{
    #region Using Directives

    using System;
    using Mocklis.Core;
    using Xunit;

    #endregion

    public class MissingEventStepAddTests
    {
        private readonly EventMock<EventHandler> _eventMock;
        private readonly MissingEventStep<EventHandler> _missingEventStep;

        public MissingEventStepAddTests()
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

            var exception = Assert.Throws<MockMissingException>(() => _missingEventStep.Add(_eventMock, NewHandler));
            Assert.Equal(MockType.EventAdd, exception.MemberType);
            Assert.Equal("TestClass", exception.MocklisClassName);
            Assert.Equal("ITest", exception.InterfaceName);
            Assert.Equal("Event", exception.MemberName);
            Assert.Equal("Event1", exception.MemberMockName);
        }
    }
}
