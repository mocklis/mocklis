// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StoredEventStepTests.cs">
//   SPDX-License-Identifier: MIT
//   Copyright © 2019-2021 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.Steps.Stored
{
    #region Using Directives

    using System;
    using Mocklis.Interfaces;
    using Mocklis.Mocks;
    using Xunit;

    #endregion

    public class StoredEventStepTests
    {
        public MockMembers MockMembers { get; } = new MockMembers();
        public IEvents Sut => MockMembers;

        private readonly EventHandler _handler1 = (sender, args) => { };
        private readonly EventHandler _handler2 = (sender, args) => { };

        [Fact]
        public void BeBothResultAndOutParameter()
        {
            var step = MockMembers.MyEvent.Stored(out var store);
            Assert.Same(step, store);
        }

        [Fact]
        public void ReturnStoredValues1()
        {
            MockMembers.MyEvent.Stored(out var store);
            Sut.MyEvent += _handler1;
            Sut.MyEvent += _handler2;
            Assert.Equal(new Delegate[] { _handler1, _handler2 }, store.EventHandler?.GetInvocationList());
        }

        [Fact]
        public void ReturnStoredValues2()
        {
            MockMembers.MyEvent.Stored(out var store);
            Sut.MyEvent += _handler1;
            Sut.MyEvent += _handler2;
            Sut.MyEvent -= _handler1;
            Assert.Equal(new Delegate[] { _handler2 }, store.EventHandler?.GetInvocationList());
        }

        [Fact]
        public void AllowExternalModification()
        {
            MockMembers.MyEvent.Stored(out var store);

            store.Add(_handler1);
            Sut.MyEvent += _handler2;

            Assert.Equal(new Delegate[] { _handler1, _handler2 }, store.EventHandler?.GetInvocationList());
        }

        [Fact]
        public void AllowExternalClearing()
        {
            MockMembers.MyEvent.Stored(out var store);
            Sut.MyEvent += _handler1;
            Sut.MyEvent += _handler2;

            store.Clear();

            Assert.Null(store.EventHandler);
        }
    }
}
