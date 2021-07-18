// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IfMethodStep.cs">
//   SPDX-License-Identifier: MIT
//   Copyright © 2019-2021 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.Steps.Conditional
{
    #region Using Directives

    using System;
    using Mocklis.Core;

    #endregion

    /// <summary>
    ///     Method step with an alternative set of steps that can be chosen given the provided conditions.
    ///     Inherits from the <see cref="IfMethodStepBase{ValueTuple, TResult}" />
    /// </summary>
    /// <typeparam name="TResult">The method return type.</typeparam>
    /// <seealso cref="IfMethodStepBase{ValueTuple, TResult}" />
    public class IfMethodStep<TResult> : IfMethodStepBase<ValueTuple, TResult>
    {
        private readonly Func<bool>? _condition;

        /// <summary>
        ///     Initializes a new instance of the <see cref="IfMethodStep{TResult}" /> class.
        /// </summary>
        /// <param name="condition">
        ///     A condition evaluated when the method is called. If <c>true</c>, the alternative branch is
        ///     taken.
        /// </param>
        /// <param name="branch">
        ///     An action to set up the alternative branch; it also provides a means of re-joining the normal
        ///     branch.
        /// </param>
        public IfMethodStep(Func<bool>? condition, Action<IfBranchCaller> branch) : base(branch)
        {
            _condition = condition;
        }

        /// <summary>
        ///     Called when the mocked method is called.
        ///     This implementation will select the alternative branch if the condition evaluates to <c>true</c>.
        /// </summary>
        /// <param name="mockInfo">Information about the mock through which the method is called.</param>
        /// <param name="param">The parameters used.</param>
        /// <returns>The returned result.</returns>
        public override TResult Call(IMockInfo mockInfo, ValueTuple param)
        {
            if (_condition?.Invoke() ?? false)
            {
                return IfBranch.Call(mockInfo, param);
            }

            return base.Call(mockInfo, param);
        }
    }

    /// <summary>
    ///     Method step with an alternative set of steps that can be chosen given the provided conditions.
    ///     Inherits from the <see cref="IfMethodStepBase{ValueTuple, TResult}" />
    /// </summary>
    /// <typeparam name="TParam">The method parameter type.</typeparam>
    /// <typeparam name="TResult">The method return type.</typeparam>
    /// <seealso cref="IfMethodStepBase{ValueTuple, TResult}" />
    public class IfMethodStep<TParam, TResult> : IfMethodStepBase<TParam, TResult>
    {
        private readonly Func<TParam, bool>? _condition;

        /// <summary>
        ///     Initializes a new instance of the <see cref="IfMethodStep{TParam, TResult}" /> class.
        /// </summary>
        /// <param name="condition">
        ///     A condition evaluated when the method is called. If <c>true</c>, the alternative branch is
        ///     taken.
        /// </param>
        /// <param name="branch">
        ///     An action to set up the alternative branch; it also provides a means of re-joining the normal
        ///     branch.
        /// </param>
        public IfMethodStep(Func<TParam, bool>? condition, Action<IfBranchCaller> branch) : base(branch)
        {
            _condition = condition;
        }

        /// <summary>
        ///     Called when the mocked method is called.
        ///     This implementation will select the alternative branch if the condition evaluates to <c>true</c>.
        /// </summary>
        /// <param name="mockInfo">Information about the mock through which the method is called.</param>
        /// <param name="param">The parameters used.</param>
        /// <returns>The returned result.</returns>
        public override TResult Call(IMockInfo mockInfo, TParam param)
        {
            if (_condition?.Invoke(param) ?? false)
            {
                return IfBranch.Call(mockInfo, param);
            }

            return base.Call(mockInfo, param);
        }
    }
}
