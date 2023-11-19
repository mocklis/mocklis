// --------------------------------------------------------------------------------------------------------------------
// <copyright file="InstanceRemoveActionEventStepTests.cs">
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

    public class InstanceRemoveActionEventStepTests
    {
        public MockMembers MockMembers { get; } = new MockMembers();
        public IEvents Sut => MockMembers;

        public EventHandler HandlerInstance { get; } = (e, s) => { };

        [Fact]
        public void RequireNonNullAction()
        {
            Assert.Throws<ArgumentNullException>(() => MockMembers.MyEvent.InstanceRemoveAction(null!));
        }

        [Fact]
        public void InvokeActionOnRemove()
        {
            object? callInstance = null;
            EventHandler? addedInstance = null;

            MockMembers.MyEvent.InstanceRemoveAction((obj, i) =>
            {
                callInstance = obj;
                addedInstance = i;
            });

            Sut.MyEvent -= HandlerInstance;

            Assert.Same(Sut, callInstance);
            Assert.Same(HandlerInstance, addedInstance);
        }

        [Fact]
        public void ForwardAdds()
        {
            MockMembers.MyEvent.InstanceRemoveAction((o, v) => throw new InvalidOperationException()).RecordBeforeAdd(out var ledger);

            Sut.MyEvent += HandlerInstance;

            Assert.Equal(new[] { HandlerInstance }, ledger);
        }
    }
}
