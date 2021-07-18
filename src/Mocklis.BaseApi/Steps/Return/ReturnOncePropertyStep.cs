// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ReturnOncePropertyStep.cs">
//   SPDX-License-Identifier: MIT
//   Copyright © 2019-2021 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.Steps.Return
{
    #region Using Directives

    using System.Threading;
    using Mocklis.Core;

    #endregion

    /// <summary>
    ///     Class that represents a 'Return' property step that returns a given value once and then forwards on reads.
    ///     Inherits from the <see cref="PropertyStepWithNext{TValue}" /> class.
    /// </summary>
    /// <typeparam name="TValue">The type of the property.</typeparam>
    /// <seealso cref="PropertyStepWithNext{TValue}" />
    public class ReturnOncePropertyStep<TValue> : PropertyStepWithNext<TValue>
    {
        private readonly TValue _value;
        private int _returnCount;

        /// <summary>
        ///     Initializes a new instance of the <see cref="ReturnOncePropertyStep{TValue}" /> class.
        /// </summary>
        /// <param name="value">The value to be returned.</param>
        public ReturnOncePropertyStep(TValue value)
        {
            _value = value;
        }

        /// <summary>
        ///     Called when a value is read from the property.
        ///     This implementation returns the given value once, and then forwards on reads.
        /// </summary>
        /// <param name="mockInfo">Information about the mock through which the value is read.</param>
        /// <returns>The value being read.</returns>
        public override TValue Get(IMockInfo mockInfo)
        {
            if (Interlocked.Exchange(ref _returnCount, 1) == 0)
            {
                return _value;
            }

            return base.Get(mockInfo);
        }
    }
}
