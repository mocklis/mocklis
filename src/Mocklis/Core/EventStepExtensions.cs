// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EventStepExtensions.cs">
//   SPDX-License-Identifier: MIT
//   Copyright © 2019 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.Core
{
    #region Using Directives

    using System;

    #endregion

    /// <summary>
    ///     Extension methods for the IEventStep interface.
    /// </summary>
    public static class EventStepExtensions
    {
        /// <summary>
        ///     Adds an event handler using an event step. If the step is <c>null</c>, uses the strictness of the mock
        ///     to decide whether to throw a <see cref="MockMissingException" /> (VeryStrict) or to do nothing
        ///     (Lenient or Strict).
        /// </summary>
        /// <typeparam name="THandler">The event handler type for the event.</typeparam>
        /// <param name="eventStep">The event step (can be null) through which the event handler is being added.</param>
        /// <param name="mockInfo">Information about the mock through which the event handler is being added.</param>
        /// <param name="value">The event handler that is being added.</param>
        public static void AddWithStrictnessCheckIfNull<THandler>(this IEventStep<THandler>? eventStep, IMockInfo mockInfo, THandler? value)
            where THandler : Delegate
        {
            if (eventStep == null)
            {
                if (mockInfo.Strictness != Strictness.VeryStrict)
                {
                    return;
                }

                throw new MockMissingException(MockType.EventAdd, mockInfo);
            }

            eventStep.Add(mockInfo, value);
        }

        /// <summary>
        ///     Removes an event handler using an event step. If the step is <c>null</c>, use the strictness of the mock
        ///     to decide whether to throw a <see cref="MockMissingException" /> (VeryStrict) or to do nothing
        ///     (Lenient or Strict).
        /// </summary>
        /// <typeparam name="THandler">The event handler type for the event.</typeparam>
        /// <param name="eventStep">The event step (can be null) through which the event handler is being added.</param>
        /// <param name="mockInfo">Information about the mock through which the event handler is being added.</param>
        /// <param name="value">The event handler that is being added.</param>
        public static void RemoveWithStrictnessCheckIfNull<THandler>(this IEventStep<THandler>? eventStep, IMockInfo mockInfo, THandler? value)
            where THandler : Delegate
        {
            if (eventStep == null)
            {
                if (mockInfo.Strictness != Strictness.VeryStrict)
                {
                    return;
                }

                throw new MockMissingException(MockType.EventRemove, mockInfo);
            }

            eventStep.Remove(mockInfo, value);
        }
    }
}
