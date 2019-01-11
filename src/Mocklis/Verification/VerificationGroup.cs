// --------------------------------------------------------------------------------------------------------------------
// <copyright file="VerificationGroup.cs">
//   Copyright © 2019 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.Verification
{
    #region Using Directives

    using System;
    using System.Collections.Generic;
    using System.Linq;

    #endregion

    public class VerificationGroup : IVerifiable
    {
        private readonly List<IVerifiable> _verifiables = new List<IVerifiable>();

        public string Name { get; }

        public VerificationGroup(string name = null)
        {
            Name = name;
        }

        public VerificationResult Verify()
        {
            string description = string.IsNullOrEmpty(Name) ? "Verification Group:" : $"Verification Group '{Name}':";
            return new VerificationResult(description, _verifiables.SelectMany(a => a.Verify()));
        }

        IEnumerable<VerificationResult> IVerifiable.Verify()
        {
            yield return Verify();
        }

        public void Add(IVerifiable verifiable)
        {
            _verifiables.Add(verifiable);
        }

        public void Assert(bool includeSuccessfulVerifications = false)
        {
            VerificationResult result = Verify();
            if (!result.Success)
            {
                var message = "Verification failed." + Environment.NewLine + Environment.NewLine + result.ToString(includeSuccessfulVerifications);
                throw new VerificationFailedException(result, message);
            }
        }
    }
}
