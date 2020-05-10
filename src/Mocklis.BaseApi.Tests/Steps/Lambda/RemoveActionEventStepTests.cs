// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RemoveActionEventStepTests.cs">
//   SPDX-License-Identifier: MIT
//   Copyright © 2019-2020 Esbjörn Redmo and contributors. All rights reserved.
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

    public class RemoveActionEventStepTests
    {
        public MockMembers MockMembers { get; } = new MockMembers();
        public IEvents Sut => MockMembers;

        public EventHandler HandlerInstance { get; } = (e, s) => { };

        [Fact]
        public void RequireNonNullAction()
        {
            Assert.Throws<ArgumentNullException>(() => MockMembers.MyEvent.RemoveAction(null!));
        }

        [Fact]
        public void InvokeActionOnRemove()
        {
            EventHandler? addedInstance = null;

            MockMembers.MyEvent.RemoveAction(i => addedInstance = i);

            Sut.MyEvent -= HandlerInstance;

            Assert.Same(HandlerInstance, addedInstance);
        }

        [Fact]
        public void ForwardAdds()
        {
            MockMembers.MyEvent.RemoveAction(_ => throw new InvalidOperationException()).RecordBeforeAdd(out var ledger);

            Sut.MyEvent += HandlerInstance;

            Assert.Equal(new[] { HandlerInstance }, ledger);
        }
    }
}
