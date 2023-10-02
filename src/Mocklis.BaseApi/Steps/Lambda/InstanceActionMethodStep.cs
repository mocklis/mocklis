// --------------------------------------------------------------------------------------------------------------------
// <copyright file="InstanceActionMethodStep.cs">
//   SPDX-License-Identifier: MIT
//   Copyright © 2019-2023 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.Steps.Lambda
{
    #region Using Directives

    using System;
    using Mocklis.Core;

    #endregion

    /// <summary>
    ///     Class that represents an 'Action' method step, where the action can depend on the state of the mock.
    ///     Implements the <see cref="IMethodStep{TParam,TResult}" /> interface.
    /// </summary>
    /// <typeparam name="TParam">The method parameter type.</typeparam>
    /// <seealso cref="IMethodStep{TParam, ValueTuple}" />
    public class InstanceActionMethodStep<TParam> : IMethodStep<TParam, ValueTuple>
    {
        private readonly Action<object, TParam> _action;

        /// <summary>
        ///     Initializes a new instance of the <see cref="InstanceActionMethodStep{TParam}" /> class.
        /// </summary>
        /// <param name="action">An action to take when the method is called.</param>
        public InstanceActionMethodStep(Action<object, TParam> action)
        {
            _action = action ?? throw new ArgumentNullException(nameof(action));
        }

        /// <summary>
        ///     Called when the mocked method is called. This implementation runs the action with the mock instance and given
        ///     parameters.
        /// </summary>
        /// <param name="mockInfo">Information about the mock through which the method is called.</param>
        /// <param name="param">The parameters used.</param>
        /// <returns>The returned result.</returns>
        public ValueTuple Call(IMockInfo mockInfo, TParam param)
        {
            _action(mockInfo.MockInstance, param);
            return ValueTuple.Create();
        }
    }

    /// <summary>
    ///     Class that represents an 'Action' method step, where the action can depend on the state of the mock.
    ///     Implements the <see cref="IMethodStep{ValueTuple, ValueTuple}" /> interface.
    /// </summary>
    /// <seealso cref="IMethodStep{ValueTuple, ValueTuple}" />
    public class InstanceActionMethodStep : IMethodStep<ValueTuple, ValueTuple>
    {
        private readonly Action<object> _action;

        /// <summary>
        ///     Initializes a new instance of the <see cref="InstanceActionMethodStep" /> class.
        /// </summary>
        /// <param name="action">An action to take when the method is called.</param>
        public InstanceActionMethodStep(Action<object> action)
        {
            _action = action ?? throw new ArgumentNullException(nameof(action));
        }

        /// <summary>
        ///     Called when the mocked method is called. This implementation runs the action with the mock instance.
        /// </summary>
        /// <param name="mockInfo">Information about the mock through which the method is called.</param>
        /// <param name="param">The parameters used.</param>
        /// <returns>The returned result.</returns>
        public ValueTuple Call(IMockInfo mockInfo, ValueTuple param)
        {
            _action(mockInfo.MockInstance);
            return ValueTuple.Create();
        }
    }
}
