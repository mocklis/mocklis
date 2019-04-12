// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RemoveActionEventStep_should.cs">
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

    public class RemoveActionEventStep_should
    {
        public MockMembers MockMembers { get; } = new MockMembers();
        public IEvents Sut => MockMembers;

        public EventHandler HandlerInstance { get; } = (e, s) => { };

        [Fact]
        public void RequireNonNullAction()
        {
            Assert.Throws<ArgumentNullException>(() => MockMembers.MyEvent.RemoveAction(null));
        }

        [Fact]
        public void invoke_action_on_remove()
        {
            EventHandler addedInstance = null;

            MockMembers.MyEvent.RemoveAction(i => addedInstance = i);

            Sut.MyEvent -= HandlerInstance;

            Assert.Same(HandlerInstance, addedInstance);
        }

        [Fact]
        public void forward_adds()
        {
            MockMembers.MyEvent.RemoveAction(_ => throw new InvalidOperationException()).RecordBeforeAdd(out var ledger).Dummy();

            Sut.MyEvent += HandlerInstance;

            Assert.Equal(new[] { HandlerInstance }, ledger);
        }
    }
}
