// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TimesPropertyStep.cs">
//   SPDX-License-Identifier: MIT
//   Copyright © 2019-2020 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.Steps.Times
{
    #region Using Directives

    using System;
    using Mocklis.Core;

    #endregion

    /// <summary>
    ///     Class that represents a 'Repetition' property step that takes an alternative branch a given number of times, and
    ///     the normal branch thereafter.
    ///     Inherits from the <see cref="PropertyStepWithNext{TValue}" /> class.
    /// </summary>
    /// <typeparam name="TValue">The type of the property.</typeparam>
    /// <seealso cref="PropertyStepWithNext{TValue}" />
    public class TimesPropertyStep<TValue> : PropertyStepWithNext<TValue>
    {
        private readonly object _lockObject = new object();
        private readonly int _times;
        private int _calls;
        private readonly PropertyStepWithNext<TValue> _branch = new PropertyStepWithNext<TValue>();

        /// <summary>
        ///     Initializes a new instance of the <see cref="TimesPropertyStep{TValue}" /> class.
        /// </summary>
        /// <param name="times">The number of times the alternative branch should be taken.</param>
        /// <param name="branch">An action to set up the alternative branch.</param>
        public TimesPropertyStep(int times, Action<ICanHaveNextPropertyStep<TValue>> branch)
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
        ///     Called when a value is read from the property.
        ///     This will chose the alternative branch for a given number of reads or writes (counted together) and the normal
        ///     branch afterwards.
        /// </summary>
        /// <param name="mockInfo">Information about the mock through which the value is read.</param>
        /// <returns>The value being read.</returns>
        public override TValue Get(IMockInfo mockInfo)
        {
            return ShouldUseBranch() ? _branch.Get(mockInfo) : base.Get(mockInfo);
        }

        /// <summary>
        ///     Called when a value is written to the property.
        ///     This will chose the alternative branch for a given number of reads or writes (counted together) and the normal
        ///     branch afterwards.
        /// </summary>
        /// <param name="mockInfo">Information about the mock through which the value is written.</param>
        /// <param name="value">The value being written.</param>
        public override void Set(IMockInfo mockInfo, TValue value)
        {
            if (ShouldUseBranch())
            {
                _branch.Set(mockInfo, value);
            }
            else
            {
                base.Set(mockInfo, value);
            }
        }
    }
}
