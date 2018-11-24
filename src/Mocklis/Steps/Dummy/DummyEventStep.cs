// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DummyEventStep.cs">
//   Copyright © 2018 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.Steps.Dummy
{
    #region Using Directives

    using System;
    using Mocklis.Core;

    #endregion

    public class DummyEventStep<THandler> : IEventStep<THandler> where THandler : Delegate
    {
        public static readonly DummyEventStep<THandler> Instance = new DummyEventStep<THandler>();

        private DummyEventStep()
        {
        }

        public void Add(MemberMock memberMock, THandler value)
        {
        }

        public void Remove(MemberMock memberMock, THandler value)
        {
        }
    }
}
