// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IEventMock.cs">
//   Copyright © 2018 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.Core
{
    #region Using Directives

    using System;

    #endregion

    public interface IEventMock<THandler> where THandler : Delegate
    {
        IEventImplementation<THandler> Implementation { get; }
        IEventMock<THandler> Use(IEventImplementation<THandler> implementation);
    }
}
