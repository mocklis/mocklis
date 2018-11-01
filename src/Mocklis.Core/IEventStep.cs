// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IEventStep.cs">
//   Copyright © 2018 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.Core
{
    #region Using Directives

    using System;

    #endregion

    public interface IEventStep<in THandler> where THandler : Delegate
    {
        void Add(MemberMock memberMock, THandler value);
        void Remove(MemberMock memberMock, THandler value);
    }
}
