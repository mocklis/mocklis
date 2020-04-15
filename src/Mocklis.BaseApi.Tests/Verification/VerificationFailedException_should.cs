// --------------------------------------------------------------------------------------------------------------------
// <copyright file="VerificationFailedException_should.cs">
//   SPDX-License-Identifier: MIT
//   Copyright © 2019-2020 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.Verification
{
    #region Using Directives

    using System;
    using Mocklis.Helpers;
    using Xunit;

    #endregion

    public class VerificationFailedException_should
    {
        [Fact]
        public void AcceptVerificationResult()
        {
            var verificationResult = new VerificationResult("Aha", false);
            var sut = new VerificationFailedException(verificationResult);

            sut.VerificationResult.AssertEquals(verificationResult);
            Assert.Equal("Verification failed.", sut.Message);
            Assert.Null(sut.InnerException);
        }

        [Fact]
        public void AcceptVerificationResultAndMessage()
        {
            var verificationResult = new VerificationResult("Aha", false);
            var sut = new VerificationFailedException(verificationResult, "This is a bespoke message!");

            sut.VerificationResult.AssertEquals(verificationResult);
            Assert.Equal("This is a bespoke message!", sut.Message);
            Assert.Null(sut.InnerException);
        }

        [Fact]
        public void AcceptVerificationResultMessageAndInnerException()
        {
            var innerException = new Exception();
            var verificationResult = new VerificationResult("Aha", false);
            var sut = new VerificationFailedException(verificationResult, "This is a message!", innerException);

            sut.VerificationResult.AssertEquals(verificationResult);
            Assert.Equal("This is a message!", sut.Message);
            Assert.Same(innerException, sut.InnerException);
        }

#if !NETCOREAPP1_1
        [Fact]
        public void BeSerialisable()
        {
            var verificationResult = new VerificationResult("Aha", false);
            var sut = new VerificationFailedException(verificationResult, "This is a special message!").RoundTripWithBinaryFormatter();

            sut.VerificationResult.AssertEquals(verificationResult);
            Assert.Equal("This is a special message!", sut.Message);
            Assert.Null(sut.InnerException);
        }

#endif
    }
}
