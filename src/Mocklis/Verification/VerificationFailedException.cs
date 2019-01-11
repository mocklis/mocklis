// --------------------------------------------------------------------------------------------------------------------
// <copyright file="VerificationFailedException.cs">
//   Copyright © 2019 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.Verification
{
    #region Using Directives

    using System;

    #endregion

#if NETSTANDARD2_0
    using System.Runtime.Serialization;

    [Serializable]
#endif
    public class VerificationFailedException : Exception
    {
        public VerificationResult VerificationResult { get; }

        public VerificationFailedException(VerificationResult verificationResult) : base("Verification failed.")
        {
            VerificationResult = verificationResult;
        }

        public VerificationFailedException(VerificationResult verificationResult, string message) : base(message)
        {
            VerificationResult = verificationResult;
        }

        public VerificationFailedException(VerificationResult verificationResult, string message, Exception innerException) : base(message,
            innerException)
        {
            VerificationResult = verificationResult;
        }

#if NETSTANDARD2_0
        protected VerificationFailedException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
            VerificationResult = (VerificationResult)info.GetValue(nameof(VerificationResult), typeof(VerificationResult));
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
            info.AddValue(nameof(VerificationResult), VerificationResult);
        }
#endif
    }
}
