// --------------------------------------------------------------------------------------------------------------------
// <copyright file="InstanceFuncMethodStep.cs">
//   SPDX-License-Identifier: MIT
//   Copyright © 2019 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.Steps.Lambda
{
    #region Using Directives

    using System;
    using Mocklis.Core;

    #endregion

    /// <summary>
    ///     Class that represents a 'Func' method step, where the function can depend on the state of the mock.
    ///     Implements the <see cref="IMethodStep{TParam,TResult}" /> interface.
    /// </summary>
    /// <typeparam name="TParam">The method parameter type.</typeparam>
    /// <typeparam name="TResult">The method return type.</typeparam>
    /// <seealso cref="IMethodStep{TParam, TResult}" />
    public class InstanceFuncMethodStep<TParam, TResult> : IMethodStep<TParam, TResult>
    {
        private readonly Func<object, TParam, TResult> _func;

        /// <summary>
        ///     Initializes a new instance of the <see cref="InstanceFuncMethodStep{TParam, TResult}" /> class.
        /// </summary>
        /// <param name="func">A function that is used to create a return value when the method is called.</param>
        public InstanceFuncMethodStep(Func<object, TParam, TResult> func)
        {
            _func = func;
        }

        /// <summary>
        ///     Called when the mocked method is called. This implementation calls the function with the mock instance and given
        ///     parameters and returns the result.
        /// </summary>
        /// <param name="mockInfo">Information about the mock through which the method is called.</param>
        /// <param name="param">The parameters used.</param>
        /// <returns>The returned result.</returns>
        public TResult Call(IMockInfo mockInfo, TParam param)
        {
            return _func(mockInfo.MockInstance, param);
        }
    }

    /// <summary>
    ///     Class that represents a 'Func' method step, where the function can depend on the state of the mock.
    ///     Implements the <see cref="IMethodStep{ValueTuple, TResult}" /> interface.
    /// </summary>
    /// <typeparam name="TResult">The method return type.</typeparam>
    /// <seealso cref="IMethodStep{ValueTuple, TResult}" />
    public class InstanceFuncMethodStep<TResult> : IMethodStep<ValueTuple, TResult>
    {
        private readonly Func<object, TResult> _func;

        /// <summary>
        ///     Initializes a new instance of the <see cref="InstanceFuncMethodStep{TResult}" /> class.
        /// </summary>
        /// <param name="func">A function that is used to create a return value when the method is called.</param>
        public InstanceFuncMethodStep(Func<object, TResult> func)
        {
            _func = func;
        }

        /// <summary>
        ///     Called when the mocked method is called. This implementation calls the function with the mock instance and returns
        ///     the result.
        /// </summary>
        /// <param name="mockInfo">Information about the mock through which the method is called.</param>
        /// <param name="param">The parameters used.</param>
        /// <returns>The returned result.</returns>
        public TResult Call(IMockInfo mockInfo, ValueTuple param)
        {
            return _func(mockInfo.MockInstance);
        }
    }
}
