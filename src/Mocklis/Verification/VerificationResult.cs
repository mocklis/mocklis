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
#if NETSTANDARD2_0
    using System.Runtime.Serialization;
    using System.Security.Permissions;
#endif

    #endregion

#if NETSTANDARD2_0
    /// <summary>
    ///     Struct that contains the result of a verification check. It is either a leaf node (and its success depends
    ///     only on that check) or a branch node (and it's deemed successful only if all its child nodes are successful.)
    ///     Implements the <see cref="ISerializable" /> interface.
    /// </summary>
    /// <seealso cref="ISerializable" />
    [Serializable]
    public readonly struct VerificationResult : ISerializable
#else
    /// <summary>
    ///     Struct that contains the result of a verification check. It is either a leaf node (and its success depends
    ///     only on that check) or a branch node (and it's deemed successful only if all its child nodes are successful.)
    /// </summary>
    public readonly struct VerificationResult
#endif
    {
        /// <summary>
        ///     Gets the description of the verification node.
        /// </summary>
        public string Description { get; }

        /// <summary>
        ///     Gets a list with subresults. If this is a leaf node then this list is empty.
        /// </summary>
        public IReadOnlyList<VerificationResult> SubResults { get; }

        /// <summary>
        ///     Gets a value indicating whether this <see cref="VerificationResult" /> represents a successful verification.
        /// </summary>
        public bool Success { get; }

        /// <summary>
        ///     Initializes a new instance of the <see cref="VerificationResult" /> struct for a leaf node.
        /// </summary>
        /// <param name="description">The description of the verification node.</param>
        /// <param name="success">A value indicating whether the verification corresponding to this node was a success.</param>
        public VerificationResult(string description, bool success)
        {
            Description = description;
            SubResults = Array.Empty<VerificationResult>();
            Success = success;
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="VerificationResult" /> struct for a branch node.
        /// </summary>
        /// <param name="description">The description of the verification node.</param>
        /// <param name="subResults">A list of verification nodes who together build up to this node.</param>
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

        /// <summary>
        ///     Returns a <see cref="System.String" /> that represents this instance, recursing over any sub-verification nodes.
        /// </summary>
        public override string ToString()
        {
            return ToString(true);
        }

        /// <summary>
        ///     Returns a <see cref="System.String" /> that represents this instance, recursing over any sub-verification nodes..
        /// </summary>
        /// <param name="includeSuccessfulVerifications">
        ///     A value indicating whether to include successful verifications in the
        ///     result.
        /// </param>
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

        /// <summary>
        ///     Sets the <see cref="SerializationInfo" /> with information about the verification result.
        /// </summary>
        /// <param name="info">
        ///     The <see cref="SerializationInfo" /> that holds the serialized object data about the verification result.
        /// </param>
        /// <param name="context">
        ///     The <see cref="StreamingContext" /> that contains contextual information about the source or
        ///     destination.
        /// </param>
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
