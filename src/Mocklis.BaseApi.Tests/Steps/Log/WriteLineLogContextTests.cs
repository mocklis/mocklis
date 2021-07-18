// --------------------------------------------------------------------------------------------------------------------
// <copyright file="WriteLineLogContextTests.cs">
//   SPDX-License-Identifier: MIT
//   Copyright © 2019-2021 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.Steps.Log
{
    #region Using Directives

    using System;
    using System.Collections.Generic;
    using Mocklis.Interfaces;
    using Mocklis.Mocks;
    using Xunit;

    #endregion

    public class WriteLineLogContextTests
    {
        private readonly MockMembers _mockMembers;
        private readonly IEvents _events;
        private readonly IIndexers _indexers;
        private readonly IMethods _methods;
        private readonly IProperties _properties;
        private readonly EventHandler _sampleEventHandler = (sender, args) => { };
        private readonly List<string> _logLines;
        private readonly WriteLineLogContext _logLineContext;

        public WriteLineLogContextTests()
        {
            _mockMembers = new MockMembers();
            _events = _mockMembers;
            _indexers = _mockMembers;
            _methods = _mockMembers;
            _properties = _mockMembers;
            _logLines = new List<string>();
            _logLineContext = new WriteLineLogContext(s => _logLines.Add(s));
        }

        [Fact]
        public void ExposeConsoleInstance()
        {
            Assert.NotNull(WriteLineLogContext.Console);
        }

        [Fact]
        public void RequireWriteLineAction()
        {
            var exception = Assert.Throws<ArgumentNullException>(() =>
            {
                var _ = new WriteLineLogContext(null!);
            });

            Assert.Equal("writeLine", exception.ParamName);
        }

        private void AssertLogLines(params string[] expectedLines)
        {
            Assert.Equal(expectedLines, _logLines);
        }

        [Fact]
        public void LogBeforeAndAfterOnEventAdd()
        {
            // Arrange
            _mockMembers.MyEvent.Log(_logLineContext);

            // Act
            _events.MyEvent += _sampleEventHandler;

            // Assert
            AssertLogLines(
                "Adding event handler to '[MockMembers] IEvents.MyEvent'",
                "Done adding event handler to '[MockMembers] IEvents.MyEvent'");
        }

        [Fact]
        public void LogBeforeAndExceptionOnEventAdd()
        {
            // Arrange
            _mockMembers.MyEvent.Log(_logLineContext).Throw(handler => new Exception("Exception thrown!"));

            // Act
            Assert.Throws<Exception>(() => _events.MyEvent += _sampleEventHandler);

            // Assert
            AssertLogLines(
                "Adding event handler to '[MockMembers] IEvents.MyEvent'",
                "Adding event handler to '[MockMembers] IEvents.MyEvent' threw exception 'Exception thrown!'");
        }

        [Fact]
        public void LogBeforeAndAfterOnEventRemove()
        {
            // Arrange
            _mockMembers.MyEvent.Log(_logLineContext);

            // Act
            _events.MyEvent -= _sampleEventHandler;

            // Assert
            AssertLogLines(
                "Removing event handler from '[MockMembers] IEvents.MyEvent'",
                "Done removing event handler from '[MockMembers] IEvents.MyEvent'");
        }

        [Fact]
        public void LogBeforeAndExceptionOnEventRemove()
        {
            // Arrange
            _mockMembers.MyEvent.Log(_logLineContext).Throw(handler => new Exception("Exception thrown!"));

            // Act
            Assert.Throws<Exception>(() => _events.MyEvent -= _sampleEventHandler);

            // Assert
            AssertLogLines(
                "Removing event handler from '[MockMembers] IEvents.MyEvent'",
                "Removing event handler from '[MockMembers] IEvents.MyEvent' threw exception 'Exception thrown!'");
        }

        [Fact]
        public void LogBeforeAndAfterOnIndexerSet()
        {
            // Arrange
            _mockMembers.Item.Log(_logLineContext);

            // Act
            _indexers[5] = "Test";

            // Assert
            AssertLogLines(
                "Setting value on '[MockMembers] IIndexers.this[]' to 'Test' using key '5'",
                "Done setting value on '[MockMembers] IIndexers.this[]'");
        }

        [Fact]
        public void LogBeforeAndExceptionOnIndexerSet()
        {
            // Arrange
            _mockMembers.Item.Log(_logLineContext).Throw(key => new Exception("Exception thrown!"));

            // Act
            Assert.Throws<Exception>(() => _indexers[5] = "Test");

            // Assert
            AssertLogLines(
                "Setting value on '[MockMembers] IIndexers.this[]' to 'Test' using key '5'",
                "Setting value on '[MockMembers] IIndexers.this[]' threw exception 'Exception thrown!'");
        }

        [Fact]
        public void LogBeforeAndAfterOnIndexerGet()
        {
            // Arrange
            _mockMembers.Item.Log(_logLineContext).Return("Test");

            // Act
            var _ = _indexers[5];

            // Assert
            AssertLogLines(
                "Getting value from '[MockMembers] IIndexers.this[]' using key '5'",
                "Received 'Test' from '[MockMembers] IIndexers.this[]'");
        }

        [Fact]
        public void LogBeforeAndExceptionOnIndexerGet()
        {
            // Arrange
            _mockMembers.Item.Log(_logLineContext).Throw(key => new Exception("Exception thrown!"));

            // Act
            Assert.Throws<Exception>(() => _indexers[5]);

            // Assert
            AssertLogLines(
                "Getting value from '[MockMembers] IIndexers.this[]' using key '5'",
                "Getting value from '[MockMembers] IIndexers.this[]' threw exception 'Exception thrown!'");
        }

        [Fact]
        public void LogBeforeAndAfterOnMethodCallWithParameterAndResult()
        {
            // Arrange
            _mockMembers.FuncWithParameter.Log(_logLineContext).Return(5);

            // Act
            _methods.FuncWithParameter(9);

            // Assert
            AssertLogLines(
                "Calling '[MockMembers] IMethods.FuncWithParameter' with parameter: '9'",
                "Returned from '[MockMembers] IMethods.FuncWithParameter' with result: '5'");
        }

        [Fact]
        public void LogBeforeAndAfterOnMethodCallWithoutParameterOrResult()
        {
            // Arrange
            _mockMembers.SimpleAction.Log(_logLineContext);

            // Act
            _methods.SimpleAction();

            // Assert
            AssertLogLines(
                "Calling '[MockMembers] IMethods.SimpleAction'",
                "Returned from '[MockMembers] IMethods.SimpleAction'");
        }

        [Fact]
        public void LogBeforeAndExceptionOnMethodCall()
        {
            // Arrange
            _mockMembers.FuncWithParameter.Log(_logLineContext).Throw(p => new Exception("Exception thrown!"));

            // Act
            Assert.Throws<Exception>(() => _methods.FuncWithParameter(9));

            // Assert
            AssertLogLines(
                "Calling '[MockMembers] IMethods.FuncWithParameter' with parameter: '9'",
                "Call to '[MockMembers] IMethods.FuncWithParameter' threw exception 'Exception thrown!'");
        }

        [Fact]
        public void LogBeforeAndAfterOnPropertySet()
        {
            // Arrange
            _mockMembers.StringProperty.Log(_logLineContext);

            // Act
            _properties.StringProperty = "Test";

            // Assert
            AssertLogLines(
                "Setting value on '[MockMembers] IProperties.StringProperty' to 'Test'",
                "Done setting value on '[MockMembers] IProperties.StringProperty'");
        }

        [Fact]
        public void LogBeforeAndExceptionOnPropertySet()
        {
            // Arrange
            _mockMembers.StringProperty.Log(_logLineContext).Throw(() => new Exception("Exception thrown!"));

            // Act
            Assert.Throws<Exception>(() => _properties.StringProperty = "Test");

            // Assert
            AssertLogLines(
                "Setting value on '[MockMembers] IProperties.StringProperty' to 'Test'",
                "Setting value on '[MockMembers] IProperties.StringProperty' threw exception 'Exception thrown!'");
        }

        [Fact]
        public void LogBeforeAndAfterOnPropertyGet()
        {
            // Arrange
            _mockMembers.StringProperty.Log(_logLineContext).Return("Test");

            // Act
            var _ = _properties.StringProperty;

            // Assert
            AssertLogLines(
                "Getting value from '[MockMembers] IProperties.StringProperty'",
                "Received 'Test' from '[MockMembers] IProperties.StringProperty'");
        }

        [Fact]
        public void LogBeforeAndExceptionOnProportyGet()
        {
            // Arrange
            _mockMembers.StringProperty.Log(_logLineContext).Throw(() => new Exception("Exception thrown!"));

            // Act
            Assert.Throws<Exception>(() => _properties.StringProperty);

            // Assert
            AssertLogLines(
                "Getting value from '[MockMembers] IProperties.StringProperty'",
                "Getting value from '[MockMembers] IProperties.StringProperty' threw exception 'Exception thrown!'");
        }
    }
}
