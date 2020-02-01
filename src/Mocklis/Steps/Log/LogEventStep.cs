// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LogEventStep.cs">
//   SPDX-License-Identifier: MIT
//   Copyright © 2019-2020 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.Steps.Log
{
    #region Using Directives

    using System;
    using Mocklis.Core;

    #endregion

    /// <summary>
    ///     Class that represents a 'Log' event step. This class cannot be inherited.
    ///     Inherits from the <see cref="EventStepWithNext{THandler}" /> class.
    /// </summary>
    /// <typeparam name="THandler">The event handler type for the event.</typeparam>
    /// <seealso cref="EventStepWithNext{THandler}" />
    public sealed class LogEventStep<THandler> : EventStepWithNext<THandler> where THandler : Delegate
    {
        private readonly ILogContext _logContext;

        /// <summary>
        ///     Initializes a new instance of the <see cref="LogEventStep{THandler}" /> class.
        /// </summary>
        /// <param name="logContext">The log context used to write log lines.</param>
        public LogEventStep(ILogContext logContext)
        {
            _logContext = logContext ?? throw new ArgumentNullException(nameof(logContext));
        }

        /// <summary>
        ///     Called when an event handler is being added to the mocked event.
        ///     This implementation logs before and after the event has been added, along with any exceptions thrown.
        /// </summary>
        /// <param name="mockInfo">Information about the mock through which the event handler is being added.</param>
        /// <param name="value">The event handler that is being added.</param>
        public override void Add(IMockInfo mockInfo, THandler? value)
        {
            _logContext.LogBeforeEventAdd(mockInfo, value);
            try
            {
                base.Add(mockInfo, value);
            }
            catch (Exception exception)
            {
                _logContext.LogEventAddException(mockInfo, exception);
                throw;
            }

            _logContext.LogAfterEventAdd(mockInfo);
        }

        /// <summary>
        ///     Called when an event handler is being removed from the mocked event.
        ///     This implementation logs before and after the event has been removed, along with any exceptions thrown.
        /// </summary>
        /// <param name="mockInfo">Information about the mock through which the event handler is being removed.</param>
        /// <param name="value">The event handler that is being removed.</param>
        public override void Remove(IMockInfo mockInfo, THandler? value)
        {
            _logContext.LogBeforeEventRemove(mockInfo, value);
            try
            {
                base.Remove(mockInfo, value);
            }
            catch (Exception exception)
            {
                _logContext.LogEventRemoveException(mockInfo, exception);
                throw;
            }

            _logContext.LogAfterEventRemove(mockInfo);
        }
    }
}
