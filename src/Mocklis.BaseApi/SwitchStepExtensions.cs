// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SwitchStepExtensions.cs">
//   SPDX-License-Identifier: MIT
//   Copyright © 2019-2024 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis
{
    #region Using Directives

    using Mocklis.Core;
    using Mocklis.Steps.Switch;

    #endregion

    /// <summary>
    ///     A class with extension methods for adding 'switch' steps to an existing mock or step.
    /// </summary>
    public static class SwitchStepExtensions
    {
        /// <summary>
        ///     Introduces a step that allows for different handling depending on the parameters passed to the method,
        ///     similar to a switch statement, optionally removing each 'case' as it's been used.
        /// </summary>
        /// <typeparam name="TParam">The method parameter type.</typeparam>
        /// <typeparam name="TResult">The method return type.</typeparam>
        /// <param name="caller">The mock or step to which this 'switch' step is added.</param>
        /// <param name="consumeOnUse">If <c>true</c> then each 'case' is only usable once. Defaults to <c>false</c>.</param>
        /// <returns>A <see cref="SwitchMethodStep{TParam,TResult}"/> that can be used to set up the mock.</returns>
        public static SwitchMethodStep<TParam, TResult> Switch<TParam, TResult>(this ICanHaveNextMethodStep<TParam, TResult> caller,
            bool consumeOnUse = false)
        {
            var nextStep = new SwitchMethodStep<TParam, TResult>(consumeOnUse);
            caller.SetNextStep(nextStep);
            return nextStep;
        }
    }
}
