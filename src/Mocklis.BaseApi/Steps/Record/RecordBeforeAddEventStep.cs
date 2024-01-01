// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RecordBeforeAddEventStep.cs">
//   SPDX-License-Identifier: MIT
//   Copyright © 2019-2024 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.Steps.Record
{
    #region Using Directives

    using System;
    using Mocklis.Core;

    #endregion

    /// <summary>
    ///     This class represents a 'Record' step that records instances of event handlers being added.
    ///     Inherits from the <see cref="RecordEventStepBase{THandler, TRecord}" /> class.
    /// </summary>
    /// <typeparam name="THandler">The event handler type for the event.</typeparam>
    /// <typeparam name="TRecord">The type of data recorded in the ledger.</typeparam>
    /// <seealso cref="RecordEventStepBase{THandler, TRecord}" />
    public class RecordBeforeAddEventStep<THandler, TRecord> : RecordEventStepBase<THandler, TRecord> where THandler : Delegate
    {
        private readonly Func<THandler?, TRecord> _selector;

        /// <summary>
        ///     Initializes a new instance of the <see cref="RecordBeforeAddEventStep{THandler, TRecord}" /> class.
        /// </summary>
        /// <param name="selector">
        ///     A Func that constructs an entry for when an event handler is added.
        ///     Takes the event handler as parameter.
        /// </param>
        public RecordBeforeAddEventStep(Func<THandler?, TRecord> selector)
        {
            _selector = selector ?? throw new ArgumentNullException(nameof(selector));
        }

        /// <summary>
        ///     Called when an event handler is being added to the mocked event.
        ///     This implementation records the add in the ledger before it's forwarded on.
        /// </summary>
        /// <param name="mockInfo">Information about the mock through which the event handler is being added.</param>
        /// <param name="value">The event handler that is being added.</param>
        public override void Add(IMockInfo mockInfo, THandler? value)
        {
            Add(_selector(value));
            base.Add(mockInfo, value);
        }
    }
}
