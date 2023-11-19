// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ICanHaveNextMethodStep.cs">
//   SPDX-License-Identifier: MIT
//   Copyright © 2019-2023 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.Core
{
    #region Using Directives

    using System.ComponentModel;

    #endregion

    /// <summary>
    ///     Interface implemented by method mocks and steps that can forward calls to a 'next' step.
    /// </summary>
    /// <typeparam name="TParam">The method parameter type.</typeparam>
    /// <typeparam name="TResult">The method return type.</typeparam>
    public interface ICanHaveNextMethodStep<out TParam, in TResult>
    {
        /// <summary>
        ///     Replaces the current 'next' step with a new step.
        /// </summary>
        /// <typeparam name="TStep">The actual type of the new step.</typeparam>
        /// <param name="step">The new step.</param>
        /// <returns>The new step, so that we can add further steps in a fluent fashion.</returns>
        [EditorBrowsable(EditorBrowsableState.Never)]
        TStep SetNextStep<TStep>(TStep step) where TStep : IMethodStep<TParam, TResult>;
    }
}
