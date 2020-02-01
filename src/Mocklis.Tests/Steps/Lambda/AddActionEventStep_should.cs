// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AddActionEventStep_should.cs">
//   SPDX-License-Identifier: MIT
//   Copyright © 2019-2020 Esbjörn Redmo and contributors. All rights reserved.
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

    public class AddActionEventStep_should
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
        public void invoke_action_on_add()
        {
            EventHandler? addedInstance = null;

            MockMembers.MyEvent.AddAction(i => addedInstance = i);

            Sut.MyEvent += HandlerInstance;

            Assert.Same(HandlerInstance, addedInstance);
        }

        [Fact]
        public void forward_removes()
        {
            MockMembers.MyEvent.AddAction(_ => throw new InvalidOperationException()).RecordBeforeRemove(out var ledger);

            Sut.MyEvent -= HandlerInstance;

            Assert.Equal(new[] { HandlerInstance }, ledger);
        }
    }
}
