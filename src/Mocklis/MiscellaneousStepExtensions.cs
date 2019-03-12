// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MiscellaneousStepExtensions.cs">
//   SPDX-License-Identifier: MIT
//   Copyright © 2019 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis
{
    #region Using Directives

    using System.ComponentModel;
    using Mocklis.Core;
    using Mocklis.Steps.Miscellaneous;
    using Mocklis.Verification;

    #endregion

    /// <summary>
    ///     A class with extension methods for adding steps that didn't fit into any of the other groupings.
    /// </summary>
    public static class MiscellaneousStepExtensions
    {
        /// <summary>
        ///     Introduces a step that will invoke the given <see cref="PropertyChangedEventHandler" /> with the name of the
        ///     property when set. It will also forward on all calls to the next step.
        /// </summary>
        /// <typeparam name="TValue">The type of the property.</typeparam>
        /// <param name="caller">The mock or step to which this 'miscellaneous' step is added.</param>
        /// <param name="propertyChangedEvent">The property changed event.</param>
        /// <returns>An <see cref="ICanHaveNextPropertyStep{TValue}" /> that can be used to add further steps.</returns>
        public static ICanHaveNextPropertyStep<TValue> RaisePropertyChangedEvent<TValue>(
            this ICanHaveNextPropertyStep<TValue> caller,
            IStoredEvent<PropertyChangedEventHandler> propertyChangedEvent)
        {
            return caller.SetNextStep(new RaisePropertyChangedEventPropertyStep<TValue>(propertyChangedEvent));
        }
    }
}
