// --------------------------------------------------------------------------------------------------------------------
// <copyright file="VerificationGroupTests.cs">
//   SPDX-License-Identifier: MIT
//   Copyright © 2019-2024 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.Verification
{
    #region Using Directives

    using System;
    using System.Globalization;
    using System.Linq;
    using Mocklis.Helpers;
    using Mocklis.Mocks;
    using Xunit;

    #endregion

    public class VerificationGroupTests
    {
        [Fact]
        public void ReturnSingleSuccessfulResultIfEmpty()
        {
            IVerifiable group = new VerificationGroup();
            var groupResult = group.Verify();
            var result = Assert.Single(groupResult);
            Assert.True(result.Success);
            Assert.Equal("Verification Group:", result.Description);
        }

        [Fact]
        public void AcceptName()
        {
            IVerifiable group = new VerificationGroup("Group Name");
            var groupResult = group.Verify();
            var result = Assert.Single(groupResult);
            Assert.True(result.Success);
            Assert.Equal("Verification Group 'Group Name':", result.Description);
        }

        [Fact]
        public void AllowAddingNewVerifiables()
        {
            var verifiable1 = new MockVerifiable();
            verifiable1.Verify.Return(new[] { new VerificationResult("My Verification", false) });

            var verifiable2 = new MockVerifiable();
            verifiable2.Verify.Return(new[] { new VerificationResult("My other verification", true) });

            IVerifiable group = new VerificationGroup("My Group")
            {
                verifiable1,
                verifiable2
            };

            var groupResult = group.Verify();
            var result = Assert.Single(groupResult);

            var expectedResult = new VerificationResult("Verification Group 'My Group':", new[]
            {
                new VerificationResult("My Verification", false),
                new VerificationResult("My other verification", true)
            });

            result.AssertEquals(expectedResult);
        }

        [Fact]
        public void ExposesVerifiablesThroughIEnumerable()
        {
            var verifiable1 = new MockVerifiable();
            verifiable1.Verify.Return(new[] { new VerificationResult("My Verification", false) });

            var verifiable2 = new MockVerifiable();
            verifiable2.Verify.Return(new[] { new VerificationResult("My other verification", true) });

            var group = new VerificationGroup("My Group")
            {
                verifiable1,
                verifiable2
            };

            Assert.Equal(new IVerifiable[] { verifiable1, verifiable2 }, group);
            Assert.Equal(new IVerifiable[] { verifiable1, verifiable2 }, group.ToArray());
        }

        [Fact]
        public void PassFormatProviderToGroupMembers()
        {
            var verifiable = new MockVerifiable();
            verifiable.Verify
                .RecordBeforeCall(out var ledger)
                .Return(new[] { new VerificationResult("aha", true) });

            IVerifiable group = new VerificationGroup("My Group")
            {
                verifiable
            };

            var french = new CultureInfo("fr-FR");
            var japanese = new CultureInfo("ja-JP");

            group.Verify(french);
            group.Verify(japanese);

            Assert.Equal(new IFormatProvider[] { french, japanese }, ledger);
            Assert.Equal(new IFormatProvider[] { french, japanese }, ledger.ToArray());
        }

        [Fact]
        public void ThrowWhenAssertingFailedGroup()
        {
            var verifiable = new MockVerifiable();
            verifiable.Verify
                .Return(new[] { new VerificationResult("a failing test", false) });

            var group = new VerificationGroup("My Group")
            {
                verifiable
            };

            var ex = Assert.Throws<VerificationFailedException>(() => group.Assert());
            ex.VerificationResult.AssertEquals(new VerificationResult("Verification Group 'My Group':", new[]
            {
                new VerificationResult("a failing test", false)
            }));

            Assert.Equal(@"Verification failed.

FAILED: Verification Group 'My Group':
FAILED:   a failing test", ex.Message);
        }

        [Fact]
        public void NotThrowWhenAssertingSuccessfulGroup()
        {
            var verifiable = new MockVerifiable();
            verifiable.Verify
                .Return(new[] { new VerificationResult("a passing test", true) });

            var group = new VerificationGroup("My Group")
            {
                verifiable
            };

            group.Assert();
        }
    }
}
