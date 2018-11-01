// --------------------------------------------------------------------------------------------------------------------
// <copyright file="VerificationResult.cs">
//   Copyright © 2018 Esbjörn Redmo and contributors. All rights reserved.
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

    public struct VerificationResult
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
            StringBuilder sb = new StringBuilder();

            AddToStringBuilder(sb, 0);

            return sb.ToString();
        }

        private void AddToStringBuilder(StringBuilder sb, int indentationLevel)
        {
            string line = (Success ? "Passed: " : "FAILED: ") + new string(' ', indentationLevel * 2) + Description;
            sb.AppendLine(line);
            if (SubResults != null)
            {
                foreach (var subResult in SubResults)
                {
                    subResult.AddToStringBuilder(sb, indentationLevel + 1);
                }
            }
        }
    }
}
