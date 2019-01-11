// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MissingEventStep_Add_should.cs">
//   Copyright © 2019 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.Core.Tests.Steps.Missing
{
    #region Using Directives

    using System;
    using Mocklis.Steps.Missing;
    using Xunit;

    #endregion

    public class MissingEventStep_Add_should
    {
        private readonly EventMock<EventHandler> _eventMock;
        private readonly MissingEventStep<EventHandler> _missingEventStep;

        public MissingEventStep_Add_should()
        {
            _eventMock = new EventMock<EventHandler>(new object(), "TestClass", "ITest", "Event", "Event_1");
            _missingEventStep = MissingEventStep<EventHandler>.Instance;
        }

        [Fact]
        public void throw_exception()
        {
            EventHandler newHandler = (sender, e) => { };

            var exception = Assert.Throws<MockMissingException>(() => _missingEventStep.Add(_eventMock, newHandler));
            Assert.Equal(MockType.EventAdd, exception.MemberType);
            Assert.Equal("TestClass", exception.MocklisClassName);
            Assert.Equal("ITest", exception.InterfaceName);
            Assert.Equal("Event", exception.MemberName);
            Assert.Equal("Event_1", exception.MemberMockName);
        }
    }
}
