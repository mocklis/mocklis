// --------------------------------------------------------------------------------------------------------------------
// <copyright file="InstanceIfMethodStep.cs">
//   Copyright © 2019 Esbjörn Redmo and contributors. All rights reserved.
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
    public class InstanceIfMethodStep<TResult> : IfMethodStepBase<ValueTuple, TResult>
    {
        private readonly Func<object, bool> _condition;

        /// <summary>
        ///     Initializes a new instance of the <see cref="InstanceIfMethodStep{TResult}" /> class.
        /// </summary>
        /// <param name="condition">
        ///     A condition evaluated when the method is called. If <c>true</c>, the alternative branch is
        ///     taken.
        /// </param>
        /// <param name="branch">
        ///     An action to set up the alternative branch; it also provides a means of re-joining the normal
        ///     branch.
        /// </param>
        public InstanceIfMethodStep(Func<object, bool> condition,
            Action<IfBranchCaller> branch) : base(branch)
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
            if (_condition?.Invoke(mockInfo.MockInstance) ?? false)
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
    public class InstanceIfMethodStep<TParam, TResult> : IfMethodStepBase<TParam, TResult>
    {
        private readonly Func<object, TParam, bool> _condition;

        /// <summary>
        ///     Initializes a new instance of the <see cref="InstanceIfMethodStep{TParam, TResult}" /> class.
        /// </summary>
        /// <param name="condition">
        ///     A condition evaluated when the method is called. If <c>true</c>, the alternative branch is
        ///     taken.
        /// </param>
        /// <param name="branch">
        ///     An action to set up the alternative branch; it also provides a means of re-joining the normal
        ///     branch.
        /// </param>
        public InstanceIfMethodStep(Func<object, TParam, bool> condition,
            Action<IfBranchCaller> branch) : base(branch)
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
            if (_condition?.Invoke(mockInfo.MockInstance, param) ?? false)
            {
                return IfBranch.Call(mockInfo, param);
            }

            return base.Call(mockInfo, param);
        }
    }
}
