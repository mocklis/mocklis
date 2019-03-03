// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ActionMethodStep.cs">
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
    ///     Class that represents an 'Action' method step.
    ///     Implements the <see cref="IMethodStep{TParam,TResult}" /> interface.
    /// </summary>
    /// <typeparam name="TParam">The method parameter type.</typeparam>
    /// <seealso cref="IMethodStep{TParam, ValueTuple}" />
    public class ActionMethodStep<TParam> : IMethodStep<TParam, ValueTuple>
    {
        private readonly Action<TParam> _action;

        /// <summary>
        ///     Initializes a new instance of the <see cref="ActionMethodStep{TParam}" /> class with an action.
        /// </summary>
        /// <param name="action">An action to take when the method is called.</param>
        public ActionMethodStep(Action<TParam> action)
        {
            _action = action;
        }

        /// <summary>
        ///     Called when the mocked method is called. This implementation runs the action with the given parameters.
        /// </summary>
        /// <param name="mockInfo">Information about the mock through which the method is called.</param>
        /// <param name="param">The parameters used.</param>
        /// <returns>The returned result.</returns>
        public ValueTuple Call(IMockInfo mockInfo, TParam param)
        {
            _action(param);
            return ValueTuple.Create();
        }
    }

    /// <summary>
    ///     Class that represents an 'Action' method step.
    ///     Implements the <see cref="IMethodStep{ValueTuple, ValueTuple}" /> interface.
    /// </summary>
    /// <seealso cref="IMethodStep{ValueTuple, ValueTuple}" />
    public class ActionMethodStep : IMethodStep<ValueTuple, ValueTuple>
    {
        private readonly Action _action;

        /// <summary>
        ///     Initializes a new instance of the <see cref="ActionMethodStep" /> class with an action.
        /// </summary>
        /// <param name="action">An action to take whet the method is called.</param>
        public ActionMethodStep(Action action)
        {
            _action = action;
        }

        /// <summary>
        ///     Called when the mocked method is called. This implementation runs the action.
        /// </summary>
        /// <param name="mockInfo">Information about the mock through which the method is called.</param>
        /// <param name="param">The parameters used.</param>
        /// <returns>The returned result.</returns>
        public ValueTuple Call(IMockInfo mockInfo, ValueTuple param)
        {
            _action();
            return ValueTuple.Create();
        }
    }
}
