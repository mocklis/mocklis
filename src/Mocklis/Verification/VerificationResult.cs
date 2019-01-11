// --------------------------------------------------------------------------------------------------------------------
// <copyright file="VerificationResult.cs">
//   Copyright © 2019 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.Verification
{
    #region Using Directives

    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Text;

    #endregion

#if NETSTANDARD2_0
    using System.Runtime.Serialization;
    using System.Security.Permissions;

    [Serializable]
    public struct VerificationResult : ISerializable
#else
    public struct VerificationResult
#endif
    {
        public string Description { get; }
        public IReadOnlyList<VerificationResult> SubResults { get; }
        public bool Success { get; }

        public VerificationResult(string description, bool success)
        {
            Description = description;
            SubResults = Array.Empty<VerificationResult>();
            Success = success;
        }

        public VerificationResult(string description, IEnumerable<VerificationResult> subResults)
        {
            Description = description;
            if (subResults is ReadOnlyCollection<VerificationResult> readOnlyCollection)
            {
                SubResults = readOnlyCollection;
            }
            else
            {
                SubResults =
                    new ReadOnlyCollection<VerificationResult>(
                        subResults?.ToArray() ?? Array.Empty<VerificationResult>());
            }

            Success = SubResults.All(sr => sr.Success);
        }

        public override string ToString()
        {
            return ToString(true);
        }

        public string ToString(bool includeSuccessfulVerifications)
        {
            StringBuilder sb = new StringBuilder();

            AddToStringBuilder(sb, 0, includeSuccessfulVerifications);

            return sb.ToString();
        }

        private void AddToStringBuilder(StringBuilder sb, int indentationLevel, bool includeSuccessfulVerifications)
        {
            if (!Success || includeSuccessfulVerifications)
            {
                string line = (Success ? "Passed: " : "FAILED: ") + new string(' ', indentationLevel * 2) + Description;
                sb.AppendLine(line);
                if (SubResults != null)
                {
                    foreach (var subResult in SubResults)
                    {
                        subResult.AddToStringBuilder(sb, indentationLevel + 1, includeSuccessfulVerifications);
                    }
                }
            }
        }

#if NETSTANDARD2_0
        private VerificationResult(SerializationInfo info, StreamingContext context)
        {
            Description = info.GetString(nameof(Description));
            SubResults = (IReadOnlyList<VerificationResult>)info.GetValue(nameof(SubResults), typeof(IReadOnlyList<VerificationResult>));
            Success = info.GetBoolean(nameof(Success));
        }

        [SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue(nameof(Description), Description);
            info.AddValue(nameof(SubResults), SubResults);
            info.AddValue(nameof(Success), Success);
        }
#endif
    }
}
