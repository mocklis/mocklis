// --------------------------------------------------------------------------------------------------------------------
// <copyright file="InstanceIfEventStepTests.cs">
//   SPDX-License-Identifier: MIT
//   Copyright © 2019-2021 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.Steps.Conditional
{
    #region Using Directives

    using System;
    using Mocklis.Interfaces;
    using Mocklis.Mocks;
    using Mocklis.Steps.Stored;
    using Mocklis.Verification;
    using Xunit;

    #endregion

    public class InstanceIfEventStepTests
    {
        private int _firstEventHandlerCallCount;

        private void MyFirstEventHandler(object? sender, EventArgs e)
        {
            _firstEventHandlerCallCount++;
        }


        public MockMembers MockMembers { get; } = new MockMembers();
        public IEvents Sut => MockMembers;

        [Fact]
        public void CheckCommonCondition()
        {
            StoredEventStep<EventHandler>? eventStore = null;
            MockMembers.MyEvent.InstanceIf((instance, e) => ((IProperties)instance).StringProperty == "Go", i => i.Stored(out eventStore));
            MockMembers.StringProperty.Stored("");

            MockMembers.StringProperty.Value = "Go";
            Sut.MyEvent += MyFirstEventHandler;
            eventStore!.Raise(this, EventArgs.Empty);
            MockMembers.StringProperty.Value = "Don't go";
            Sut.MyEvent -= MyFirstEventHandler;
            eventStore!.Raise(this, EventArgs.Empty);

            Assert.Equal(2, _firstEventHandlerCallCount);
        }

        [Fact]
        public void CheckSeparateConditions()
        {
            StoredEventStep<EventHandler>? eventStore = null;
            MockMembers.MyEvent.InstanceIf(
                (instance, e) => ((IProperties)instance).StringProperty == "Go",
                (instance, e) => ((IProperties)instance).IntProperty == 42,
                i => i.Stored(out eventStore));
            MockMembers.StringProperty.Stored("");
            MockMembers.IntProperty.Stored();

            MockMembers.StringProperty.Value = "Go";
            MockMembers.IntProperty.Value = 99;
            Sut.MyEvent += MyFirstEventHandler;
            eventStore!.Raise(this, EventArgs.Empty);

            MockMembers.StringProperty.Value = "Don't go";
            MockMembers.IntProperty.Value = 42;
            Sut.MyEvent -= MyFirstEventHandler;
            eventStore!.Raise(this, EventArgs.Empty);

            Assert.Equal(1, _firstEventHandlerCallCount);
        }

        [Fact]
        public void CallBaseIfConditionsAreNotMet()
        {
            var group = new VerificationGroup();
            MockMembers.MyEvent
                .InstanceIf((i, h) => false, (i, h) => false, b => b.ExpectedUsage(group, null, 0, 0))
                .ExpectedUsage(group, null, 1, 1);

            Sut.MyEvent += MyFirstEventHandler;
            Sut.MyEvent -= MyFirstEventHandler;

            group.Assert();
        }
    }
}
