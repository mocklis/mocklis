// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EventMock.cs">
//   Copyright © 2018 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.Core
{
    #region Using Directives

    using System;

    #endregion

    public sealed class EventMock<THandler> : MemberMock, IEventMock<THandler> where THandler : Delegate
    {
        public IEventImplementation<THandler> Implementation { get; private set; } = MissingEventImplementation<THandler>.Instance;

        public EventMock(string interfaceName, string memberName, string memberMockName) : base(interfaceName, memberName, memberMockName)
        {
        }

        public IEventMock<THandler> Use(IEventImplementation<THandler> implementation)
        {
            Implementation = implementation ?? throw new ArgumentNullException(nameof(implementation));
            return this;
        }

        public void Add(THandler value)
        {
            Implementation.Add(this, value);
        }

        public void Remove(THandler value)
        {
            Implementation.Remove(this, value);
        }
    }
}
