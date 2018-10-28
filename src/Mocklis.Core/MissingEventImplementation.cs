// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MissingEventImplementation.cs">
//   Copyright © 2018 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.Core
{
    #region Using Directives

    using System;

    #endregion

    public sealed class MissingEventImplementation<THandler> : IEventImplementation<THandler> where THandler : Delegate
    {
        public static readonly MissingEventImplementation<THandler> Instance = new MissingEventImplementation<THandler>();

        private MissingEventImplementation()
        {
        }

        public void Add(MemberMock memberMock, THandler value)
        {
            throw new MockMissingException(MockType.EventAdd, memberMock.InterfaceName, memberMock.MemberName, memberMock.MemberMockName);
        }

        public void Remove(MemberMock memberMock, THandler value)
        {
            throw new MockMissingException(MockType.EventRemove, memberMock.InterfaceName, memberMock.MemberName, memberMock.MemberMockName);
        }
    }
}
