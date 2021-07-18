// --------------------------------------------------------------------------------------------------------------------
// <copyright file="InstanceGetFuncPropertyStep.cs">
//   SPDX-License-Identifier: MIT
//   Copyright © 2019-2021 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.Steps.Lambda
{
    #region Using Directives

    using System;
    using Mocklis.Core;

    #endregion

    /// <summary>
    ///     Class that represents a 'GetFunc' property step, where the function can also depend on the current mock instance.
    ///     Inherits from the <see cref="PropertyStepWithNext{TValue}" /> class.
    /// </summary>
    /// <typeparam name="TValue">The type of the property.</typeparam>
    /// <seealso cref="PropertyStepWithNext{TValue}" />
    public class InstanceGetFuncPropertyStep<TValue> : PropertyStepWithNext<TValue>
    {
        private readonly Func<object, TValue> _func;

        /// <summary>
        ///     Initializes a new instance of the <see cref="InstanceGetFuncPropertyStep{TValue}" /> class.
        /// </summary>
        /// <param name="func">A function used to create a value when the property is read from.</param>
        public InstanceGetFuncPropertyStep(Func<object, TValue> func)
        {
            _func = func ?? throw new ArgumentNullException(nameof(func));
        }

        /// <summary>
        ///     Called when a value is read from the property.
        ///     This implementation evaluates the function with the mock instance as parameter and returns the result.
        /// </summary>
        /// <param name="mockInfo">Information about the mock through which the value is read.</param>
        /// <returns>The value being read.</returns>
        public override TValue Get(IMockInfo mockInfo)
        {
            return _func(mockInfo.MockInstance);
        }
    }
}
