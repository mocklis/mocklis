// --------------------------------------------------------------------------------------------------------------------
// <copyright file="VerificationResultTests.cs">
//   SPDX-License-Identifier: MIT
//   Copyright © 2019-2023 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.Verification
{
    #region Using Directives

    using System;
    using Xunit;

#if NETFRAMEWORK
    using Mocklis.Helpers;
#endif

    #endregion

    public class VerificationResultTests
    {
        private readonly VerificationResult _successLeaf;
        private readonly VerificationResult _failureLeaf;
        private readonly VerificationResult _emptyBranch;
        private readonly VerificationResult _successfulBranch;
        private readonly VerificationResult _sut;

        public VerificationResultTests()
        {
            _successLeaf = new VerificationResult("Successful Leaf", true);

            _failureLeaf = new VerificationResult("Failed Leaf", false);

            _emptyBranch = new VerificationResult("Empty Branch", Array.Empty<VerificationResult>());

            _successfulBranch = new VerificationResult("Branch", new[]
            {
                new VerificationResult("Success1", true),
                new VerificationResult("Success2", true)
            });

            _sut = new VerificationResult("Branch1", new[]
            {
                _successLeaf,
                new VerificationResult("Branch2", new[]
                {
                    new VerificationResult("InnerLeaf1", false),
                    new VerificationResult("InnerLeaf2", true)
                }),
                _failureLeaf,
                _successfulBranch,
                _emptyBranch
            });
        }

        [Fact]
        public void HaveRightPropertiesForSuccessfulLeaf()
        {
            Assert.Equal("Successful Leaf", _successLeaf.Description);
            Assert.True(_successLeaf.Success);
            Assert.Empty(_successLeaf.SubResults);
        }

        [Fact]
        public void HaveRightPropertiesForFailedLeaf()
        {
            Assert.Equal("Failed Leaf", _failureLeaf.Description);
            Assert.False(_failureLeaf.Success);
            Assert.Empty(_failureLeaf.SubResults);
        }

        [Fact]
        public void HaveRightPropertiesForBranchWithNoChildren()
        {
            Assert.Equal("Empty Branch", _emptyBranch.Description);
            Assert.True(_emptyBranch.Success);
            Assert.Empty(_emptyBranch.SubResults);
        }

        [Fact]
        public void ReturnCorrectSuccessString()
        {
            Assert.Equal("Passed: Successful Leaf", _successLeaf.ToString());
        }

        [Fact]
        public void ReturnCorrectFailureString()
        {
            Assert.Equal("FAILED: Failed Leaf", _failureLeaf.ToString());
        }

        [Fact]
        public void FailureIfAnyLeafNodeIsFailure()
        {
            Assert.False(_sut.Success);
        }

        [Fact]
        public void SuccessIfAllLeafNodesSucceed()
        {
            Assert.True(_successfulBranch.Success);
        }

        [Fact]
        public void ToStringWithParameterRecursesAcrossAllNodes()
        {
            // ReSharper disable once ImpureMethodCallOnReadonlyValueField
            Assert.Equal(@"FAILED: Branch1
Passed:   Successful Leaf
FAILED:   Branch2
FAILED:     InnerLeaf1
Passed:     InnerLeaf2
FAILED:   Failed Leaf
Passed:   Branch
Passed:     Success1
Passed:     Success2
Passed:   Empty Branch", _sut.ToString(true));
        }

        [Fact]
        public void ToStringWithParameterCanReturnFailedNodesOnly()
        {
            // ReSharper disable once ImpureMethodCallOnReadonlyValueField
            Assert.Equal(@"FAILED: Branch1
FAILED:   Branch2
FAILED:     InnerLeaf1
FAILED:   Failed Leaf", _sut.ToString(false));
        }

        [Fact]
        public void ToStringReturnsAllNodes()
        {
            // ReSharper disable once ImpureMethodCallOnReadonlyValueField
            Assert.Equal(_sut.ToString(true), _sut.ToString());
        }

#if NETFRAMEWORK
        [Fact]
        public void BeSerializable()
        {
            var roundtripped = _sut.RoundTripWithBinaryFormatter();
            roundtripped.AssertEquals(_sut);
        }

#endif
    }
}
