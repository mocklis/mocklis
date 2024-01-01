// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SerilogContextTests.cs">
//   SPDX-License-Identifier: MIT
//   Copyright © 2019-2024 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.Steps.Log
{
    #region Using Directives

    using System;
    using Mocklis.Helpers;
    using Mocklis.Interfaces;
    using Mocklis.Mocks;
    using Serilog;
    using Serilog.Events;
    using Xunit;

    #endregion

    public class SerilogContextTests : ILogContextProvider
    {
        private MockMembers MockMembers { get; } = new MockMembers();
        private IMembers Members { get; }
        private ILogger Logger { get; }
        public ILogContext LogContext { get; }
        private LogEventListSink LogEvents { get; }
        private readonly EventHandler _handler = (sender, args) => { };

        public SerilogContextTests()
        {
            Members = MockMembers;

            LogEvents = new LogEventListSink();

            Logger = new LoggerConfiguration()
                .MinimumLevel.Verbose()
                .WriteTo.Sink(LogEvents)
                .CreateLogger();

            LogContext = new SerilogContext(Logger);
        }


        [Fact]
        public void AddEventHandlerCreatesLogEvents()
        {
            MockMembers.MyEvent.Log(this).Times(1, _ => { }).Throw(h => new Exception("Aha!"));

            Members.MyEvent += _handler;
            var ex = Assert.Throws<Exception>(() => Members.MyEvent += _handler);

            Assert.Equal(4, LogEvents.Count);
            Assert.Equal(LogEventLevel.Debug, LogEvents[0].Level);
            Assert.Equal(@"Adding event handler to [MockMembers] IMembers.MyEvent", LogEvents[0].RenderMessage());
            Assert.Equal(LogEventLevel.Debug, LogEvents[1].Level);
            Assert.Equal(@"Done adding event handler to [MockMembers] IMembers.MyEvent", LogEvents[1].RenderMessage());
            Assert.Equal(LogEventLevel.Debug, LogEvents[2].Level);
            Assert.Equal(@"Adding event handler to [MockMembers] IMembers.MyEvent", LogEvents[2].RenderMessage());
            Assert.Equal(LogEventLevel.Error, LogEvents[3].Level);
            Assert.Equal(@"Adding event handler to [MockMembers] IMembers.MyEvent threw exception ""Aha!""",
                LogEvents[3].RenderMessage());
            Assert.Same(ex, LogEvents[3].Exception);
        }

        [Fact]
        public void RemoveEventHandlerCreatesLogEvents()
        {
            MockMembers.MyEvent.Log(this).Times(1, _ => { }).Throw(h => new Exception("Aha!"));

            Members.MyEvent -= _handler;
            var ex = Assert.Throws<Exception>(() => Members.MyEvent -= _handler);

            Assert.Equal(4, LogEvents.Count);
            Assert.Equal(LogEventLevel.Debug, LogEvents[0].Level);
            Assert.Equal(@"Removing event handler from [MockMembers] IMembers.MyEvent", LogEvents[0].RenderMessage());
            Assert.Equal(LogEventLevel.Debug, LogEvents[1].Level);
            Assert.Equal(@"Done removing event handler from [MockMembers] IMembers.MyEvent", LogEvents[1].RenderMessage());
            Assert.Equal(LogEventLevel.Debug, LogEvents[2].Level);
            Assert.Equal(@"Removing event handler from [MockMembers] IMembers.MyEvent", LogEvents[2].RenderMessage());
            Assert.Equal(LogEventLevel.Error, LogEvents[3].Level);
            Assert.Equal(@"Removing event handler from [MockMembers] IMembers.MyEvent threw exception ""Aha!""",
                LogEvents[3].RenderMessage());
            Assert.Same(ex, LogEvents[3].Exception);
        }

        [Fact]
        public void IndexerGetCreatesLogEvents()
        {
            MockMembers.Item.Log(this).ReturnOnce("Hello").Throw(k => new Exception("Goodbye"));

            var _ = Members[5];
            var ex = Assert.Throws<Exception>(() => Members[8]);

            Assert.Equal(4, LogEvents.Count);
            Assert.Equal(LogEventLevel.Debug, LogEvents[0].Level);
            Assert.Equal(@"Getting value from [MockMembers] IMembers.this[] using key 5", LogEvents[0].RenderMessage());
            Assert.Equal(LogEventLevel.Debug, LogEvents[1].Level);
            Assert.Equal(@"Received ""Hello"" from [MockMembers] IMembers.this[]", LogEvents[1].RenderMessage());
            Assert.Equal(LogEventLevel.Debug, LogEvents[2].Level);
            Assert.Equal(@"Getting value from [MockMembers] IMembers.this[] using key 8", LogEvents[2].RenderMessage());
            Assert.Equal(LogEventLevel.Error, LogEvents[3].Level);
            Assert.Equal(@"Getting value from [MockMembers] IMembers.this[] threw exception ""Goodbye""", LogEvents[3].RenderMessage());
            Assert.Same(ex, LogEvents[3].Exception);
        }

        [Fact]
        public void IndexerSetCreatesLogEvents()
        {
            MockMembers.Item.Log(this).Times(1, _ => { }).Throw(k => new Exception("Goodbye"));

            Members[5] = "Hello";
            var ex = Assert.Throws<Exception>(() => Members[8] = "Hello Again");

            Assert.Equal(4, LogEvents.Count);
            Assert.Equal(LogEventLevel.Debug, LogEvents[0].Level);
            Assert.Equal(@"Setting value on [MockMembers] IMembers.this[] to ""Hello"" using key 5", LogEvents[0].RenderMessage());
            Assert.Equal(LogEventLevel.Debug, LogEvents[1].Level);
            Assert.Equal(@"Done setting value on [MockMembers] IMembers.this[]", LogEvents[1].RenderMessage());
            Assert.Equal(LogEventLevel.Debug, LogEvents[2].Level);
            Assert.Equal(@"Setting value on [MockMembers] IMembers.this[] to ""Hello Again"" using key 8", LogEvents[2].RenderMessage());
            Assert.Equal(LogEventLevel.Error, LogEvents[3].Level);
            Assert.Equal(@"Setting value on [MockMembers] IMembers.this[] threw exception ""Goodbye""", LogEvents[3].RenderMessage());
            Assert.Same(ex, LogEvents[3].Exception);
        }


        [Fact]
        public void ParameterlessActionCreatesLogEvents()
        {
            MockMembers.DoStuff.Log(this).Times(1, _ => { }).Throw(() => new Exception("Goodbye"));

            Members.DoStuff();
            var ex = Assert.Throws<Exception>(() => Members.DoStuff());

            Assert.Equal(4, LogEvents.Count);
            Assert.Equal(LogEventLevel.Debug, LogEvents[0].Level);
            Assert.Equal(@"Calling [MockMembers] IMembers.DoStuff", LogEvents[0].RenderMessage());
            Assert.Equal(LogEventLevel.Debug, LogEvents[1].Level);
            Assert.Equal(@"Returned from [MockMembers] IMembers.DoStuff", LogEvents[1].RenderMessage());
            Assert.Equal(LogEventLevel.Debug, LogEvents[2].Level);
            Assert.Equal(@"Calling [MockMembers] IMembers.DoStuff", LogEvents[2].RenderMessage());
            Assert.Equal(LogEventLevel.Error, LogEvents[3].Level);
            Assert.Equal(@"Call to [MockMembers] IMembers.DoStuff threw exception ""Goodbye""", LogEvents[3].RenderMessage());
            Assert.Same(ex, LogEvents[3].Exception);
        }

        [Fact]
        public void FuncWithParameterCreatesLogEvents()
        {
            MockMembers.Calculate
                .Log(this)
                .ReturnOnce(15)
                .Throw(p => new Exception("Beep - wrong!"));

            var _ = Members.Calculate(13, 21);
            var ex = Assert.Throws<Exception>(() => Members.Calculate(14, 22));

            Assert.Equal(4, LogEvents.Count);
            Assert.Equal(LogEventLevel.Debug, LogEvents[0].Level);
            Assert.Equal(@"Calling [MockMembers] IMembers.Calculate with parameter(s): [13, 21]", LogEvents[0].RenderMessage());
            Assert.Equal(LogEventLevel.Debug, LogEvents[1].Level);
            Assert.Equal(@"Returned from [MockMembers] IMembers.Calculate with result(s): 15", LogEvents[1].RenderMessage());
            Assert.Equal(LogEventLevel.Debug, LogEvents[2].Level);
            Assert.Equal(@"Calling [MockMembers] IMembers.Calculate with parameter(s): [14, 22]", LogEvents[2].RenderMessage());
            Assert.Equal(LogEventLevel.Error, LogEvents[3].Level);
            Assert.Equal(@"Call to [MockMembers] IMembers.Calculate threw exception ""Beep - wrong!""", LogEvents[3].RenderMessage());
            Assert.Same(ex, LogEvents[3].Exception);
        }

        [Fact]
        public void PropertyGetCreatesLogEvents()
        {
            MockMembers.StringProperty.Log(this).ReturnOnce("Hello").Throw(() => new Exception("Goodbye"));

            var _ = Members.StringProperty;
            var ex = Assert.Throws<Exception>(() => Members.StringProperty);

            Assert.Equal(4, LogEvents.Count);
            Assert.Equal(LogEventLevel.Debug, LogEvents[0].Level);
            Assert.Equal(@"Getting value from [MockMembers] IMembers.StringProperty", LogEvents[0].RenderMessage());
            Assert.Equal(LogEventLevel.Debug, LogEvents[1].Level);
            Assert.Equal(@"Received ""Hello"" from [MockMembers] IMembers.StringProperty", LogEvents[1].RenderMessage());
            Assert.Equal(LogEventLevel.Debug, LogEvents[2].Level);
            Assert.Equal(@"Getting value from [MockMembers] IMembers.StringProperty", LogEvents[2].RenderMessage());
            Assert.Equal(LogEventLevel.Error, LogEvents[3].Level);
            Assert.Equal(@"Getting value from [MockMembers] IMembers.StringProperty threw exception ""Goodbye""",
                LogEvents[3].RenderMessage());
            Assert.Same(ex, LogEvents[3].Exception);
        }

        [Fact]
        public void PropertySetCreatesLogEvents()
        {
            MockMembers.StringProperty.Log(this).Times(1, _ => { }).Throw(() => new Exception("Goodbye"));

            Members.StringProperty = "Hello";
            var ex = Assert.Throws<Exception>(() => Members.StringProperty = "Hello Again");

            Assert.Equal(4, LogEvents.Count);
            Assert.Equal(LogEventLevel.Debug, LogEvents[0].Level);
            Assert.Equal(@"Setting value on [MockMembers] IMembers.StringProperty to ""Hello""", LogEvents[0].RenderMessage());
            Assert.Equal(LogEventLevel.Debug, LogEvents[1].Level);
            Assert.Equal(@"Done setting value on [MockMembers] IMembers.StringProperty", LogEvents[1].RenderMessage());
            Assert.Equal(LogEventLevel.Debug, LogEvents[2].Level);
            Assert.Equal(@"Setting value on [MockMembers] IMembers.StringProperty to ""Hello Again""", LogEvents[2].RenderMessage());
            Assert.Equal(LogEventLevel.Error, LogEvents[3].Level);
            Assert.Equal(@"Setting value on [MockMembers] IMembers.StringProperty threw exception ""Goodbye""",
                LogEvents[3].RenderMessage());
            Assert.Same(ex, LogEvents[3].Exception);
        }

        [Fact]
        public void CanUseNonStandardLogLevels()
        {
            MockMembers.StringProperty
                .Log(new SerilogContext(Logger, LogEventLevel.Information, LogEventLevel.Warning))
                .Throw(() => new Exception("Ex"));

            Assert.Throws<Exception>(() => Members.StringProperty);

            Assert.Equal(2, LogEvents.Count);
            Assert.Equal(LogEventLevel.Information, LogEvents[0].Level);
            Assert.Equal(LogEventLevel.Warning, LogEvents[1].Level);
        }
    }
}
