// --------------------------------------------------------------------------------------------------------------------
// <copyright file="InstanceIfEventStep_should.cs">
//   Copyright © 2019 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.Tests.Steps.Conditional
{
    #region Using Directives

    using System;
    using Mocklis.Steps.Stored;
    using Mocklis.Tests.Interfaces;
    using Mocklis.Tests.Mocks;
    using Mocklis.Verification;
    using Xunit;

    #endregion

    public class InstanceIfEventStep_should
    {
        private int _firstEventHandlerCallCount;

        private void MyFirstEventHandler(object sender, EventArgs e)
        {
            _firstEventHandlerCallCount++;
        }


        public MockMembers MockMembers { get; } = new MockMembers();
        public IEvents Sut => MockMembers;

        [Fact]
        public void check_common_condition()
        {
            StoredEventStep<EventHandler> eventStore = null;
            MockMembers.MyEvent.InstanceIf((instance, e) => ((IProperties)instance).Name == "Go", i => i.Stored(out eventStore)).Dummy();
            MockMembers.Name.Stored();

            MockMembers.Name.Value = "Go";
            Sut.MyEvent += MyFirstEventHandler;
            eventStore.Raise(this, EventArgs.Empty);
            MockMembers.Name.Value = "Don't go";
            Sut.MyEvent -= MyFirstEventHandler;
            eventStore.Raise(this, EventArgs.Empty);

            Assert.Equal(2, _firstEventHandlerCallCount);
        }

        [Fact]
        public void check_separate_conditions()
        {
            StoredEventStep<EventHandler> eventStore = null;
            MockMembers.MyEvent.InstanceIf(
                (instance, e) => ((IProperties)instance).Name == "Go",
                (instance, e) => ((IProperties)instance).Age == 42,
                i => i.Stored(out eventStore)).Dummy();
            MockMembers.Name.Stored();
            MockMembers.Age.Stored();


            MockMembers.Name.Value = "Go";
            MockMembers.Age.Value = 99;
            Sut.MyEvent += MyFirstEventHandler;
            eventStore.Raise(this, EventArgs.Empty);

            MockMembers.Name.Value = "Don't go";
            MockMembers.Age.Value = 42;
            Sut.MyEvent -= MyFirstEventHandler;
            eventStore.Raise(this, EventArgs.Empty);

            Assert.Equal(1, _firstEventHandlerCallCount);
        }
    }
}
