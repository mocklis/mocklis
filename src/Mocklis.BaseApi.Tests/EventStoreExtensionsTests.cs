// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EventStoreExtensionsTests.cs">
//   SPDX-License-Identifier: MIT
//   Copyright © 2019-2021 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis
{
    #region Using Directives

    using System;
    using System.ComponentModel;
    using Mocklis.Helpers;
    using Mocklis.Interfaces;
    using Mocklis.Mocks;
    using Xunit;

    #endregion

    public class EventStoreExtensionsTests
    {
        private MockEvents MockEvents { get; } = new MockEvents();
        private IEvents Evs => MockEvents;
        private INotifyPropertyChanged Npc => MockEvents;

        [Fact]
        public void RaiseEventHandlerCorrectly()
        {
            MockEvents.MyEvent.Stored(out var stored);
            object? sender = null;
            EventArgs? eventArgs = null;
            Evs.MyEvent += (s, e) =>
            {
                sender = s;
                eventArgs = e;
            };

            var newEventArgs = new EventArgs();
            stored.Raise(this, newEventArgs);

            Assert.Same(this, sender);
            Assert.Same(newEventArgs, eventArgs);
        }

        [Fact]
        public void RaiseGenericEventHandlerCorrectly()
        {
            MockEvents.SpecialEvent.Stored(out var stored);
            object? sender = null;
            SpecialEventArgs? eventArgs = null;
            Evs.SpecialEvent += (s, e) =>
            {
                sender = s;
                eventArgs = e;
            };

            stored.Raise(this, new SpecialEventArgs("Hello", 42));

            Assert.Same(this, sender);
            Assert.Equal("Hello", eventArgs?.Text);
            Assert.Equal(42, eventArgs?.Number);
        }

        [Fact]
        public void RaiseNotifyPropertyChangedCorrectly()
        {
            MockEvents.PropertyChanged.Stored(out var stored);
            object? sender = null;
            PropertyChangedEventArgs? eventArgs = null;
            Npc.PropertyChanged += (s, e) =>
            {
                sender = s;
                eventArgs = e;
            };

            stored.Raise(this, new PropertyChangedEventArgs("MyProperty"));

            Assert.Same(this, sender);
            Assert.Equal("MyProperty", eventArgs?.PropertyName);
        }
    }
}
