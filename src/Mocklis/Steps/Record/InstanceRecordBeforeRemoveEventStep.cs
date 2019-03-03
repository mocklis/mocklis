// --------------------------------------------------------------------------------------------------------------------
// <copyright file="InstanceRecordBeforeRemoveEventStep.cs">
//   Copyright © 2019 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.Steps.Record
{
    #region Using Directives

    using System;
    using Mocklis.Core;

    #endregion

    /// <summary>
    ///     This class represents a 'Record' step that records instances of event handlers being removed.
    ///     Inherits from the <see cref="RecordEventStepBase{THandler, TRecord}" /> class.
    /// </summary>
    /// <typeparam name="THandler">The event handler type for the event.</typeparam>
    /// <typeparam name="TRecord">The type of data recorded in the ledger.</typeparam>
    /// <seealso cref="RecordEventStepBase{THandler, TRecord}" />
    public class InstanceRecordBeforeRemoveEventStep<THandler, TRecord> : RecordEventStepBase<THandler, TRecord> where THandler : Delegate
    {
        private readonly Func<object, THandler, TRecord> _selection;

        /// <summary>
        ///     Initializes a new instance of the <see cref="InstanceRecordBeforeRemoveEventStep{THandler, TRecord}" /> class.
        /// </summary>
        /// <param name="selection">
        ///     A Func that selects what we want to record. Takes the entire state of the mock and the event as
        ///     parameters.
        /// </param>
        public InstanceRecordBeforeRemoveEventStep(Func<object, THandler, TRecord> selection)
        {
            _selection = selection ?? throw new ArgumentNullException(nameof(selection));
        }

        /// <summary>
        ///     Called when an event handler is being removed from the mocked event.
        ///     This implementation records the remove in the ledger before it's forwarded on.
        /// </summary>
        /// <param name="mockInfo">Information about the mock through which the event handler is being removed.</param>
        /// <param name="value">The event handler that is being removed.</param>
        public override void Remove(IMockInfo mockInfo, THandler value)
        {
            Add(_selection(mockInfo.MockInstance, value));
            base.Remove(mockInfo, value);
        }
    }
}
