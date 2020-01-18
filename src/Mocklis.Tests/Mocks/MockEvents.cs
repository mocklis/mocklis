// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MockEvents.cs">
//   SPDX-License-Identifier: MIT
//   Copyright © 2019 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.Tests.Mocks
{
    #region Using Directives

    using System;
    using System.ComponentModel;
    using Mocklis.Core;
    using Mocklis.Tests.Interfaces;

    #endregion

    [MocklisClass]
    public class MockEvents : IEvents, INotifyPropertyChanged
    {
        // The contents of this class were created by the Mocklis code-generator.
        // Any changes you make will be overwritten if the contents are re-generated.

        public MockEvents()
        {
            MyEvent = new EventMock<EventHandler>(this, "MockEvents", "IEvents", "MyEvent", "MyEvent", Strictness.Lenient);
            SpecialEvent = new EventMock<EventHandler<Helpers.SpecialEventArgs>>(this, "MockEvents", "IEvents", "SpecialEvent", "SpecialEvent", Strictness.Lenient);
            PropertyChanged = new EventMock<PropertyChangedEventHandler>(this, "MockEvents", "INotifyPropertyChanged", "PropertyChanged", "PropertyChanged", Strictness.Lenient);
        }

        public EventMock<EventHandler> MyEvent { get; }

        event EventHandler? IEvents.MyEvent { add => MyEvent.Add(value); remove => MyEvent.Remove(value); }

        public EventMock<EventHandler<Helpers.SpecialEventArgs>> SpecialEvent { get; }

        event EventHandler<Helpers.SpecialEventArgs>? IEvents.SpecialEvent { add => SpecialEvent.Add(value); remove => SpecialEvent.Remove(value); }

        public EventMock<PropertyChangedEventHandler> PropertyChanged { get; }

        event PropertyChangedEventHandler? INotifyPropertyChanged.PropertyChanged { add => PropertyChanged.Add(value); remove => PropertyChanged.Remove(value); }
    }
}
