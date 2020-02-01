// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SetActionPropertyStep.cs">
//   SPDX-License-Identifier: MIT
//   Copyright © 2019-2020 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.Steps.Lambda
{
    #region Using Directives

    using System;
    using Mocklis.Core;

    #endregion

    /// <summary>
    ///     Class that represents a 'SetAction' property step.
    ///     Inherits from the <see cref="PropertyStepWithNext{TValue}" /> class.
    /// </summary>
    /// <typeparam name="TValue">The type of the property.</typeparam>
    /// <seealso cref="PropertyStepWithNext{TValue}" />
    public class SetActionPropertyStep<TValue> : PropertyStepWithNext<TValue>
    {
        private readonly Action<TValue> _action;

        /// <summary>
        ///     Initializes a new instance of the <see cref="SetActionPropertyStep{TValue}" /> class.
        /// </summary>
        /// <param name="action">An action to be invoked when the property is written to.</param>
        public SetActionPropertyStep(Action<TValue> action)
        {
            _action = action ?? throw new ArgumentNullException(nameof(action));
        }

        /// <summary>
        ///     Called when a value is written to the property.
        ///     This implementation invokes the action with the given value.
        /// </summary>
        /// <param name="mockInfo">Information about the mock through which the value is written.</param>
        /// <param name="value">The value being written.</param>
        public override void Set(IMockInfo mockInfo, TValue value)
        {
            _action(value);
        }
    }
}
