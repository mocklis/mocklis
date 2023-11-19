// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ExpectedUsageEventStepTests.cs">
//   SPDX-License-Identifier: MIT
//   Copyright © 2019-2023 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.Verification.Steps
{
    #region Using Directives

    using System;
    using System.Linq;
    using Mocklis.Helpers;
    using Mocklis.Interfaces;
    using Mocklis.Mocks;
    using Xunit;

    #endregion

    public class ExpectedUsageEventStepTests
    {
        private MockMembers MockMembers { get; }
        private IEvents Events { get; }
        private VerificationGroup Group { get; }
        private readonly EventHandler _handler = (sender, args) => { };

        public ExpectedUsageEventStepTests()
        {
            Events = MockMembers = new MockMembers();
            Group = new VerificationGroup();
        }

        [Fact]
        public void RequireVerificationGroup()
        {
            Assert.Throws<ArgumentNullException>(() => MockMembers.MyEvent.ExpectedUsage(null!, "test"));
        }

        [Fact]
        public void NotRequireName()
        {
            MockMembers.MyEvent.ExpectedUsage(Group, null, 0, 0);
            var groupResult = ((IVerifiable)Group).Verify();
            var result = Assert.Single(groupResult);
            Assert.True(result.Success);
            var subResults = result.SubResults.ToArray();
            Assert.Equal(2, subResults.Length);
            Assert.Equal("Usage Count: Expected 0 add(s); received 0 add(s).", subResults[0].Description);
            Assert.Equal("Usage Count: Expected 0 remove(s); received 0 remove(s).", subResults[1].Description);
        }

        [Fact]
        public void NotRequireExpectedValues()
        {
            MockMembers.MyEvent.ExpectedUsage(Group, "Usage");
            var groupResult = ((IVerifiable)Group).Verify();
            var result = Assert.Single(groupResult);
            Assert.True(result.Success);
            Assert.Empty(result.SubResults);
        }

        [Fact]
        public void ExpectedAddsMustNotBeNegative()
        {
            var x = Assert.Throws<ArgumentOutOfRangeException>(() => MockMembers.MyEvent.ExpectedUsage(Group, null, -1));
            Assert.Equal("expectedNumberOfAdds", x.ParamName);
        }

        [Fact]
        public void ExpectedRemovesMustNotBeNegative()
        {
            var x = Assert.Throws<ArgumentOutOfRangeException>(() => MockMembers.MyEvent.ExpectedUsage(Group, null, null, -1));
            Assert.Equal("expectedNumberOfRemoves", x.ParamName);
        }

        [Fact]
        public void AcceptOnlyAdds()
        {
            MockMembers.MyEvent.ExpectedUsage(Group, "Test", 1);
            var groupResult = ((IVerifiable)Group).Verify();
            var result = Assert.Single(groupResult);
            Assert.False(result.Success);
            var subResults = result.SubResults.ToArray();
            var adds = Assert.Single(subResults);
            Assert.Equal("Usage Count 'Test': Expected 1 add(s); received 0 add(s).", adds.Description);
        }

        [Fact]
        public void AcceptOnlyRemoves()
        {
            MockMembers.MyEvent.ExpectedUsage(Group, "Test", null, 1);
            var groupResult = ((IVerifiable)Group).Verify();
            var result = Assert.Single(groupResult);
            Assert.False(result.Success);
            var subResults = result.SubResults.ToArray();
            var adds = Assert.Single(subResults);
            Assert.Equal("Usage Count 'Test': Expected 1 remove(s); received 0 remove(s).", adds.Description);
        }

        [Fact]
        public void CountAndReportSuccessfulAddsAndFailedRemoves()
        {
            MockMembers.MyEvent.ExpectedUsage(Group, "Event", 1, 2);
            Events.MyEvent += _handler;
            Events.MyEvent -= _handler;

            var ex = Assert.Throws<VerificationFailedException>(() => Group.Assert());
            ex.VerificationResult.AssertEquals(
                new VerificationResult("Verification Group:", new[]
                {
                    new VerificationResult("Usage Count 'Event': Expected 1 add(s); received 1 add(s).", true),
                    new VerificationResult("Usage Count 'Event': Expected 2 remove(s); received 1 remove(s).", false)
                }));
        }

        [Fact]
        public void CountAndReportFailedAddsAndSuccessfulRemoves()
        {
            MockMembers.MyEvent.ExpectedUsage(Group, "Event", 2, 1);
            Events.MyEvent += _handler;
            Events.MyEvent -= _handler;

            var ex = Assert.Throws<VerificationFailedException>(() => Group.Assert());
            ex.VerificationResult.AssertEquals(
                new VerificationResult("Verification Group:", new[]
                {
                    new VerificationResult("Usage Count 'Event': Expected 2 add(s); received 1 add(s).", false),
                    new VerificationResult("Usage Count 'Event': Expected 1 remove(s); received 1 remove(s).", true)
                }));
        }
    }
}
