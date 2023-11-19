// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AddActionEventStepTests.cs">
//   SPDX-License-Identifier: MIT
//   Copyright © 2019-2023 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.Steps.Lambda
{
    #region Using Directives

    using System;
    using Mocklis.Interfaces;
    using Mocklis.Mocks;
    using Xunit;

    #endregion

    public class AddActionEventStepTests
    {
        public MockMembers MockMembers { get; } = new MockMembers();
        public IEvents Sut => MockMembers;

        public EventHandler HandlerInstance { get; } = (e, s) => { };

        [Fact]
        public void RequireNonNullAction()
        {
            Assert.Throws<ArgumentNullException>(() => MockMembers.MyEvent.AddAction(null!));
        }

        [Fact]
        public void InvokeActionOnAdd()
        {
            EventHandler? addedInstance = null;

            MockMembers.MyEvent.AddAction(i => addedInstance = i);

            Sut.MyEvent += HandlerInstance;

            Assert.Same(HandlerInstance, addedInstance);
        }

        [Fact]
        public void ForwardRemoves()
        {
            MockMembers.MyEvent.AddAction(_ => throw new InvalidOperationException()).RecordBeforeRemove(out var ledger);

            Sut.MyEvent -= HandlerInstance;

            Assert.Equal(new[] { HandlerInstance }, ledger);
        }
    }
}
