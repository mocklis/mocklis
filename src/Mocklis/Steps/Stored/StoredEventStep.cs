// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StoredEventStep.cs">
//   Copyright © 2018 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.Steps.Stored
{
    #region Using Directives

    using System;
    using System.Threading;
    using Mocklis.Core;
    using Mocklis.Verification;

    #endregion

    public class StoredEventStep<THandler> : IEventStep<THandler>, IStoredEvent<THandler> where THandler : Delegate
    {
        private THandler _eventHandler;

        public THandler EventHandler => _eventHandler;

        void IEventStep<THandler>.Add(MemberMock memberMock, THandler value)
        {
            THandler previousHandler;
            THandler eventHandler = _eventHandler;
            do
            {
                previousHandler = eventHandler;
                THandler newHandler = (THandler)Delegate.Combine(previousHandler, value);
                eventHandler = Interlocked.CompareExchange(ref _eventHandler, newHandler, previousHandler);
            }
            while (eventHandler != previousHandler);
        }

        void IEventStep<THandler>.Remove(MemberMock memberMock, THandler value)
        {
            THandler previousHandler;
            THandler eventHandler = _eventHandler;
            do
            {
                previousHandler = eventHandler;
                THandler newHandler = (THandler)Delegate.Remove(previousHandler, value);
                eventHandler = Interlocked.CompareExchange(ref _eventHandler, newHandler, previousHandler);
            }
            while (eventHandler != previousHandler);
        }
    }
}
