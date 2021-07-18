// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ExpectedUsagePropertyStepTests.cs">
//   SPDX-License-Identifier: MIT
//   Copyright © 2019-2021 Esbjörn Redmo and contributors. All rights reserved.
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

    public class ExpectedUsagePropertyStepTests
    {
        private MockMembers MockMembers { get; }
        private IProperties Properties { get; }
        private VerificationGroup Group { get; }

        public ExpectedUsagePropertyStepTests()
        {
            Properties = MockMembers = new MockMembers();
            Group = new VerificationGroup();
        }

        [Fact]
        public void RequireVerificationGroup()
        {
            Assert.Throws<ArgumentNullException>(() => MockMembers.StringProperty.ExpectedUsage(null!, "test"));
        }

        [Fact]
        public void NotRequireName()
        {
            MockMembers.Item.ExpectedUsage(Group, null, 0, 0);
            var groupResult = ((IVerifiable)Group).Verify();
            var result = Assert.Single(groupResult);
            Assert.True(result.Success);
            var subResults = result.SubResults.ToArray();
            Assert.Equal(2, subResults.Length);
            Assert.Equal("Usage Count: Expected 0 get(s); received 0 get(s).", subResults[0].Description);
            Assert.Equal("Usage Count: Expected 0 set(s); received 0 set(s).", subResults[1].Description);
        }

        [Fact]
        public void NotRequireExpectedValues()
        {
            MockMembers.StringProperty.ExpectedUsage(Group, "Usage");
            var groupResult = ((IVerifiable)Group).Verify();
            var result = Assert.Single(groupResult);
            Assert.True(result.Success);
            Assert.Empty(result.SubResults);
        }

        [Fact]
        public void ExpectedGetsMustNotBeNegative()
        {
            var x = Assert.Throws<ArgumentOutOfRangeException>(() => MockMembers.StringProperty.ExpectedUsage(Group, null, -1));
            Assert.Equal("expectedNumberOfGets", x.ParamName);
        }

        [Fact]
        public void ExpectedSetsMustNotBeNegative()
        {
            var x = Assert.Throws<ArgumentOutOfRangeException>(() => MockMembers.StringProperty.ExpectedUsage(Group, null, null, -1));
            Assert.Equal("expectedNumberOfSets", x.ParamName);
        }


        [Fact]
        public void AcceptOnlyGets()
        {
            MockMembers.StringProperty.ExpectedUsage(Group, "Test", 1);
            var groupResult = ((IVerifiable)Group).Verify();
            var result = Assert.Single(groupResult);
            Assert.False(result.Success);
            var subResults = result.SubResults.ToArray();
            var adds = Assert.Single(subResults);
            Assert.Equal("Usage Count 'Test': Expected 1 get(s); received 0 get(s).", adds.Description);
        }

        [Fact]
        public void AcceptOnlySets()
        {
            MockMembers.StringProperty.ExpectedUsage(Group, "Test", null, 1);
            var groupResult = ((IVerifiable)Group).Verify();
            var result = Assert.Single(groupResult);
            Assert.False(result.Success);
            var subResults = result.SubResults.ToArray();
            var adds = Assert.Single(subResults);
            Assert.Equal("Usage Count 'Test': Expected 1 set(s); received 0 set(s).", adds.Description);
        }

        [Fact]
        public void CountAndReportSuccessfulGetsAndFailedSets()
        {
            MockMembers.StringProperty.ExpectedUsage(Group, "Property", 1, 2);
            Properties.StringProperty = "Hello";
            var _ = Properties.StringProperty;

            var ex = Assert.Throws<VerificationFailedException>(() => Group.Assert());
            ex.VerificationResult.AssertEquals(
                new VerificationResult("Verification Group:", new[]
                {
                    new VerificationResult("Usage Count 'Property': Expected 1 get(s); received 1 get(s).", true),
                    new VerificationResult("Usage Count 'Property': Expected 2 set(s); received 1 set(s).", false)
                }));
        }

        [Fact]
        public void CountAndReportFailedGetsAndSuccessfulSets()
        {
            MockMembers.StringProperty.ExpectedUsage(Group, "Property", 2, 1);
            Properties.StringProperty = "Hello";
            var _ = Properties.StringProperty;

            var ex = Assert.Throws<VerificationFailedException>(() => Group.Assert());
            ex.VerificationResult.AssertEquals(
                new VerificationResult("Verification Group:", new[]
                {
                    new VerificationResult("Usage Count 'Property': Expected 2 get(s); received 1 get(s).", false),
                    new VerificationResult("Usage Count 'Property': Expected 1 set(s); received 1 set(s).", true)
                }));
        }
    }
}
