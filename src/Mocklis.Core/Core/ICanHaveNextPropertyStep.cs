// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ICanHaveNextPropertyStep.cs">
//   SPDX-License-Identifier: MIT
//   Copyright © 2019-2020 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.Core
{
    #region Using Directives

    using System.ComponentModel;

    #endregion

    /// <summary>
    ///     Interface implemented by property mocks and steps that can forward calls to a 'next' step.
    /// </summary>
    /// <typeparam name="TValue">The type of the property.</typeparam>
    public interface ICanHaveNextPropertyStep<TValue>
    {
        /// <summary>
        ///     Replaces the current 'next' step with a new step.
        /// </summary>
        /// <typeparam name="TStep">The actual type of the new step.</typeparam>
        /// <param name="step">The new step.</param>
        /// <returns>The new step, so that we can add further steps in a fluent fashion.</returns>
        [EditorBrowsable(EditorBrowsableState.Never)]
        TStep SetNextStep<TStep>(TStep step) where TStep : IPropertyStep<TValue>;
    }
}
