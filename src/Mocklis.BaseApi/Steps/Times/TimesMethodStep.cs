// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TimesMethodStep.cs">
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
    ///     Class that represents a 'Repetition' method step that takes an alternative branch a given number of times, and the
    ///     normal branch thereafter.
    ///     Inherits from the <see cref="MethodStepWithNext{TParam,TResult}" /> class.
    /// </summary>
    /// <typeparam name="TParam">The method parameter type.</typeparam>
    /// <typeparam name="TResult">The method return type.</typeparam>
    /// <seealso cref="MethodStepWithNext{TParam, TResult}" />
    public class TimesMethodStep<TParam, TResult> : MethodStepWithNext<TParam, TResult>
    {
        private readonly object _lockObject = new object();
        private readonly int _times;
        private int _calls;
        private readonly MethodStepWithNext<TParam, TResult> _branch = new MethodStepWithNext<TParam, TResult>();

        /// <summary>
        ///     Initializes a new instance of the <see cref="TimesMethodStep{TParam, TResult}" /> class.
        /// </summary>
        /// <param name="times">The number of times the alternative branch should be taken.</param>
        /// <param name="branch">An action to set up the alternative branch.</param>
        public TimesMethodStep(int times, Action<ICanHaveNextMethodStep<TParam, TResult>> branch)
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
        ///     Called when the mocked method is called.
        ///     This will chose the alternative branch for a given number of calls and the normal branch afterwards.
        /// </summary>
        /// <param name="mockInfo">Information about the mock through which the method is called.</param>
        /// <param name="param">The parameters used.</param>
        /// <returns>The returned result.</returns>
        public override TResult Call(IMockInfo mockInfo, TParam param)
        {
            return ShouldUseBranch() ? _branch.Call(mockInfo, param) : base.Call(mockInfo, param);
        }
    }
}
