// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SetActionIndexerStep.cs">
//   SPDX-License-Identifier: MIT
//   Copyright © 2019-2023 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.Steps.Lambda
{
    #region Using Directives

    using System;
    using Mocklis.Core;

    #endregion

    /// <summary>
    ///     Class that represents a 'SetAction' indexer step.
    ///     Inherits from the <see cref="IndexerStepWithNext{TKey,TValue}" /> class.
    /// </summary>
    /// <typeparam name="TKey">The type of the indexer key.</typeparam>
    /// <typeparam name="TValue">The type of the indexer value.</typeparam>
    /// <seealso cref="IndexerStepWithNext{TKey, TValue}" />
    public class SetActionIndexerStep<TKey, TValue> : IndexerStepWithNext<TKey, TValue>
    {
        private readonly Action<TKey, TValue> _action;

        /// <summary>
        ///     Initializes a new instance of the <see cref="SetActionIndexerStep{TKey,TValue}" /> class.
        /// </summary>
        /// <param name="action">An action to be invoked when the indexer is written to.</param>
        public SetActionIndexerStep(Action<TKey, TValue> action)
        {
            _action = action ?? throw new ArgumentNullException(nameof(action));
        }

        /// <summary>
        ///     Called when a value is written to the indexer.
        ///     This implementation invokes the action with the given key and value.
        /// </summary>
        /// <param name="mockInfo">Information about the mock through which the value is written.</param>
        /// <param name="key">The indexer key used.</param>
        /// <param name="value">The value being written</param>
        public override void Set(IMockInfo mockInfo, TKey key, TValue value)
        {
            _action(key, value);
        }
    }
}
