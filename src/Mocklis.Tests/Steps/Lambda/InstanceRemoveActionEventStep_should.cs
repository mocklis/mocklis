// --------------------------------------------------------------------------------------------------------------------
// <copyright file="InstanceRemoveActionEventStep_should.cs">
//   SPDX-License-Identifier: MIT
//   Copyright © 2019 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.Tests.Steps.Lambda
{
    #region Using Directives

    using System;
    using Mocklis.Tests.Interfaces;
    using Mocklis.Tests.Mocks;
    using Xunit;

    #endregion

    public class InstanceRemoveActionEventStep_should
    {
        public MockMembers MockMembers { get; } = new MockMembers();
        public IEvents Sut => MockMembers;

        public EventHandler HandlerInstance { get; } = (e, s) => { };

        [Fact]
        public void RequireNonNullAction()
        {
            Assert.Throws<ArgumentNullException>(() => MockMembers.MyEvent.InstanceRemoveAction(null));
        }

        [Fact]
        public void invoke_action_on_remove()
        {
            object callInstance = null;
            EventHandler addedInstance = null;

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
        public void forward_adds()
        {
            MockMembers.MyEvent.InstanceRemoveAction((o, v) => throw new InvalidOperationException()).RecordBeforeAdd(out var ledger, v => v).Dummy();

            Sut.MyEvent += HandlerInstance;

            Assert.Equal(new[] { HandlerInstance }, ledger);
        }
    }
}
