// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ReturnEachPropertyStep.cs">
//   SPDX-License-Identifier: MIT
//   Copyright © 2019-2020 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.Steps.Return
{
    #region Using Directives

    using System;
    using System.Collections.Generic;
    using Mocklis.Core;

    #endregion


    /// <summary>
    ///     Class that represents a 'Return' property step that returns given set of values one-by-one, and then forwards on
    ///     reads.
    ///     Inherits from the <see cref="PropertyStepWithNext{TValue}" /> class.
    /// </summary>
    /// <typeparam name="TValue">The type of the property.</typeparam>
    /// <seealso cref="PropertyStepWithNext{TValue}" />
    public class ReturnEachPropertyStep<TValue> : PropertyStepWithNext<TValue>
    {
        private readonly object _lockObject = new object();
        private IEnumerator<TValue>? _values;

        /// <summary>
        ///     Initializes a new instance of the <see cref="ReturnEachPropertyStep{TValue}" /> class.
        /// </summary>
        /// <param name="values">The values to be returned one-by-one.</param>
        public ReturnEachPropertyStep(IEnumerable<TValue> values)
        {
            _values = (values ?? throw new ArgumentNullException(nameof(values))).GetEnumerator();
        }

        /// <summary>
        ///     Called when a value is read from the property.
        ///     This implementation returns the values provided one-by-one, and then forwards on reads.
        /// </summary>
        /// <param name="mockInfo">Information about the mock through which the value is read.</param>
        /// <returns>The value being read.</returns>
        public override TValue Get(IMockInfo mockInfo)
        {
            if (_values == null)
            {
                return base.Get(mockInfo);
            }

            lock (_lockObject)
            {
                if (_values != null)
                {
                    if (_values.MoveNext())
                    {
                        return _values.Current;
                    }

                    _values.Dispose();
                    _values = null;
                }
            }

            return base.Get(mockInfo);
        }
    }
}
