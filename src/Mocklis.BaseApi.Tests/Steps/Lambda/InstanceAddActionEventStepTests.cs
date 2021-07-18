// --------------------------------------------------------------------------------------------------------------------
// <copyright file="InstanceAddActionEventStepTests.cs">
//   SPDX-License-Identifier: MIT
//   Copyright © 2019-2021 Esbjörn Redmo and contributors. All rights reserved.
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

    public class InstanceAddActionEventStepTests
    {
        public MockMembers MockMembers { get; } = new MockMembers();
        public IEvents Sut => MockMembers;

        public EventHandler HandlerInstance { get; } = (e, s) => { };

        [Fact]
        public void RequireNonNullAction()
        {
            Assert.Throws<ArgumentNullException>(() => MockMembers.MyEvent.InstanceAddAction(null!));
        }

        [Fact]
        public void InvokeActionOnAdd()
        {
            object? callInstance = null;
            EventHandler? addedInstance = null;

            MockMembers.MyEvent.InstanceAddAction((obj, i) =>
            {
                callInstance = obj;
                addedInstance = i;
            });

            Sut.MyEvent += HandlerInstance;

            Assert.Same(Sut, callInstance);
            Assert.Same(HandlerInstance, addedInstance);
        }

        [Fact]
        public void ForwardRemoves()
        {
            MockMembers.MyEvent.InstanceAddAction((o, v) => throw new InvalidOperationException()).RecordBeforeRemove(out var ledger);

            Sut.MyEvent -= HandlerInstance;

            Assert.Equal(new[] { HandlerInstance }, ledger);
        }
    }
}
