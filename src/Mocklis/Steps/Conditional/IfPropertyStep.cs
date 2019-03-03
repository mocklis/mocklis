// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IfPropertyStep.cs">
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
    ///     Property step with an alternative set of steps that can be chosen given the provided conditions.
    ///     Inherits from the <see cref="Mocklis.Steps.Conditional.IfPropertyStepBase{TValue}" /> class.
    /// </summary>
    /// <typeparam name="TValue">The type of the property.</typeparam>
    /// <seealso cref="Mocklis.Steps.Conditional.IfPropertyStepBase{TValue}" />
    public class IfPropertyStep<TValue> : IfPropertyStepBase<TValue>
    {
        private readonly Func<bool> _getCondition;
        private readonly Func<TValue, bool> _setCondition;

        /// <summary>
        ///     Initializes a new instance of the <see cref="IfPropertyStep{TValue}" /> class.
        /// </summary>
        /// <param name="getCondition">A condition evaluated when a value is read. If <c>true</c>, the alternative branch is taken.</param>
        /// <param name="setCondition">
        ///     A condition evaluated when a value is written. If <c>true</c>, the alternative branch is
        ///     taken.
        /// </param>
        /// <param name="branch">
        ///     An action to set up the alternative branch; it also provides a means of re-joining the normal
        ///     branch.
        /// </param>
        public IfPropertyStep(Func<bool> getCondition, Func<TValue, bool> setCondition,
            Action<IfBranchCaller> branch) : base(branch)
        {
            _getCondition = getCondition;
            _setCondition = setCondition;
        }

        /// <summary>
        ///     Called when a value is read from the property.
        ///     This implementation will select the alternative branch if the get condition evaluates to <c>true</c>.
        /// </summary>
        /// <param name="mockInfo">Information about the mock through which the value is read.</param>
        /// <returns>The value being read.</returns>
        public override TValue Get(IMockInfo mockInfo)
        {
            if (_getCondition?.Invoke() ?? false)
            {
                return IfBranch.Get(mockInfo);
            }

            return base.Get(mockInfo);
        }

        /// <summary>
        ///     Called when a value is written to the property.
        ///     This implementation will select the alternative branch if the set condition evaluates to <c>true</c>.
        /// </summary>
        /// <param name="mockInfo">Information about the mock through which the value is written.</param>
        /// <param name="value">The value being written.</param>
        public override void Set(IMockInfo mockInfo, TValue value)
        {
            if (_setCondition?.Invoke(value) ?? false)
            {
                IfBranch.Set(mockInfo, value);
            }
            else
            {
                base.Set(mockInfo, value);
            }
        }
    }
}
