// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ExpectedUsageMethodStepTests.cs">
//   SPDX-License-Identifier: MIT
//   Copyright © 2019-2021 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.Verification.Steps
{
    #region Using Directives

    using System;
    using Mocklis.Helpers;
    using Mocklis.Interfaces;
    using Mocklis.Mocks;
    using Xunit;

    #endregion

    public class ExpectedUsageMethodStepTests
    {
        private MockMembers MockMembers { get; }
        private IMethods Methods { get; }
        private VerificationGroup Group { get; }

        public ExpectedUsageMethodStepTests()
        {
            Methods = MockMembers = new MockMembers();
            Group = new VerificationGroup();
        }

        [Fact]
        public void RequireVerificationGroup()
        {
            Assert.Throws<ArgumentNullException>(() => MockMembers.FuncWithParameter.ExpectedUsage(null!, "test"));
        }

        [Fact]
        public void NotRequireName()
        {
            MockMembers.FuncWithParameter.ExpectedUsage(Group, null, 0);
            var groupResult = ((IVerifiable)Group).Verify();
            var result = Assert.Single(groupResult);
            Assert.True(result.Success);
            var subResult = Assert.Single(result.SubResults);
            Assert.Equal("Usage Count: Expected 0 call(s); received 0 call(s).", subResult.Description);
        }

        [Fact]
        public void NotRequireExpectedValue()
        {
            MockMembers.FuncWithParameter.ExpectedUsage(Group, "Usage");
            var groupResult = ((IVerifiable)Group).Verify();
            var result = Assert.Single(groupResult);
            Assert.True(result.Success);
            Assert.Empty(result.SubResults);
        }

        [Fact]
        public void ExpectedCallsMustNotBeNegative()
        {
            var x = Assert.Throws<ArgumentOutOfRangeException>(() => MockMembers.FuncWithParameter.ExpectedUsage(Group, null, -1));
            Assert.Equal("expectedNumberOfCalls", x.ParamName);
        }

        [Fact]
        public void NotThrowOnRightNumberOfCalls()
        {
            MockMembers.FuncWithParameter.ExpectedUsage(Group, "Method", 1);
            Methods.FuncWithParameter(13);

            Group.Assert();
        }

        [Fact]
        public void CountAndReportWrongNumberOfCalls()
        {
            MockMembers.FuncWithParameter.ExpectedUsage(Group, "Method", 2);
            Methods.FuncWithParameter(13);

            var ex = Assert.Throws<VerificationFailedException>(() => Group.Assert());
            ex.VerificationResult.AssertEquals(
                new VerificationResult("Verification Group:", new[]
                {
                    new VerificationResult("Usage Count 'Method': Expected 2 call(s); received 1 call(s).", false)
                }));
        }
    }
}
