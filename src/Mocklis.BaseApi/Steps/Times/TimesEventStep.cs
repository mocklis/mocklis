// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TimesEventStep.cs">
//   SPDX-License-Identifier: MIT
//   Copyright © 2019-2021 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.Steps.Times
{
    #region Using Directives

    using System;
    using Mocklis.Core;

    #endregion

    /// <summary>
    ///     Class that represents a 'Repetition' event step that takes an alternative branch a given number of times, and the
    ///     normal branch thereafter.
    ///     Inherits from the <see cref="EventStepWithNext{THandler}" /> class.
    /// </summary>
    /// <typeparam name="THandler">The event handler type for the event.</typeparam>
    /// <seealso cref="EventStepWithNext{THandler}" />
    public class TimesEventStep<THandler> : EventStepWithNext<THandler> where THandler : Delegate
    {
        private readonly object _lockObject = new object();
        private readonly int _times;
        private int _calls;
        private readonly EventStepWithNext<THandler> _branch = new EventStepWithNext<THandler>();

        /// <summary>
        ///     Initializes a new instance of the <see cref="TimesEventStep{THandler}" /> class.
        /// </summary>
        /// <param name="times">The number of times the alternative branch should be taken.</param>
        /// <param name="branch">An action to set up the alternative branch.</param>
        public TimesEventStep(int times, Action<ICanHaveNextEventStep<THandler>> branch)
        {
            _times = times;
            branch?.Invoke(_branch);
        }

        private bool ShouldUseBranch()
        {
            lock (_lockObject)
            {
                if (_calls < _times)
                {
                    _calls++;
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        ///     Called when an event handler is being added to the mocked event.
        ///     This will chose the alternative branch for a given number of adds or removes (counted together) and the normal
        ///     branch afterwards.
        /// </summary>
        /// <param name="mockInfo">Information about the mock through which the event handler is being added.</param>
        /// <param name="value">The event handler that is being added.</param>
        public override void Add(IMockInfo mockInfo, THandler? value)
        {
            if (ShouldUseBranch())
            {
                _branch.Add(mockInfo, value);
            }
            else
            {
                base.Add(mockInfo, value);
            }
        }

        /// <summary>
        ///     Called when an event handler is being removed from the mocked event.
        ///     This will chose the alternative branch for a given number of adds or removes (counted together) and the normal
        ///     branch afterwards.
        /// </summary>
        /// <param name="mockInfo">Information about the mock through which the event handler is being removed.</param>
        /// <param name="value">The event handler that is being removed.</param>
        public override void Remove(IMockInfo mockInfo, THandler? value)
        {
            if (ShouldUseBranch())
            {
                _branch.Remove(mockInfo, value);
            }
            else
            {
                base.Remove(mockInfo, value);
            }
        }
    }
}
