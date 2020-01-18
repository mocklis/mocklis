// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OnlySetIfChangedPropertyStep.cs">
//   SPDX-License-Identifier: MIT
//   Copyright © 2019 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.Steps.Conditional
{
    #region Using Directives

    using System.Collections.Generic;
    using Mocklis.Core;

    #endregion

    /// <summary>
    ///     Property step that will only forward a write to the next step if the value is different from
    ///     what can currently be read from the next step.
    ///     Inherits from the <see cref="PropertyStepWithNext{TValue}" /> class.
    /// </summary>
    /// <typeparam name="TValue">The type of the property.</typeparam>
    /// <seealso cref="PropertyStepWithNext{TValue}" />
    public class OnlySetIfChangedPropertyStep<TValue> : PropertyStepWithNext<TValue>
    {
        private IEqualityComparer<TValue> Comparer { get; }

        /// <summary>
        ///     Initializes a new instance of the <see cref="OnlySetIfChangedPropertyStep{TValue}" /> class.
        /// </summary>
        /// <param name="comparer">
        ///     An optional <see cref="IEqualityComparer{TValue}" /> that is used to determine whether the new
        ///     value is different from the current one.
        /// </param>
        public OnlySetIfChangedPropertyStep(IEqualityComparer<TValue>? comparer = null)
        {
            Comparer = comparer ?? EqualityComparer<TValue>.Default;
        }


        /// <summary>
        ///     Called when a value is written to the property. This implementation first gets the current value, and only calls
        ///     next if this value differs from the one that is to be written.
        /// </summary>
        /// <param name="mockInfo">Information about the mock through which the value is written.</param>
        /// <param name="value">The value being written.</param>
        public override void Set(IMockInfo mockInfo, TValue value)
        {
            if (!Comparer.Equals(Get(mockInfo), value))
            {
                base.Set(mockInfo, value);
            }
        }
    }
}
