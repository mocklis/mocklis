// --------------------------------------------------------------------------------------------------------------------
// <copyright file="VerificationStepExtensions.cs">
//   SPDX-License-Identifier: MIT
//   Copyright © 2019-2023 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.Verification
{
    #region Using Directives

    using System;
    using Mocklis.Core;
    using Mocklis.Verification.Steps;

    #endregion

    /// <summary>
    ///     A class with extension methods for adding 'verification' steps to an existing mock or step.
    /// </summary>
    public static class VerificationStepExtensions
    {
        /// <summary>
        ///     Step that checks the number of times event handlers have been added or removed. Adds the check to the verification
        ///     group provided.
        /// </summary>
        /// <typeparam name="THandler">The event handler type for the event.</typeparam>
        /// <param name="caller">The mock or step to which this 'verification' step is added.</param>
        /// <param name="verificationGroup">The verification group to which this check is added.</param>
        /// <param name="name">A name that can be used to identify the check in its group.</param>
        /// <param name="expectedNumberOfAdds">The expected number of times event handlers have been added.</param>
        /// <param name="expectedNumberOfRemoves">The expected number of times event handlers have been removed.</param>
        /// <returns>An <see cref="ICanHaveNextEventStep{THandler}" /> that can be used to add further steps.</returns>
        public static ICanHaveNextEventStep<THandler> ExpectedUsage<THandler>(
            this ICanHaveNextEventStep<THandler> caller,
            VerificationGroup verificationGroup,
            string? name,
            int? expectedNumberOfAdds = null,
            int? expectedNumberOfRemoves = null) where THandler : Delegate
        {
            if (verificationGroup == null)
            {
                throw new ArgumentNullException(nameof(verificationGroup));
            }

            var step = new ExpectedUsageEventStep<THandler>(name, expectedNumberOfAdds, expectedNumberOfRemoves);
            verificationGroup.Add(step);
            return caller.SetNextStep(step);
        }

        /// <summary>
        ///     Step that checks the number of times values have been read from or written to the indexer. Adds the check
        ///     to the verification group provided.
        /// </summary>
        /// <typeparam name="TKey">The type of the indexer key.</typeparam>
        /// <typeparam name="TValue">The type of the indexer value.</typeparam>
        /// <param name="caller">The mock or step to which this 'verification' step is added.</param>
        /// <param name="verificationGroup">The verification group to which this check is added.</param>
        /// <param name="name">A name that can be used to identify the check in its group.</param>
        /// <param name="expectedNumberOfGets">The expected number of times values have been read from the indexer.</param>
        /// <param name="expectedNumberOfSets">The expected number of times values have been written to the indexer.</param>
        /// <returns>An <see cref="ICanHaveNextIndexerStep{TKey, TValue}" /> that can be used to add further steps.</returns>
        public static ICanHaveNextIndexerStep<TKey, TValue> ExpectedUsage<TKey, TValue>(
            this ICanHaveNextIndexerStep<TKey, TValue> caller,
            VerificationGroup verificationGroup,
            string? name,
            int? expectedNumberOfGets = null,
            int? expectedNumberOfSets = null)
        {
            if (verificationGroup == null)
            {
                throw new ArgumentNullException(nameof(verificationGroup));
            }

            var step = new ExpectedUsageIndexerStep<TKey, TValue>(name, expectedNumberOfGets, expectedNumberOfSets);
            verificationGroup.Add(step);
            return caller.SetNextStep(step);
        }

        /// <summary>
        ///     Step that checks the number of times the method has been called. Adds the check to the verification group
        ///     provided.
        /// </summary>
        /// <typeparam name="TParam">The method parameter type.</typeparam>
        /// <typeparam name="TResult">The method return type.</typeparam>
        /// <param name="caller">The mock or step to which this 'verification' step is added.</param>
        /// <param name="verificationGroup">The verification group to which this check is added.</param>
        /// <param name="name">A name that can be used to identify the check in its group.</param>
        /// <param name="expectedNumberOfCalls">The expected number of times the method has been called.</param>
        /// <returns>An <see cref="ICanHaveNextMethodStep{TParam, TResult}" /> that can be used to add further steps.</returns>
        public static ICanHaveNextMethodStep<TParam, TResult> ExpectedUsage<TParam, TResult>(
            this ICanHaveNextMethodStep<TParam, TResult> caller,
            VerificationGroup verificationGroup,
            string? name,
            int? expectedNumberOfCalls = null)
        {
            if (verificationGroup == null)
            {
                throw new ArgumentNullException(nameof(verificationGroup));
            }

            var step = new ExpectedUsageMethodStep<TParam, TResult>(name, expectedNumberOfCalls);
            verificationGroup.Add(step);
            return caller.SetNextStep(step);
        }

        /// <summary>
        ///     Step that checks the number of times values have been read from or written to the property. Adds the check
        ///     to the verification group provided.
        /// </summary>
        /// <typeparam name="TValue">The type of the property.</typeparam>
        /// <param name="caller">The mock or step to which this 'verification' step is added.</param>
        /// <param name="verificationGroup">The verification group to which this check is added.</param>
        /// <param name="name">A name that can be used to identify the check in its group.</param>
        /// <param name="expectedNumberOfGets">The expected number of times values have been read from the property.</param>
        /// <param name="expectedNumberOfSets">The expected number of times values have been written to the property.</param>
        /// <returns>An <see cref="ICanHaveNextPropertyStep{TValue}" /> that can be used to add further steps.</returns>
        public static ICanHaveNextPropertyStep<TValue> ExpectedUsage<TValue>(
            this ICanHaveNextPropertyStep<TValue> caller,
            VerificationGroup verificationGroup,
            string? name,
            int? expectedNumberOfGets = null,
            int? expectedNumberOfSets = null)
        {
            if (verificationGroup == null)
            {
                throw new ArgumentNullException(nameof(verificationGroup));
            }

            var step = new ExpectedUsagePropertyStep<TValue>(name, expectedNumberOfGets, expectedNumberOfSets);
            verificationGroup.Add(step);
            return caller.SetNextStep(step);
        }
    }
}
