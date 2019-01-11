// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StoredEventExtensions.cs">
//   Copyright © 2019 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.Verification
{
    #region Using Directives

    using System;
    using System.ComponentModel;

    #endregion

    public static class StoredEventExtensions
    {
        public static void Raise(this IStoredEvent<EventHandler> storedEvent, object sender, EventArgs e)
        {
            storedEvent.EventHandler?.Invoke(sender, e);
        }

        public static void Raise<TEventArg>(this IStoredEvent<EventHandler<TEventArg>> storedEvent, object sender,
            TEventArg e) where TEventArg : EventArgs
        {
            storedEvent.EventHandler?.Invoke(sender, e);
        }

        public static void Raise(this IStoredEvent<PropertyChangedEventHandler> storedEvent, object sender,
            PropertyChangedEventArgs e)
        {
            storedEvent.EventHandler?.Invoke(sender, e);
        }
    }
}
